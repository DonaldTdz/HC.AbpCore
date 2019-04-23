

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

namespace HC.AbpCore.Contracts.ContractDetails.DomainService
{
    /// <summary>
    /// ContractDetail领域层的业务管理
    ///</summary>
    public class ContractDetailManager :AbpCoreDomainServiceBase, IContractDetailManager
    {
		
		private readonly IRepository<ContractDetail,Guid> _repository;
        private readonly IRepository<Contract, Guid> _contractRepository;
        private readonly IRepository<ProjectDetail, Guid> _projectDetailRepository;
        private readonly IRepository<PurchaseDetail, Guid> _purchaseDetailRepository;

        /// <summary>
        /// ContractDetail的构造方法
        ///</summary>
        public ContractDetailManager(
			IRepository<ContractDetail, Guid> repository,
             IRepository<Contract, Guid> contractRepository
                        , IRepository<ProjectDetail, Guid> projectDetailRepository
            , IRepository<PurchaseDetail, Guid> purchaseDetailRepository
        )
		{
			_repository =  repository;
            _contractRepository = contractRepository;
            _projectDetailRepository = projectDetailRepository;
            _purchaseDetailRepository = purchaseDetailRepository;
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
                decimal detailAmount = 0;
                ProjectDetail projectDetail = new ProjectDetail();
                if (input.RefDetailId.HasValue)
                {
                    if (contract.Type == ContractTypeEnum.销项)
                    {
                        projectDetail = await _projectDetailRepository.GetAsync(input.RefDetailId.Value);
                        if (projectDetail.Num.HasValue && projectDetail.Price.HasValue)
                            detailAmount = projectDetail.Num.Value * projectDetail.Price.Value;
                    }
                    else
                    {
                        var purchaseDetail = await _purchaseDetailRepository.GetAsync(input.RefDetailId.Value);
                        if (purchaseDetail.ProjectDetailId.HasValue)
                            projectDetail = await _projectDetailRepository.GetAsync(purchaseDetail.ProjectDetailId.Value);
                        if (projectDetail.Num.HasValue && purchaseDetail.Price.HasValue)
                            detailAmount = projectDetail.Num.Value * purchaseDetail.Price.Value;
                    }
                }
                contract.Amount += detailAmount;
                await _contractRepository.UpdateAsync(contract);
            }


            // var entity = ObjectMapper.Map <ContractDetail>(input);

            input = await _repository.InsertAsync(input);
            return input;
        }

        /// <summary>
        /// 编辑ContractDetail
        /// </summary>

        public async Task UpdateAsync(ContractDetail input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _repository.GetAsync(input.Id);
            //修改合同金额
            if (entity.RefDetailId != input.RefDetailId && input.ContractId.HasValue)
            {
                var contract = await _contractRepository.GetAsync(input.ContractId.Value);
                decimal detailAmount = 0;
                ProjectDetail projectDetail = new ProjectDetail();
                if (input.RefDetailId.HasValue)
                {
                    if (contract.Type == ContractTypeEnum.销项)
                    {
                        projectDetail = await _projectDetailRepository.GetAsync(input.RefDetailId.Value);
                        if (projectDetail.Num.HasValue && projectDetail.Price.HasValue)
                            detailAmount = projectDetail.Num.Value * projectDetail.Price.Value;
                    }
                    else
                    {
                        var purchaseDetail = await _purchaseDetailRepository.GetAsync(input.RefDetailId.Value);
                        if (purchaseDetail.ProjectDetailId.HasValue)
                            projectDetail = await _projectDetailRepository.GetAsync(purchaseDetail.ProjectDetailId.Value);
                        if (projectDetail.Num.HasValue && purchaseDetail.Price.HasValue)
                            detailAmount = projectDetail.Num.Value * purchaseDetail.Price.Value;
                    }
                }
                contract.Amount += detailAmount;
                await _contractRepository.UpdateAsync(contract);
            }
            

            // ObjectMapper.Map(input, entity);
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(Guid Id)
        {
            var entity = await _repository.GetAsync(Id);
            if (entity.ContractId.HasValue)
            {
                var contract = await _contractRepository.GetAsync(entity.ContractId.Value);
                decimal detailAmount = 0;
                ProjectDetail projectDetail = new ProjectDetail();
                if (entity.RefDetailId.HasValue)
                {
                    if (contract.Type == ContractTypeEnum.销项)
                    {
                        projectDetail = await _projectDetailRepository.GetAsync(entity.RefDetailId.Value);
                        if (projectDetail.Num.HasValue && projectDetail.Price.HasValue)
                            detailAmount = projectDetail.Num.Value * projectDetail.Price.Value;
                    }
                    else
                    {
                        var purchaseDetail = await _purchaseDetailRepository.GetAsync(entity.RefDetailId.Value);
                        if (purchaseDetail.ProjectDetailId.HasValue)
                            projectDetail = await _projectDetailRepository.GetAsync(purchaseDetail.ProjectDetailId.Value);
                        if (projectDetail.Num.HasValue && purchaseDetail.Price.HasValue)
                            detailAmount = projectDetail.Num.Value * purchaseDetail.Price.Value;
                    }
                }
                contract.Amount -= detailAmount;
                await _contractRepository.UpdateAsync(contract);
            }
            await _repository.DeleteAsync(Id);
        }
    }
}
