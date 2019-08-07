

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
using HC.AbpCore.Products;

namespace HC.AbpCore.InventoryFlows.DomainService
{
    /// <summary>
    /// InventoryFlow领域层的业务管理
    ///</summary>
    public class InventoryFlowManager :AbpCoreDomainServiceBase, IInventoryFlowManager
    {
		
		private readonly IRepository<InventoryFlow,long> _repository;
        private readonly IRepository<Product, int> _productRepository;

        /// <summary>
        /// InventoryFlow的构造方法
        ///</summary>
        public InventoryFlowManager(
			IRepository<InventoryFlow, long> repository
            , IRepository<Product, int> productRepository
        )
		{
            _productRepository = productRepository;
            _repository =  repository;
		}


		/// <summary>
		/// 初始化
		///</summary>
		public void InitInventoryFlow()
		{
			throw new NotImplementedException();
		}

        public Task UpdateAsync(Guid? contractId, int productId, int num)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(Guid? contractId, int productId, int num)
        {
            //var products = await _productRepository.FirstOrDefaultAsync(aa => aa.Id == productId);
            //InventoryFlow inventoryFlow = new InventoryFlow();
            //inventoryFlow.Desc = "合同明细出库";
            //inventoryFlow
            throw new NotImplementedException();
        }

        // TODO:编写领域业务代码







    }
}
