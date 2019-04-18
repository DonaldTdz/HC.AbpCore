

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.Purchases;


namespace HC.AbpCore.Purchases.DomainService
{
    public interface IPurchaseManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitPurchase();



		 
      
         

    }
}
