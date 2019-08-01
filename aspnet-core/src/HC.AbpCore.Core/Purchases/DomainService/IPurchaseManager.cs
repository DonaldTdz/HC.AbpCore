

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.AdvancePayments;
using HC.AbpCore.Products;
using HC.AbpCore.Purchases;
using HC.AbpCore.Purchases.PurchaseDetails;

namespace HC.AbpCore.Purchases.DomainService
{
    public interface IPurchaseManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitPurchase();



        /// <summary>
        /// Web一键新增采购,采购明细,产品,预付款计划
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<string> OnekeyCreateAsync(Purchase purchase, List<PurchaseDetailNew> purchaseDetailNews, List<AdvancePayment> advancePayments);



    }
}
