

using System;
using System.Threading.Tasks;
using Abp;
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



		 
      
         

    }
}
