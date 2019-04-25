

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
using HC.AbpCore.Invoices.InvoiceDetails;


namespace HC.AbpCore.Invoices.InvoiceDetails.DomainService
{
    /// <summary>
    /// InvoiceDetail领域层的业务管理
    ///</summary>
    public class InvoiceDetailManager :AbpCoreDomainServiceBase, IInvoiceDetailManager
    {
		
		private readonly IRepository<InvoiceDetail,Guid> _repository;

		/// <summary>
		/// InvoiceDetail的构造方法
		///</summary>
		public InvoiceDetailManager(
			IRepository<InvoiceDetail, Guid> repository
		)
		{
			_repository =  repository;
		}


		/// <summary>
		/// 初始化
		///</summary>
		public void InitInvoiceDetail()
		{
			throw new NotImplementedException();
		}

		// TODO:编写领域业务代码



		 
		  
		 

	}
}
