

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
using HC.AbpCore.AdvancePayments;


namespace HC.AbpCore.AdvancePayments.DomainService
{
    /// <summary>
    /// AdvancePayment领域层的业务管理
    ///</summary>
    public class AdvancePaymentManager :AbpCoreDomainServiceBase, IAdvancePaymentManager
    {
		
		private readonly IRepository<AdvancePayment,Guid> _repository;

		/// <summary>
		/// AdvancePayment的构造方法
		///</summary>
		public AdvancePaymentManager(
			IRepository<AdvancePayment, Guid> repository
		)
		{
			_repository =  repository;
		}


		/// <summary>
		/// 初始化
		///</summary>
		public void InitAdvancePayment()
		{
			throw new NotImplementedException();
		}

		// TODO:编写领域业务代码



		 
		  
		 

	}
}
