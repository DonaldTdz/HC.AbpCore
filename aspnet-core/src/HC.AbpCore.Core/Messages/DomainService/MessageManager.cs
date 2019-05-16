

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


namespace HC.AbpCore.Messages.DomainService
{
    /// <summary>
    /// Message领域层的业务管理
    ///</summary>
    public class MessageManager :AbpCoreDomainServiceBase, IMessageManager
    {
		
		private readonly IRepository<Message,Guid> _repository;

		/// <summary>
		/// Message的构造方法
		///</summary>
		public MessageManager(
			IRepository<Message, Guid> repository
		)
		{
			_repository =  repository;
		}


		/// <summary>
		/// 初始化
		///</summary>
		public void InitMessage()
		{
			throw new NotImplementedException();
		}

		// TODO:编写领域业务代码



		 
		  
		 

	}
}
