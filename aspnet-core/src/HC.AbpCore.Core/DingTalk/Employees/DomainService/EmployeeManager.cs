

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
using HC.AbpCore.DingTalk.Employees;
using HC.AbpCore.Messages;
using HC.AbpCore.Messages.DomainService;
using HC.AbpCore.Common;
using Senparc.CO2NET.Helpers;
using System.Text;
using Senparc.CO2NET.HttpUtility;

namespace HC.AbpCore.DingTalk.Employees.DomainService
{
    /// <summary>
    /// Employee领域层的业务管理
    ///</summary>
    public class EmployeeManager : AbpCoreDomainServiceBase, IEmployeeManager
    {

        private readonly IRepository<Employee, string> _repository;
        private readonly IMessageManager _messageManager;

        /// <summary>
        /// Employee的构造方法
        ///</summary>
        public EmployeeManager(
            IRepository<Employee, string> repository,
            IMessageManager messageManager
        )
        {
            _messageManager = messageManager;
            _repository = repository;
        }


        /// <summary>
        /// 初始化
        ///</summary>
        public void InitEmployee()
        {
            throw new NotImplementedException();
        }

        // TODO:编写领域业务代码


        public async Task EmployeeWeeklyRemind(string accessToken, DingDingAppConfig dingDingAppConfig)
        {
            var url = string.Format("https://oapi.dingtalk.com/topapi/message/corpconversation/asyncsend_v2?access_token={0}", accessToken);
            List<string> employeeIdList = await _repository.GetAll().Select(aa=>aa.Id).Distinct().AsNoTracking().ToListAsync();

            string employeeIds = string.Join(",", employeeIdList.ToArray());
            Message message = new Message();
            message.Content = "您好! 请按时填写上周周报并且提交审批";
            message.SendTime = DateTime.Now;
            message.Type = MessageTypeEnum.周报填写提醒;
            message.IsRead = false;
            DingMsgs dingMsgs = new DingMsgs();
            dingMsgs.userid_list = employeeIds;
            dingMsgs.to_all_user = false;
            dingMsgs.agent_id = dingDingAppConfig.AgentID;
            dingMsgs.msg.msgtype = "link";
            dingMsgs.msg.link.title = "周报填写提醒";
            dingMsgs.msg.link.text = "您好!请按时填写上周周报并且提交审批,点击查看消息详情";
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
}
