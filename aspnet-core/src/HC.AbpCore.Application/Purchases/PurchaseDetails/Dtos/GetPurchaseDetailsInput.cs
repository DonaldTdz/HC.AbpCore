
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.Purchases.PurchaseDetails;
using System;

namespace HC.AbpCore.Purchases.PurchaseDetails.Dtos
{
    public class GetPurchaseDetailsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {

        /// <summary>
        /// 正常化排序使用
        /// </summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }

        /// <summary>
        /// 查询条件-采购Id
        /// </summary>
        public Guid? PurchaseId { get; set; }

    }
}
