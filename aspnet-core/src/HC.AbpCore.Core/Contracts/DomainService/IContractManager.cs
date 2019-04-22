

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.Contracts;


namespace HC.AbpCore.Contracts.DomainService
{
    public interface IContractManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitContract();



		 
      
         

    }
}
