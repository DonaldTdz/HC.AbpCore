

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.Domain.Repositories;

using HC.AbpCore.DingTalk;
using HC.AbpCore.Common;
using HC.AbpCore.DingTalk.Employees;
using Senparc.CO2NET.Helpers;
using System.Text;
using Senparc.CO2NET.HttpUtility;
using HC.AbpCore.Projects;
using HC.AbpCore.Messages;
using DingTalk.Api.Response;
using HC.AbpCore.Messages.DomainService;
using System.Collections.Generic;

namespace HC.AbpCore.PaymentPlans.DomainService
{
    /// <summary>
    /// PaymentPlan领域层的业务管理
    ///</summary>
    public class PaymentPlanManager : AbpCoreDomainServiceBase, IPaymentPlanManager
    {

        private readonly IRepository<PaymentPlan, Guid> _repository;
        private readonly IRepository<Employee, string> _employeeRepository;
        private readonly IRepository<Project, Guid> _projectRepository;
        private readonly IMessageManager _messageManager;

        /// <summary>
        /// PaymentPlan的构造方法
        ///</summary>
        public PaymentPlanManager(
            IRepository<PaymentPlan, Guid> repository,
            IRepository<Employee, string> employeeRepository,
            IMessageManager messageManager,
            IRepository<Project, Guid> projectRepository
        )
        {
            _projectRepository = projectRepository;
            _employeeRepository = employeeRepository;
            _messageManager = messageManager;
            _repository = repository;
        }


        /// <summary>
        /// 初始化
        ///</summary>
        public void InitPaymentPlan()
        {
            throw new NotImplementedException();
        }

        //TODO:编写领域业务代码



        /// <summary>
        /// 回款提醒   钉钉提前45天提醒
        /// </summary>
        /// <returns>employeeIds</returns>
        public async Task PaymentRemindAsync(string accessToken, DingDingAppConfig dingDingAppConfig)
        {
            var projects = _projectRepository.GetAll();
            var query = _repository.GetAll()
                .Where(aa => aa.Status == PaymentPlanStatusEnum.未回款)
                .Where(aa => aa.PaymentTime <= DateTime.Now.AddDays(45) && aa.PaymentTime >= DateTime.Now);
            var paymentPlans = from paymentPlan in query
                               join project in projects on paymentPlan.ProjectId equals project.Id
                               select new
                               {
                                   ProjectName = project.Name + "(" + project.ProjectCode + ")",
                                   project.EmployeeId,
                                   paymentPlan.PlanTime
                               };
            var items = await paymentPlans.AsNoTracking().ToListAsync();
            var employeeIdList = await _employeeRepository.GetAll().Where(aa => aa.IsLeaderInDepts == "key:73354253value:True").Select(aa => aa.Id)
                .Distinct().AsNoTracking().ToListAsync();
            string employeeIds = string.Join(",", employeeIdList.ToArray());
            var url = string.Format("https://oapi.dingtalk.com/topapi/message/corpconversation/asyncsend_v2?access_token={0}", accessToken);
            foreach (var item in items)
            {
                Message message = new Message();
                message.Content = string.Format("您好! 项目:{0}计划回款时间即将达到，计划回款时间为:{1}", item.ProjectName, item.PlanTime.ToString("yyyy-MM-dd"));
                message.SendTime = DateTime.Now;
                message.Type = MessageTypeEnum.催款提醒;
                message.IsRead = false;
                DingMsgs dingMsgs = new DingMsgs();
                dingMsgs.userid_list = employeeIds + "," + item.EmployeeId;
                dingMsgs.to_all_user = false;
                dingMsgs.agent_id = dingDingAppConfig.AgentID;
                dingMsgs.msg.msgtype = "link";
                dingMsgs.msg.link.title = "催款提醒";
                dingMsgs.msg.link.text = string.Format("所属项目:{0}，点击查看详情", item.ProjectName, item.PlanTime.ToString("yyyy-MM-dd"));
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
                    employeeIdList.Add(item.EmployeeId);
                    await _messageManager.CreateByTaskId(response.task_id, message, dingDingAppConfig.AgentID, accessToken, employeeIdList);
                }
            }

        }


    }
}
