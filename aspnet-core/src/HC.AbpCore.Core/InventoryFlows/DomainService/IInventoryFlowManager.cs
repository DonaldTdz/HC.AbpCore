

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.InventoryFlows;


namespace HC.AbpCore.InventoryFlows.DomainService
{
    public interface IInventoryFlowManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitInventoryFlow();



		 
      
         

    }
}
