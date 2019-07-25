

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

        /// <summary>
        /// 编辑明细并更新产品表
        /// </summary>
        /// <param name="purchaseDetail"></param>
        /// <returns></returns>
        Task UpdateAsync(PurchaseDetail purchaseDetail);


        /// <summary>
        /// 新增明细并更新产品表
        /// </summary>
        /// <param name="purchaseDetailNew"></param>
        /// <returns></returns>
        Task<PurchaseDetail> CreateAsync(PurchaseDetailNew purchaseDetailNew);


        /// <summary>
        /// 删除明细并更新产品表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid id);
    }
}
