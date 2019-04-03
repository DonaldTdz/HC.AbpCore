

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
using HC.AbpCore.DingTalk.Organizations;


namespace HC.AbpCore.DingTalk.Organizations.DomainService
{
    /// <summary>
    /// Organization领域层的业务管理
    ///</summary>
    public class OrganizationManager :AbpCoreDomainServiceBase, IOrganizationManager
    {
		
		private readonly IRepository<Organization,long> _repository;

		/// <summary>
		/// Organization的构造方法
		///</summary>
		public OrganizationManager(
			IRepository<Organization, long> repository
		)
		{
			_repository =  repository;
		}


		/// <summary>
		/// 初始化
		///</summary>
		public void InitOrganization()
		{
			throw new NotImplementedException();
		}

		// TODO:编写领域业务代码



		 
		  
		 

	}
}
