

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Entities;
using Abp.Domain.Services;
using HC.AbpCore.Contracts.ContractDetails;


namespace HC.AbpCore.Contracts.ContractDetails.DomainService
{
    public interface IContractDetailManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitContractDetail();


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ContractDetail> CreateAsync(ContractDetail input);


        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ContractDetail> UpdateAsync(ContractDetail input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid Id);


    }
}
