

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
using HC.AbpCore.Contracts.ContractDetails;
using HC.AbpCore.Projects.ProjectDetails;
using HC.AbpCore.Purchases.PurchaseDetails;
using static HC.AbpCore.Contracts.ContractEnum;
using Abp.Domain.Entities;
using HC.AbpCore.Products;
using HC.AbpCore.InventoryFlows;

namespace HC.AbpCore.Contracts.ContractDetails.DomainService
{
    /// <summary>
    /// ContractDetail领域层的业务管理
    ///</summary>
    public class ContractDetailManager : AbpCoreDomainServiceBase, IContractDetailManager
    {

        private readonly IRepository<ContractDetail, Guid> _repository;
        private readonly IRepository<Contract, Guid> _contractRepository;
        private readonly IRepository<Product, int> _productRepository;
        private readonly IRepository<InventoryFlow, long> _inventoryFlowRepository;

        /// <summary>
        /// ContractDetail的构造方法
        ///</summary>
        public ContractDetailManager(
            IRepository<ContractDetail, Guid> repository,
            IRepository<Contract, Guid> contractRepository,
            IRepository<Product, int> productRepository,
            IRepository<InventoryFlow, long> inventoryFlowRepository
        )
        {
            _productRepository = productRepository;
            _repository = repository;
            _contractRepository = contractRepository;
            _inventoryFlowRepository = inventoryFlowRepository;
        }


        /// <summary>
        /// 初始化
        ///</summary>
        public void InitContractDetail()
        {
            throw new NotImplementedException();
        }

        // TODO:编写领域业务代码

        /// <summary>
        /// 新增ContractDetail
        /// </summary>

        public async Task<ContractDetail> CreateAsync(ContractDetail input)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            //修改合同金额
            if (input.ContractId.HasValue)
            {
                var contract = await _contractRepository.GetAsync(input.ContractId.Value);
                contract.Amount += input.Num * input.Price;
                await _contractRepository.UpdateAsync(contract);
            }
            input = await _repository.InsertAsync(input);
            var product = await _productRepository.FirstOrDefaultAsync(aa => aa.Id == input.ProductId);
            if (input.Num.HasValue)
            {
                //更新库存流水
                InventoryFlow inventoryFlow = new InventoryFlow();
                inventoryFlow.Desc = "出库";
                inventoryFlow.Initial = product.Num;
                inventoryFlow.StreamNumber = input.Num;
                inventoryFlow.Ending = product.Num - input.Num;
                inventoryFlow.ProductId = product.Id;
                inventoryFlow.RefId = input.Id.ToString();
                inventoryFlow.Type = InventoryFlowType.出库;
                await _inventoryFlowRepository.InsertAsync(inventoryFlow);
                product.Num -= input.Num;
                await _productRepository.UpdateAsync(product);
            }


            // var entity = ObjectMapper.Map <ContractDetail>(input);
            //await CurrentUnitOfWork.SaveChangesAsync();
            return input;
        }

        /// <summary>
        /// 编辑ContractDetail
        /// </summary>

        public async Task<ContractDetail> UpdateAsync(ContractDetail input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _repository.GetAsync(input.Id);
            var product = await _productRepository.FirstOrDefaultAsync(aa => aa.Id == entity.ProductId);
            if (input.ProductId == entity.ProductId)
            {
                if (entity.Num > input.Num)
                {
                    //更新库存流水
                    InventoryFlow inventoryFlow = new InventoryFlow();
                    inventoryFlow.Desc = "合同明细更新产生的退货";
                    inventoryFlow.Initial = product.Num;
                    inventoryFlow.StreamNumber = entity.Num-input.Num;
                    inventoryFlow.Ending = product.Num + entity.Num - input.Num;
                    inventoryFlow.ProductId = product.Id;
                    inventoryFlow.RefId = input.Id.ToString();
                    inventoryFlow.Type = InventoryFlowType.入库;
                    await _inventoryFlowRepository.InsertAsync(inventoryFlow);
                }
                else if (entity.Num < input.Num)
                {
                    //更新库存流水
                    InventoryFlow inventoryFlow = new InventoryFlow();
                    inventoryFlow.Desc = "合同明细更新产生的新增出库";
                    inventoryFlow.Initial = product.Num;
                    inventoryFlow.StreamNumber =  input.Num- entity.Num;
                    inventoryFlow.Ending = product.Num + entity.Num - input.Num;
                    inventoryFlow.ProductId = product.Id;
                    inventoryFlow.RefId = input.Id.ToString();
                    inventoryFlow.Type = InventoryFlowType.出库;
                    await _inventoryFlowRepository.InsertAsync(inventoryFlow);
                }
                else
                {

                }
                product.Num = product.Num + entity.Num - input.Num;
                await _productRepository.UpdateAsync(product);
            }
            else
            {
                var productNew = await _productRepository.GetAsync(input.ProductId.Value);
                #region  把更新合同之前的货物返回到库存
                InventoryFlow inventoryFlowOld = new InventoryFlow();
                inventoryFlowOld.Desc = "合同明细更新产生的退货";
                inventoryFlowOld.Initial = product.Num;
                inventoryFlowOld.StreamNumber = entity.Num;
                inventoryFlowOld.Ending = product.Num + entity.Num;
                inventoryFlowOld.ProductId = product.Id;
                inventoryFlowOld.RefId = input.Id.ToString();
                inventoryFlowOld.Type = InventoryFlowType.入库;
                await _inventoryFlowRepository.InsertAsync(inventoryFlowOld);
                #endregion
                product.Num += entity.Num;
                await _productRepository.UpdateAsync(product);
                #region  把更新合同之后的货物进行出库
                InventoryFlow inventoryFlowNew = new InventoryFlow();
                inventoryFlowNew.Desc = "合同明细更新产生的出库";
                inventoryFlowNew.Initial = productNew.Num;
                inventoryFlowNew.StreamNumber = input.Num;
                inventoryFlowNew.Ending = productNew.Num - input.Num;
                inventoryFlowNew.ProductId = productNew.Id;
                inventoryFlowNew.RefId = input.Id.ToString();
                inventoryFlowNew.Type = InventoryFlowType.出库;
                await _inventoryFlowRepository.InsertAsync(inventoryFlowNew);
                #endregion
                if (productNew.Num.HasValue)
                    productNew.Num -= input.Num;
                else
                    productNew.Num = 0 - input.Num;
                await _productRepository.UpdateAsync(productNew);
            }
            //修改合同金额
            if (input.ContractId.HasValue)
            {
                var contract = await _contractRepository.GetAsync(input.ContractId.Value);
                contract.Amount = contract.Amount + input.Num * input.Price - entity.Num * entity.Price;
                await _contractRepository.UpdateAsync(contract);
            }
            //ObjectMapper.Map(input, entity);
            entity.ProductId = input.ProductId;
            entity.Model = input.Model;
            entity.Name = input.Name;
            entity.Num = input.Num;
            entity.Price = input.Price;
            entity = await _repository.UpdateAsync(entity);
            return entity;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Guid Id)
        {
            var entity = await _repository.GetAsync(Id);
            var product = await _productRepository.FirstOrDefaultAsync(aa => aa.Id == entity.ProductId);
            if (product != null)
            {
                //更新库存流水
                InventoryFlow inventoryFlow = new InventoryFlow();
                inventoryFlow.Desc = "合同明细更改参生的退货";
                inventoryFlow.Initial = product.Num;
                inventoryFlow.StreamNumber = entity.Num;
                inventoryFlow.Ending = product.Num + entity.Num;
                inventoryFlow.ProductId = product.Id;
                inventoryFlow.RefId = entity.Id.ToString();
                inventoryFlow.Type = InventoryFlowType.入库;
                await _inventoryFlowRepository.InsertAsync(inventoryFlow);
                product.Num += entity.Num;
                await _productRepository.UpdateAsync(product);
            }
            if (entity.ContractId.HasValue)
            {
                var contract = await _contractRepository.GetAsync(entity.ContractId.Value);
                contract.Amount -= entity.Num * entity.Price;
                await _contractRepository.UpdateAsync(contract);
            }
            await _repository.DeleteAsync(Id);
        }
    }
}
