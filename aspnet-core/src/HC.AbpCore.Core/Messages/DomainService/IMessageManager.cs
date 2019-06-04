

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
        /// 修改消息状态为已读
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task ModifyDoReadById(Guid id);




    }
}
