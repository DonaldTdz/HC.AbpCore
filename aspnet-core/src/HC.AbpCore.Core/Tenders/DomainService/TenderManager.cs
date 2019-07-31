

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
using HC.AbpCore.Tenders;
using HC.AbpCore.Tasks;
using HC.AbpCore.CompletedTasks;
using HC.AbpCore.DingTalk;
using HC.AbpCore.Projects;
using HC.AbpCore.Messages;
using HC.AbpCore.Common;
using Senparc.CO2NET.Helpers;
using System.Text;
using Senparc.CO2NET.HttpUtility;

namespace HC.AbpCore.Tenders.DomainService
{
    /// <summary>
    /// Tender领域层的业务管理
    ///</summary>
    public class TenderManager : AbpCoreDomainServiceBase, ITenderManager
    {

        private readonly IRepository<Tender, Guid> _repository;
        private readonly IRepository<CompletedTask, Guid> _taskRepository;
        private readonly IRepository<Message, Guid> _messageRepository;
        private readonly IRepository<Project, Guid> _projectRepository;

        /// <summary>
        /// Tender的构造方法
        ///</summary>
        public TenderManager(
            IRepository<Tender, Guid> repository
            , IRepository<CompletedTask, Guid> taskRepository
            , IRepository<Project, Guid> projectRepository
            , IRepository<Message, Guid> messageRepository
        )
        {
            _taskRepository = taskRepository;
            _messageRepository = messageRepository;
            _repository = repository;
            _projectRepository = projectRepository;
        }


        /// <summary>
        /// 初始化
        ///</summary>
        public void InitTender()
        {
            throw new NotImplementedException();
        }

        // TODO:编写领域业务代码


        public async Task<Tender> UpdateAndUpdateTaskAsync(Tender input)
        {
            var entity= await _repository.UpdateAsync(input);
            var task =await _taskRepository.FirstOrDefaultAsync(aa => aa.RefId == input.Id && aa.Status == TaskStatusEnum.招标保证金缴纳);
            if (entity.IsPayBond == true)
            {
                if (task != null)
                {
                    task.IsCompleted = true;
                    await _taskRepository.UpdateAsync(task);
                }
            }
            else
            {
                if (task != null)
                {
                    task.IsCompleted = false;
                    await _taskRepository.UpdateAsync(task);
                }
                else
                {
                    await _taskRepository.InsertAsync(new CompletedTask()
                    {
                        Status = TaskStatusEnum.招标保证金缴纳,
                        IsCompleted = entity.IsPayBond,
                        RefId = entity.Id,
                        EmployeeId = entity.PreparationPerson,
                        ClosingDate = entity.BondTime.Value,
                        Content = "保证金缴纳截止日期即将到达"
                    });
                }
            }
            return entity;
        }


        public async Task<Tender> CreateAndCreateTaskAsync(Tender input)
        {
            var entity = await _repository.InsertAsync(input);
            if (entity.IsPayBond == false)
            {
                await _taskRepository.InsertAsync(new CompletedTask() {
                    Status = TaskStatusEnum.招标保证金缴纳,
                    IsCompleted = entity.IsPayBond,
                    RefId = entity.Id,
                    EmployeeId = entity.PreparationPerson,
                    ClosingDate = entity.BondTime.Value,
                Content="保证金缴纳截止日期即将到达"});
            }
            return entity;

        }


        /// <summary>
        /// 招标提醒
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="dingDingAppConfig"></param>
        /// <returns></returns>
        public async Task TenderRemindAsync(string accessToken, DingDingAppConfig dingDingAppConfig)
        {
            var query = _repository.GetAll().Where(aa => aa.TenderTime <= DateTime.Now.AddDays(4) && aa.TenderTime >= DateTime.Now);
            var projects = _projectRepository.GetAll();
            var tenders = from tender in query
                        join project in projects on tender.ProjectId equals project.Id
                        select new
                        {
                            tender.PreparationPerson,
                            tender.ProjectId,
                            ProjectName = project.Name + "(" + project.ProjectCode + ")",
                            tender.TenderTime
                        };
            var items = await tenders.Distinct().AsNoTracking().ToListAsync();
            var url = string.Format("https://oapi.dingtalk.com/topapi/message/corpconversation/asyncsend_v2?access_token={0}", accessToken);
            foreach (var item in items)
            {
                Message message = new Message();
                message.Content = string.Format("您好! 项目:{0}，招标日期即将到达,请检查招标资料是否全部完成准备，招标日期:{1}", item.ProjectName, item.TenderTime.Value.ToString("yyyy-MM-dd"));
                message.SendTime = DateTime.Now;
                message.Type = MessageTypeEnum.待办提醒;
                message.IsRead = false;
                message.EmployeeId = item.PreparationPerson;
                //新增到消息中心
                var messageId = await _messageRepository.InsertAndGetIdAsync(message);

                DingMsgs dingMsgs = new DingMsgs();
                dingMsgs.userid_list = item.PreparationPerson;
                dingMsgs.to_all_user = false;
                dingMsgs.agent_id = dingDingAppConfig.AgentID;
                dingMsgs.msg.msgtype = "link";
                dingMsgs.msg.link.title = "待办提醒";
                dingMsgs.msg.link.text = string.Format("您好! 项目:{0}，招标日期即将到达，招标日期:{1}，点击查看详情", item.ProjectName, item.TenderTime.Value.ToString("yyyy-MM-dd"));
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
                    await _messageRepository.DeleteAsync(messageId);
            }
        }


    }
}
