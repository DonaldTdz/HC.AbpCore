

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
        /// 修改消息状态为已读
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task ModifyDoReadById(Guid id)
        {
            Message message = await _repository.GetAsync(id);
            message.IsRead = true;
            message.ReadTime = DateTime.Now;
            await _repository.UpdateAsync(message);
        }





    }
}
