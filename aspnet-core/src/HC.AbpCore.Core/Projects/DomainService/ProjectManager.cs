

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
using HC.AbpCore.Projects;
using HC.AbpCore.DingTalk;
using static HC.AbpCore.Projects.ProjectBase;
using HC.AbpCore.Messages;
using HC.AbpCore.Common;
using Senparc.CO2NET.Helpers;
using System.Text;
using Senparc.CO2NET.HttpUtility;

namespace HC.AbpCore.Projects.DomainService
{
    /// <summary>
    /// Project领域层的业务管理
    ///</summary>
    public class ProjectManager :AbpCoreDomainServiceBase, IProjectManager
    {
		
		private readonly IRepository<Project,Guid> _repository;
        private readonly IRepository<Message, Guid> _message;

        /// <summary>
        /// Project的构造方法
        ///</summary>
        public ProjectManager(
			IRepository<Project, Guid> repository,
                IRepository<Message, Guid> message
        )
		{
            _message = message;
            _repository =  repository;
		}


		/// <summary>
		/// 初始化
		///</summary>
		public void InitProject()
		{
			throw new NotImplementedException();
		}

        public async Task ProjectStatusRemind(string accessToken, DingDingAppConfig dingDingAppConfig)
        {
            var url = string.Format("https://oapi.dingtalk.com/topapi/message/corpconversation/asyncsend_v2?access_token={0}", accessToken);
            var projects =await _repository.GetAll().Where(aa => aa.Status != ProjectStatus.丢单 && aa.Status != ProjectStatus.已完成).AsNoTracking().ToListAsync();
            foreach (var project in projects)
            {
                Message message = new Message();
                message.Content = string.Format("您好! 项目:{0}，当前进度为:{1},描述{2}", project.Name + "(" + project.ProjectCode + ")",project.Status.ToString(),project.Desc);
                message.SendTime = DateTime.Now;
                message.Type = MessageTypeEnum.项目进度提醒;
                message.IsRead = false;
                message.EmployeeId = project.EmployeeId;
                message= await _message.InsertAsync(message);
                DingMsgs dingMsgs = new DingMsgs();
                dingMsgs.userid_list = project.EmployeeId;
                dingMsgs.to_all_user = false;
                dingMsgs.agent_id = dingDingAppConfig.AgentID;
                dingMsgs.msg.msgtype = "link";
                dingMsgs.msg.link.title = "项目进度提醒";
                dingMsgs.msg.link.text = string.Format("您好! 项目:{0}，当前进度为:{1},点击查看详情", project.Name + "(" + project.ProjectCode + ")", project.Status.ToString());
                dingMsgs.msg.link.picUrl = "eapp://";
                dingMsgs.msg.link.messageUrl = "eapp://page/messages/detail-messages/detail-messages?id="+message.Id;
                var jsonString = SerializerHelper.GetJsonString(dingMsgs, null);
                MessageResponseResult response = new MessageResponseResult();
                using (MemoryStream ms = new MemoryStream())
                {
                    var bytes = Encoding.UTF8.GetBytes(jsonString);
                    ms.Write(bytes, 0, bytes.Length);
                    ms.Seek(0, SeekOrigin.Begin);
                    response = Post.PostGetJson<MessageResponseResult>(url, null, ms);
                };
                //发送消息失败则删除消息中心对应数据
                if (response.errcode != 0)
                {
                    await _message.DeleteAsync(message.Id);
                }
            }
        }

        // TODO:编写领域业务代码







    }
}
