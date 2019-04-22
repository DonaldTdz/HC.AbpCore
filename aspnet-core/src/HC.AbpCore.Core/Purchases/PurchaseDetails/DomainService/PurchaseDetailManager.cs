

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
using HC.AbpCore.Purchases.PurchaseDetails;


namespace HC.AbpCore.Purchases.PurchaseDetails.DomainService
{
    /// <summary>
    /// PurchaseDetail领域层的业务管理
    ///</summary>
    public class PurchaseDetailManager :AbpCoreDomainServiceBase, IPurchaseDetailManager
    {
		
		private readonly IRepository<PurchaseDetail,Guid> _repository;

		/// <summary>
		/// PurchaseDetail的构造方法
		///</summary>
		public PurchaseDetailManager(
			IRepository<PurchaseDetail, Guid> repository
		)
		{
			_repository =  repository;
		}


		/// <summary>
		/// 初始化
		///</summary>
		public void InitPurchaseDetail()
		{
			throw new NotImplementedException();
		}

		// TODO:编写领域业务代码



		 
		  
		 

	}
}
