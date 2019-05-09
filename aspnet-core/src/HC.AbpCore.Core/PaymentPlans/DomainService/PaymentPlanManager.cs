

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
using HC.AbpCore.PaymentPlans;


namespace HC.AbpCore.PaymentPlans.DomainService
{
    /// <summary>
    /// PaymentPlan领域层的业务管理
    ///</summary>
    public class PaymentPlanManager :AbpCoreDomainServiceBase, IPaymentPlanManager
    {
		
		private readonly IRepository<PaymentPlan,Guid> _repository;

		/// <summary>
		/// PaymentPlan的构造方法
		///</summary>
		public PaymentPlanManager(
			IRepository<PaymentPlan, Guid> repository
		)
		{
			_repository =  repository;
		}


		/// <summary>
		/// 初始化
		///</summary>
		public void InitPaymentPlan()
		{
			throw new NotImplementedException();
		}

		// TODO:编写领域业务代码



		 
		  
		 

	}
}
