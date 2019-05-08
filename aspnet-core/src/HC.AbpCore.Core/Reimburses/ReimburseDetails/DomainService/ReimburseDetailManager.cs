

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
using HC.AbpCore.Reimburses.ReimburseDetails;


namespace HC.AbpCore.Reimburses.ReimburseDetails.DomainService
{
    /// <summary>
    /// ReimburseDetail领域层的业务管理
    ///</summary>
    public class ReimburseDetailManager :AbpCoreDomainServiceBase, IReimburseDetailManager
    {
		
		private readonly IRepository<ReimburseDetail,Guid> _repository;

		/// <summary>
		/// ReimburseDetail的构造方法
		///</summary>
		public ReimburseDetailManager(
			IRepository<ReimburseDetail, Guid> repository
		)
		{
			_repository =  repository;
		}


		/// <summary>
		/// 初始化
		///</summary>
		public void InitReimburseDetail()
		{
			throw new NotImplementedException();
		}

		// TODO:编写领域业务代码



		 
		  
		 

	}
}
