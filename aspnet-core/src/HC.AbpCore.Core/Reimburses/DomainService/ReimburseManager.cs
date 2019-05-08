

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
using HC.AbpCore.Reimburses;


namespace HC.AbpCore.Reimburses.DomainService
{
    /// <summary>
    /// Reimburse领域层的业务管理
    ///</summary>
    public class ReimburseManager :AbpCoreDomainServiceBase, IReimburseManager
    {
		
		private readonly IRepository<Reimburse,Guid> _repository;

		/// <summary>
		/// Reimburse的构造方法
		///</summary>
		public ReimburseManager(
			IRepository<Reimburse, Guid> repository
		)
		{
			_repository =  repository;
		}


		/// <summary>
		/// 初始化
		///</summary>
		public void InitReimburse()
		{
			throw new NotImplementedException();
		}

		// TODO:编写领域业务代码



		 
		  
		 

	}
}
