

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
using HC.AbpCore.InventoryFlows;


namespace HC.AbpCore.InventoryFlows.DomainService
{
    /// <summary>
    /// InventoryFlow领域层的业务管理
    ///</summary>
    public class InventoryFlowManager :AbpCoreDomainServiceBase, IInventoryFlowManager
    {
		
		private readonly IRepository<InventoryFlow,long> _repository;

		/// <summary>
		/// InventoryFlow的构造方法
		///</summary>
		public InventoryFlowManager(
			IRepository<InventoryFlow, long> repository
		)
		{
			_repository =  repository;
		}


		/// <summary>
		/// 初始化
		///</summary>
		public void InitInventoryFlow()
		{
			throw new NotImplementedException();
		}

		// TODO:编写领域业务代码



		 
		  
		 

	}
}
