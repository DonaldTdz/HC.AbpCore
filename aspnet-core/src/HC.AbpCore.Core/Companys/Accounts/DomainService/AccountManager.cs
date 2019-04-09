

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
using HC.AbpCore.Companys.Accounts;


namespace HC.AbpCore.Companys.Accounts.DomainService
{
    /// <summary>
    /// Account领域层的业务管理
    ///</summary>
    public class AccountManager :AbpCoreDomainServiceBase, IAccountManager
    {
		
		private readonly IRepository<Account,long> _repository;

		/// <summary>
		/// Account的构造方法
		///</summary>
		public AccountManager(
			IRepository<Account, long> repository
		)
		{
			_repository =  repository;
		}


		/// <summary>
		/// 初始化
		///</summary>
		public void InitAccount()
		{
			throw new NotImplementedException();
		}

		// TODO:编写领域业务代码



		 
		  
		 

	}
}
