
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.AdvancePayments.AdvancePaymentDetails;
using System;

namespace HC.AbpCore.AdvancePayments.AdvancePaymentDetails.Dtos
{
    public class GetAdvancePaymentDetailsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
        /// 查询条件-预付款Id
        /// </summary>
        public Guid? AdvancePaymentId { get; set; }

    }
}
