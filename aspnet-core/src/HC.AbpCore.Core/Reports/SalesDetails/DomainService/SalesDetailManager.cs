

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
using HC.AbpCore.Reports.SalesDetails;


namespace HC.AbpCore.Reports.SalesDetails.DomainService
{
    /// <summary>
    /// SalesDetail领域层的业务管理
    ///</summary>
    public class SalesDetailManager :AbpCoreDomainServiceBase, ISalesDetailManager
    {
		
		private readonly IRepository<SalesDetail,Guid> _repository;

		/// <summary>
		/// SalesDetail的构造方法
		///</summary>
		public SalesDetailManager(
			IRepository<SalesDetail, Guid> repository
		)
		{
			_repository =  repository;
		}


		/// <summary>
		/// 初始化
		///</summary>
		public void InitSalesDetail()
		{
			throw new NotImplementedException();
		}

		// TODO:编写领域业务代码



		 
		  
		 

	}
}
