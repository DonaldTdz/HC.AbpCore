

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
using HC.AbpCore.Messages;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using Senparc.CO2NET.HttpUtility;
using System.Text;
using Senparc.CO2NET.Helpers;
using HC.AbpCore.Common;

namespace HC.AbpCore.Messages.DomainService
{
    /// <summary>
    /// Message领域层的业务管理
    ///</summary>
    public class MessageManager : AbpCoreDomainServiceBase, IMessageManager
    {

        private readonly IRepository<Message, Guid> _repository;

        /// <summary>
        /// Message的构造方法
        ///</summary>
        public MessageManager(
            IRepository<Message, Guid> repository
        )
        {
            _repository = repository;
        }


        /// <summary>
        /// 初始化
        ///</summary>
        public void InitMessage()
        {
            throw new NotImplementedException();
        }

        // TODO:编写领域业务代码


        /// <summary>
        /// 根据返回的任务ID查询结果并把相关内容新增到消息中心
        /// </summary>
        /// <param name="taskId">钉钉发送消息返回的任务ID</param>
        /// <param name="message">消息修改内容</param>
        /// <param name="agentId">E应用的agentId</param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task CreateByTaskId(long taskId, Message message,int agentId, string accessToken,List<string> employeeIds)
        {
            var url = string.Format("https://oapi.dingtalk.com/topapi/message/corpconversation/getsendresult?access_token={0}", accessToken);
            MessageSendResultRequest request = new MessageSendResultRequest();
            request.agent_id = agentId;
            request.task_id = taskId;
            var jsonString = SerializerHelper.GetJsonString(request, null);
            MessageSendResultResponse response = new MessageSendResultResponse();
            using (MemoryStream ms = new MemoryStream())
            {
                var bytes = Encoding.UTF8.GetBytes(jsonString);
                ms.Write(bytes, 0, bytes.Length);
                ms.Seek(0, SeekOrigin.Begin);
                response = Post.PostGetJson<MessageSendResultResponse>(url, null, ms);
            };

            employeeIds.RemoveAll(it => response.send_result.invalid_user_id_list.Contains(it));
            employeeIds.RemoveAll(it => response.send_result.failed_user_id_list.Contains(it));
            //employeeIds.RemoveAll(it => response.send_result.forbidden_user_id_list.Contains(it));
            foreach (var employeeId in employeeIds)
            {
                message.EmployeeId = employeeId;
                await _repository.InsertAsync(message);
            }

        }





    }
}
