
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.AdvancePayments;
using System;

namespace HC.AbpCore.AdvancePayments.Dtos
{
    public class GetAdvancePaymentsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
        /// 查询条件-采购id
        /// </summary>
        public Guid? PurchaseId { get; set; }

    }
}
