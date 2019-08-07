

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
using HC.AbpCore.Reports.ProfitStatistics;


namespace HC.AbpCore.Reports.ProfitStatistics.DomainService
{
    /// <summary>
    /// ProfitStatistic领域层的业务管理
    ///</summary>
    public class ProfitStatisticManager :AbpCoreDomainServiceBase, IProfitStatisticManager
    {
		
		private readonly IRepository<ProfitStatistic,Guid> _repository;

		/// <summary>
		/// ProfitStatistic的构造方法
		///</summary>
		public ProfitStatisticManager(
			IRepository<ProfitStatistic, Guid> repository
		)
		{
			_repository =  repository;
		}


		/// <summary>
		/// 初始化
		///</summary>
		public void InitProfitStatistic()
		{
			throw new NotImplementedException();
		}

		// TODO:编写领域业务代码



		 
		  
		 

	}
}
