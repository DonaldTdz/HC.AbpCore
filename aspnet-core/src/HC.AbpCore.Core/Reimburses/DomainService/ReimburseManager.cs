

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
using HC.AbpCore.Reimburses;
using Abp.Domain.Entities;
using HC.AbpCore.Reimburses.ReimburseDetails;
using HC.AbpCore.Projects;
using HC.AbpCore.DingTalk.Employees;
using HC.AbpCore.Common;
using Newtonsoft.Json;
using System.Collections;
using HC.AbpCore.DingTalk;
using Senparc.CO2NET.HttpUtility;
using DingTalk.Api;
using DingTalk.Api.Response;
using DingTalk.Api.Request;
using HC.AbpCore.Messages;
using Senparc.CO2NET.Helpers;
using System.Text;
using HC.AbpCore.Messages.DomainService;

namespace HC.AbpCore.Reimburses.DomainService
{
    /// <summary>
    /// Reimburse领域层的业务管理
    ///</summary>
    public class ReimburseManager : AbpCoreDomainServiceBase, IReimburseManager
    {

        private readonly IRepository<Reimburse, Guid> _repository;
        private readonly IRepository<ReimburseDetail, Guid> _detailRepository;
        private readonly IRepository<Project, Guid> _projectRepository;
        private readonly IRepository<Employee, string> _employeeRepository;
        private readonly IDingTalkManager _dingTalkManager;
        private readonly IMessageManager _messageManager;

        /// <summary>
        /// Reimburse的构造方法
        ///</summary>
        public ReimburseManager(
            IRepository<Reimburse, Guid> repository,
            IRepository<Project, Guid> projectRepository,
            IRepository<Employee, string> employeeRepository,
            IRepository<ReimburseDetail, Guid> detailRepository,
            IDingTalkManager dingTalkManager,
            IMessageManager messageManager
        )
        {
            _dingTalkManager = dingTalkManager;
            _employeeRepository = employeeRepository;
            _projectRepository = projectRepository;
            _detailRepository = detailRepository;
            _repository = repository;
            _messageManager = messageManager;
        }


        /// <summary>
        /// 初始化
        ///</summary>
        public void InitReimburse()
        {
            throw new NotImplementedException();
        }




        // TODO:编写领域业务代码


        /// <summary>
        /// 报销审批提醒
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="dingDingAppConfig"></param>
        /// <returns></returns>
        public async Task ReimburseApprovalRemind(string accessToken, DingDingAppConfig dingDingAppConfig)
        {
            var projects = _projectRepository.GetAll();
            var employees = _employeeRepository.GetAll();
            var query = _repository.GetAll()
                .Where(aa => aa.Status == ReimburseStatusEnum.待审核);
            var reimburses = from reimburse in query
                             join project in projects on reimburse.ProjectId equals project.Id
                             join employee in employees on reimburse.EmployeeId equals employee.Id
                             select new
                             {
                                 ProjectName = project.Name + "(" + project.ProjectCode + ")",
                                 reimburse.Amount,
                                 reimburse.SubmitDate,
                                 employee.Name,
                                 employee.Department,
                             };
            var items = await reimburses.AsNoTracking().ToListAsync();
            var url = string.Format("https://oapi.dingtalk.com/topapi/message/corpconversation/asyncsend_v2?access_token={0}", accessToken);
            foreach (var item in items)
            {
                // TODO:提醒人后期需要完善
                var employeeIdList = await _employeeRepository.GetAll().Where(aa => aa.IsLeaderInDepts == "key:73354253value:True" || aa.IsLeaderInDepts== "key:"+item.Department+ "value:True").Select(aa => aa.Id)
                                     .Distinct().AsNoTracking().ToListAsync();
                string employeeIds = string.Join(",", employeeIdList.ToArray());
                Message message = new Message();
                message.Content = string.Format("您好! 项目:{0}有一笔报销费用，报销人:{1}，报销金额:{2},申请时间:{3}", item.ProjectName, item.Name,item.Amount,item.SubmitDate.Value.ToString("yyyy-MM-dd"));
                message.SendTime = DateTime.Now;
                message.Type = MessageTypeEnum.审批提醒;
                message.IsRead = false;
                DingMsgs dingMsgs = new DingMsgs();
                dingMsgs.userid_list = employeeIds;
                dingMsgs.to_all_user = false;
                dingMsgs.agent_id = dingDingAppConfig.AgentID;
                dingMsgs.msg.msgtype = "link";
                dingMsgs.msg.link.title = "审批提醒";
                dingMsgs.msg.link.text = string.Format("您好! 项目:{0}有一笔报销费用，报销人:{1},点击查看详情", item.ProjectName, item.Name);
                dingMsgs.msg.link.picUrl = "eapp://";
                dingMsgs.msg.link.messageUrl = "eapp://";
                var jsonString = SerializerHelper.GetJsonString(dingMsgs, null);
                MessageResponseResult response = new MessageResponseResult();
                using (MemoryStream ms = new MemoryStream())
                {
                    var bytes = Encoding.UTF8.GetBytes(jsonString);
                    ms.Write(bytes, 0, bytes.Length);
                    ms.Seek(0, SeekOrigin.Begin);
                    response = Post.PostGetJson<MessageResponseResult>(url, null, ms);
                };
                //新增到消息中心
                if (response.errcode == 0 && response.task_id != 0)
                {
                    await _messageManager.CreateByTaskId(response.task_id, message, dingDingAppConfig.AgentID, accessToken, employeeIdList);
                }
            }
        }


        /// <summary>
        /// 提交审批(return 1)  
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ResultCode> SubmitApproval(Guid Id)
        {
            ResultCode resultCode = new ResultCode();
            var config = await _dingTalkManager.GetDingDingConfigByAppAsync(DingDingAppEnum.智能办公);
            string accessToken = await _dingTalkManager.GetAccessTokenByAppAsync(DingDingAppEnum.智能办公); //GetAccessToken();
            var reimburse = await _repository.GetAsync(Id);
            var project = await _projectRepository.GetAsync(reimburse.ProjectId.Value);
            if (project == null)
            {
                resultCode.Code = 1;
                resultCode.Msg = "所属项目不存在";
                return resultCode;
            }
            var employee = await _employeeRepository.GetAsync(reimburse.EmployeeId);
            if (employee == null)
            {
                resultCode.Code = 2;
                resultCode.Msg = "所属报销人不存在";
                return resultCode;
            }
            var reimburseDetails = await _detailRepository.GetAll().Where(aa => aa.ReimburseId == Id).AsNoTracking().ToListAsync();
            var deptId = employee.Department.Replace("[", "").Replace("]", "");

            DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/processinstance/create");
            OapiProcessinstanceCreateRequest request = new OapiProcessinstanceCreateRequest();
            request.ProcessCode = "PROC-0AA374CC-7381-46EA-BB8F-3BF4B2BFDEA2";
            request.OriginatorUserId = reimburse.EmployeeId;
            request.AgentId = config.AgentID;
            request.DeptId = Convert.ToInt32(deptId);
            List<OapiProcessinstanceCreateRequest.FormComponentValueVoDomain> formComponentValues = new List<OapiProcessinstanceCreateRequest.FormComponentValueVoDomain>();

            OapiProcessinstanceCreateRequest.FormComponentValueVoDomain vo = new OapiProcessinstanceCreateRequest.FormComponentValueVoDomain();
            formComponentValues.Add(new OapiProcessinstanceCreateRequest.FormComponentValueVoDomain() { Name = "所属项目", Value = project.Name + "(" + project.ProjectCode + ")" });
            formComponentValues.Add(new OapiProcessinstanceCreateRequest.FormComponentValueVoDomain() { Name = "报销总金额", Value = reimburse.Amount.Value.ToString() });
            formComponentValues.Add(new OapiProcessinstanceCreateRequest.FormComponentValueVoDomain() { Name = "报销人", Value = employee.Name });
            formComponentValues.Add(new OapiProcessinstanceCreateRequest.FormComponentValueVoDomain() { Name = "申请日期", Value = reimburse.SubmitDate.Value.ToString() });
            ArrayList items = new ArrayList();
            foreach (var item in reimburseDetails)
            {
                ArrayList approvalReimburseDetail = new ArrayList();
                approvalReimburseDetail.Add(new OapiProcessinstanceCreateRequest.FormComponentValueVoDomain() { Name = "客户", Value = item.Customer });
                approvalReimburseDetail.Add(new OapiProcessinstanceCreateRequest.FormComponentValueVoDomain() { Name = "发生日期", Value = item.HappenDate.ToString() });
                approvalReimburseDetail.Add(new OapiProcessinstanceCreateRequest.FormComponentValueVoDomain() { Name = "报销类型", Value = item.Type });
                approvalReimburseDetail.Add(new OapiProcessinstanceCreateRequest.FormComponentValueVoDomain() { Name = "金额", Value = item.Amount.ToString() });
                approvalReimburseDetail.Add(new OapiProcessinstanceCreateRequest.FormComponentValueVoDomain() { Name = "发生地点", Value = item.Place });
                approvalReimburseDetail.Add(new OapiProcessinstanceCreateRequest.FormComponentValueVoDomain() { Name = "费用说明", Value = item.Desc });
                items.Add(approvalReimburseDetail);
            }
            formComponentValues.Add(new OapiProcessinstanceCreateRequest.FormComponentValueVoDomain() { Name = "明细", Value = JsonConvert.SerializeObject(items) });
            request.FormComponentValues_ = formComponentValues;
            OapiProcessinstanceCreateResponse response = client.Execute(request, accessToken);
            //var depts=Post.PostGetJson<ApprovalReturn>(string.Format("https://oapi.dingtalk.com/topapi/processinstance/create?access_token={0}", accessToken), null)
            if (response.ErrCode == "0")
            {
                reimburse.ProcessInstanceId = response.ProcessInstanceId;
                reimburse.Status = ReimburseStatusEnum.待审核;
                await _repository.UpdateAsync(reimburse);
                return new ResultCode() { Code = 0, Msg = "提交成功" };

            }
            else
            {
                return new ResultCode() { Code = 4, Msg = "提交失败" };
            }
        }




    }
}
