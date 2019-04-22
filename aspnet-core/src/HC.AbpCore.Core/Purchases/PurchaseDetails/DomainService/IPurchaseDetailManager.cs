

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.Purchases.PurchaseDetails;


namespace HC.AbpCore.Purchases.PurchaseDetails.DomainService
{
    public interface IPurchaseDetailManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitPurchaseDetail();



		 
      
         

    }
}
