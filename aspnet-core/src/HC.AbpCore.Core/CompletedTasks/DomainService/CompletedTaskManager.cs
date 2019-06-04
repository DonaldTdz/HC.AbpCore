

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
using HC.AbpCore.Tasks;
using HC.AbpCore.DingTalk;
using HC.AbpCore.Projects;
using HC.AbpCore.DingTalk.Employees;
using HC.AbpCore.Messages;
using HC.AbpCore.Common;
using Senparc.CO2NET.Helpers;
using System.Text;
using Senparc.CO2NET.HttpUtility;
using HC.AbpCore.Messages.DomainService;

namespace HC.AbpCore.Tasks.DomainService
{
    /// <summary>
    /// CompletedTask领域层的业务管理
    ///</summary>
    public class CompletedTaskManager : AbpCoreDomainServiceBase, ICompletedTaskManager
    {

        private readonly IRepository<CompletedTask, Guid> _repository;
        private readonly IRepository<Project, Guid> _projectRepository;
        private readonly IRepository<Employee, string> _employeeRepository;
        private readonly IRepository<Message, Guid> _messageRepository;
        /// <summary>
        /// CompletedTask的构造方法
        ///</summary>
        public CompletedTaskManager(
            IRepository<CompletedTask, Guid> repository,
            IRepository<Project, Guid> projectRepository,
            IRepository<Employee, string> employeeRepository,
            IRepository<Message, Guid> messageRepository,
            IMessageManager messageManager
        )
        {
            _messageRepository = messageRepository;
            _repository = repository;
            _employeeRepository = employeeRepository;
            _projectRepository = projectRepository;
        }


        /// <summary>
        /// 初始化
        ///</summary>
        public void InitCompletedTask()
        {
            throw new NotImplementedException();
        }

        // TODO:编写领域业务代码

        /// <summary>
        /// 待完成任务提醒
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="dingDingAppConfig"></param>
        /// <returns></returns>
        public async Task TaskRemindAsync(string accessToken, DingDingAppConfig dingDingAppConfig)
        {
            var query = _repository.GetAll().Where(aa => aa.IsCompleted == false);
            var projects = _projectRepository.GetAll();
            var employees = _employeeRepository.GetAll();
            var tasks = from task in query
                        join project in projects on task.ProjectId equals project.Id
                        join employee in employees on task.EmployeeId equals employee.Id
                        select new
                        {
                            task.Id,
                            ProjectName = project.Name + "(" + project.ProjectCode + ")",
                            task.Status,
                            task.Content,
                            employee.Name,
                            task.EmployeeId,
                            task.CreationTime,
                            task.ClosingDate
                        };
            var items = await tasks.Distinct().AsNoTracking().ToListAsync();
            var url = string.Format("https://oapi.dingtalk.com/topapi/message/corpconversation/asyncsend_v2?access_token={0}", accessToken);
            foreach (var item in items)
            {
                Message message = new Message();
                message.Content = string.Format("您好! 项目:{0}，{1}:日期即将到达，截止日期:{2}", item.ProjectName, item.Status.ToString(), item.ClosingDate.ToString("yyyy-MM-dd"));
                message.SendTime = DateTime.Now;
                message.Type = MessageTypeEnum.待办提醒;
                message.IsRead = false;
                message.EmployeeId = item.EmployeeId;
                //新增到消息中心
                var messageId = await _messageRepository.InsertAndGetIdAsync(message);

                DingMsgs dingMsgs = new DingMsgs();
                dingMsgs.userid_list = item.EmployeeId;
                dingMsgs.to_all_user = false;
                dingMsgs.agent_id = dingDingAppConfig.AgentID;
                dingMsgs.msg.msgtype = "link";
                dingMsgs.msg.link.title = "待办提醒";
                dingMsgs.msg.link.text = string.Format("您好! 项目:{0}，{1}:日期即将到达，截止日期:{2}，点击查看详情", item.ProjectName, item.Status.ToString(), item.ClosingDate.ToString("yyyy-MM-dd"));
                dingMsgs.msg.link.picUrl = "@lALPBY0V4-AiG7vMgMyA";
                dingMsgs.msg.link.messageUrl = "eapp://page/completedtask/modify-completedtask/modify-completedtask?id="+item.Id+ "&messageId="+messageId;
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
                    await _messageRepository.DeleteAsync(messageId);
            }
        }


    }
}
