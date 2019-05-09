
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.PaymentPlans;
using System;

namespace HC.AbpCore.PaymentPlans.Dtos
{
    public class GetPaymentPlansInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
        /// 查询条件-所属项目
        /// </summary>
        public Guid? ProjectId { get; set; }

    }
}
