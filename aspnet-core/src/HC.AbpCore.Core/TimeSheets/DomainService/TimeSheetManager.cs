

using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.Extensions;
using Abp.UI;
using Abp.Domain.Repositories;
using Abp.Domain.Services;

using HC.AbpCore;
using HC.AbpCore.TimeSheets;
using HC.AbpCore.Common;
using HC.AbpCore.DingTalk.Employees;
using HC.AbpCore.Projects;
using HC.AbpCore.DingTalk;
using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using HC.AbpCore.Messages;
using Senparc.CO2NET.Helpers;
using System.Text;
using Senparc.CO2NET.HttpUtility;
using HC.AbpCore.Messages.DomainService;

namespace HC.AbpCore.TimeSheets.DomainService
{
    /// <summary>
    /// TimeSheet领域层的业务管理
    ///</summary>
    public class TimeSheetManager :AbpCoreDomainServiceBase, ITimeSheetManager
    {
        private readonly IRepository<Project, Guid> _projectRepository;
        private readonly IRepository<Employee, string> _employeeRepository;
        private readonly IRepository<TimeSheet,Guid> _repository;
        private readonly IRepository<Message, Guid> _messageRepository;
        private readonly IDingTalkManager _dingTalkManager;

        /// <summary>
        /// TimeSheet的构造方法
        ///</summary>
        public TimeSheetManager(
			IRepository<TimeSheet, Guid> repository,
            IRepository<Project, Guid> projectRepository,
            IRepository<Employee, string> employeeRepository,
            IRepository<Message, Guid> messageRepository,
            IDingTalkManager dingTalkManager

        )
		{
            _repository =  repository;
            _employeeRepository = employeeRepository;
            _projectRepository = projectRepository;
            _dingTalkManager = dingTalkManager;
            _messageRepository = messageRepository;
        }


		/// <summary>
		/// 初始化
		///</summary>
		public void InitTimeSheet()
		{
			throw new NotImplementedException();
		}

        // TODO:编写领域业务代码


        /// <summary>
        /// 提交审批(return 1)  
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ResultCode> SubmitApproval(TimeSheet item)
        {
            ResultCode resultCode = new ResultCode();
            var config = await _dingTalkManager.GetDingDingConfigByAppAsync(DingDingAppEnum.智能办公);
            string accessToken = await _dingTalkManager.GetAccessTokenByAppAsync(DingDingAppEnum.智能办公);
            //var timeSheet = await _repository.GetAsync(Id);
            var project = await _projectRepository.GetAsync(item.ProjectId);
            if (project == null)
            {
                resultCode.Code = 1;
                resultCode.Msg = "所属项目不存在";
                return resultCode;
            }
            var employee = await _employeeRepository.GetAsync(item.EmployeeId);
            if (employee == null)
            {
                resultCode.Code = 2;
                resultCode.Msg = "员工不存在";
                return resultCode;
            }
            //entity.dept_id = deptIds[1];
            //entity.form_component_values = approvals;

            //DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/processinstance/create");
            var url = string.Format("https://oapi.dingtalk.com/topapi/processinstance/create?access_token={0}", accessToken);
            //OapiProcessinstanceCreateRequest request = new OapiProcessinstanceCreateRequest();
            SubmitApprovalEntity request = new SubmitApprovalEntity();
            request.process_code = "PROC-A9C47A1B-D617-42BE-A041-6C0CB767A79F";
            request.originator_user_id = item.EmployeeId;
            request.agent_id = config.AgentID;
            request.dept_id = Convert.ToInt32(employee.Department);
            List<Approval> formComponentValues = new List<Approval>();

            //OapiProcessinstanceCreateRequest.FormComponentValueVoDomain vo = new OapiProcessinstanceCreateRequest.FormComponentValueVoDomain();
            formComponentValues.Add(new Approval() { name = "所属项目", value = project.Name + "(" + project.ProjectCode + ")" });
            formComponentValues.Add(new Approval() { name = "工作日期", value = item.WorkeDate.ToString("yyyy-MM-dd") });
            formComponentValues.Add(new Approval() { name = "员工", value = employee.Name });
            formComponentValues.Add(new Approval() { name = "工时", value = item.Hour.HasValue? item.Hour.Value.ToString():null });
            formComponentValues.Add(new Approval() { name = "工作内容", value = item.Content});
            request.form_component_values = formComponentValues;
            //OapiProcessinstanceCreateResponse response = client.Execute(request, accessToken);
            ApprovalReturn approvalReturn = new ApprovalReturn();
            var jsonString = SerializerHelper.GetJsonString(request, null);
            using (MemoryStream ms = new MemoryStream())
            {
                var bytes = Encoding.UTF8.GetBytes(jsonString);
                ms.Write(bytes, 0, bytes.Length);
                ms.Seek(0, SeekOrigin.Begin);
                approvalReturn = Post.PostGetJson<ApprovalReturn>(url, null, ms);
            };
            if (approvalReturn.errcode == 0)
            {
                item.ProcessInstanceId = approvalReturn.process_instance_id;
                item.Status = TimeSheetStatusEnum.提交;
                await _repository.InsertAsync(item);
                return new ResultCode() { Code = 0, Msg = "提交成功" };
            }
            else
            {
                return new ResultCode() { Code = 4, Msg = "提交失败" };
            }
        }

        public async Task TimeSheetApprovalRemind(string accessToken, DingDingAppConfig dingDingAppConfig)
        {
            var projects = _projectRepository.GetAll();
            var employees = _employeeRepository.GetAll();
            var query = _repository.GetAll()
                .Where(aa => aa.Status == TimeSheetStatusEnum.提交);
            var timeSheets = from timeSheet in query
                             join project in projects on timeSheet.ProjectId equals project.Id
                             join employee in employees on timeSheet.EmployeeId equals employee.Id
                             select new
                             {
                                 ProjectName = project.Name + "(" + project.ProjectCode + ")",
                                 timeSheet.WorkeDate,
                                 timeSheet.Hour,
                                 timeSheet.Content,
                                 employee.Name,
                                 employee.Department,
                             };
            var items = await timeSheets.AsNoTracking().ToListAsync();
            var url = string.Format("https://oapi.dingtalk.com/topapi/message/corpconversation/asyncsend_v2?access_token={0}", accessToken);
            foreach (var item in items)
            {
                // TODO:提醒人后期需要完善
                var employeeIdList = await _employeeRepository.GetAll().Where(aa => aa.IsLeaderInDepts == "key:" + item.Department + "value:True").Select(aa => aa.Id)
                                     .Distinct().AsNoTracking().ToListAsync();
                //string employeeIds = string.Join(",", employeeIdList.ToArray());
                foreach (var employeeId in employeeIdList)
                {
                    Message message = new Message();
                    message.Content = string.Format("您好! 项目:{0}工时审批，员工:{1}，工作日期:{2},工时:{3},工作内容:{4}", item.ProjectName, item.Name, item.WorkeDate.ToString("yyyy-MM-dd"), item.Hour, item.Content);
                    message.SendTime = DateTime.Now;
                    message.Type = MessageTypeEnum.审批提醒;
                    message.IsRead = false;
                    message.EmployeeId = employeeId;
                    //新增到消息中心
                    var messageId = await _messageRepository.InsertAndGetIdAsync(message);

                    DingMsgs dingMsgs = new DingMsgs();
                    dingMsgs.userid_list = employeeId;
                    dingMsgs.to_all_user = false;
                    dingMsgs.agent_id = dingDingAppConfig.AgentID;
                    dingMsgs.msg.msgtype = "link";
                    dingMsgs.msg.link.title = "审批提醒";
                    dingMsgs.msg.link.text = string.Format("您好! 项目:{0}工时审批，员工:{1}，工作日期:{2},点击查看详情", item.ProjectName, item.Name, item.WorkeDate.ToString("yyyy-MM-dd"));
                    dingMsgs.msg.link.picUrl = "@lALPDeC2uQ_7MOHMgMyA";
                    dingMsgs.msg.link.messageUrl = "eapp://page/messages/detail-messages/detail-messages?id=" + messageId;
                    var jsonString = SerializerHelper.GetJsonString(dingMsgs, null);
                    MessageResponseResult response = new MessageResponseResult();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        var bytes = Encoding.UTF8.GetBytes(jsonString);
                        ms.Write(bytes, 0, bytes.Length);
                        ms.Seek(0, SeekOrigin.Begin);
                        response = Post.PostGetJson<MessageResponseResult>(url, null, ms);
                    };
                    //发送失败则自动删除消息中心对应数据
                    if (response.errcode != 0)
                    {
                        await _messageRepository.DeleteAsync(messageId);
                    }
                }
            }
        }

        /// <summary>
        /// 根据审批实例Id修改工时状态
        /// </summary>
        /// <param name="processInstanceId"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public async Task UpdateTimeSheetByPIIdAsync(string processInstanceId, string result)
        {
            var item = await _repository.FirstOrDefaultAsync(aa => aa.ProcessInstanceId == processInstanceId);
            var employee = await _employeeRepository.GetAsync(item.EmployeeId);
            var employeeId = (await _employeeRepository.FirstOrDefaultAsync(aa => aa.IsLeaderInDepts == "key:" + employee.Department + "value:True")).Id;
                //.Where(aa =>aa.IsLeaderInDepts == "key:" + employee.Department + "value:True")
                //.Select(aa => aa.Id).Distinct().AsNoTracking().ToListAsync();
            item.ApprovalTime = DateTime.Now;
            item.ApproverId = employeeId;
            if (result == "agree")
                item.Status = TimeSheetStatusEnum.审批通过;
            else if (result == "refuse")
                item.Status = TimeSheetStatusEnum.拒绝;
            else
                item.Status = TimeSheetStatusEnum.取消;
            await _repository.UpdateAsync(item);
        }
    }
}
