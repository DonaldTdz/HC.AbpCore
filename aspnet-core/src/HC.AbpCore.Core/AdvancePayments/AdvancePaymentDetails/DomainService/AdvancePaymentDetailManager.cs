

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
using HC.AbpCore.AdvancePayments.AdvancePaymentDetails;


namespace HC.AbpCore.AdvancePayments.AdvancePaymentDetails.DomainService
{
    /// <summary>
    /// AdvancePaymentDetail领域层的业务管理
    ///</summary>
    public class AdvancePaymentDetailManager :AbpCoreDomainServiceBase, IAdvancePaymentDetailManager
    {
		
		private readonly IRepository<AdvancePaymentDetail,Guid> _repository;

		/// <summary>
		/// AdvancePaymentDetail的构造方法
		///</summary>
		public AdvancePaymentDetailManager(
			IRepository<AdvancePaymentDetail, Guid> repository
		)
		{
			_repository =  repository;
		}


		/// <summary>
		/// 初始化
		///</summary>
		public void InitAdvancePaymentDetail()
		{
			throw new NotImplementedException();
		}

		// TODO:编写领域业务代码



		 
		  
		 

	}
}
