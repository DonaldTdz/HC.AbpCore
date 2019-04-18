

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
using HC.AbpCore.Tenders;


namespace HC.AbpCore.Tenders.DomainService
{
    /// <summary>
    /// Tender领域层的业务管理
    ///</summary>
    public class TenderManager :AbpCoreDomainServiceBase, ITenderManager
    {
		
		private readonly IRepository<Tender,Guid> _repository;

		/// <summary>
		/// Tender的构造方法
		///</summary>
		public TenderManager(
			IRepository<Tender, Guid> repository
		)
		{
			_repository =  repository;
		}


		/// <summary>
		/// 初始化
		///</summary>
		public void InitTender()
		{
			throw new NotImplementedException();
		}

		// TODO:编写领域业务代码



		 
		  
		 

	}
}
