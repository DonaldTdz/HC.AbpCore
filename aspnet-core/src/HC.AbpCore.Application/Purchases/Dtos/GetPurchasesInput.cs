
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.Purchases;
using System;

namespace HC.AbpCore.Purchases.Dtos
{
    public class GetPurchasesInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
        /// 查询条件-采购编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 查询条件-项目Id
        /// </summary>
        public Guid? ProjectId { get; set; }

    }
}
