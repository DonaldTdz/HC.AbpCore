

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
using HC.AbpCore.Customers.CustomerContacts;


namespace HC.AbpCore.Customers.CustomerContacts.DomainService
{
    /// <summary>
    /// CustomerContact领域层的业务管理
    ///</summary>
    public class CustomerContactManager :AbpCoreDomainServiceBase, ICustomerContactManager
    {
		
		private readonly IRepository<CustomerContact,int> _repository;

		/// <summary>
		/// CustomerContact的构造方法
		///</summary>
		public CustomerContactManager(
			IRepository<CustomerContact, int> repository
		)
		{
			_repository =  repository;
		}


		/// <summary>
		/// 初始化
		///</summary>
		public void InitCustomerContact()
		{
			throw new NotImplementedException();
		}

		// TODO:编写领域业务代码



		 
		  
		 

	}
}
