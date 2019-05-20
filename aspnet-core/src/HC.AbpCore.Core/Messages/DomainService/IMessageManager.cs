

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.Messages;


namespace HC.AbpCore.Messages.DomainService
{
    public interface IMessageManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitMessage();


        /// <summary>
        /// 根据返回的任务ID查询结果并把相关内容新增到消息中心
        /// </summary>
        /// <param name="taskId">钉钉发送消息返回的任务ID</param>
        /// <param name="message">消息修改内容</param
        /// <param name="agentId">E应用的agentId</param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        Task CreateByTaskId(long taskId, Message message, int agentId, string accessToken,List<string> employeeIds);




    }
}
