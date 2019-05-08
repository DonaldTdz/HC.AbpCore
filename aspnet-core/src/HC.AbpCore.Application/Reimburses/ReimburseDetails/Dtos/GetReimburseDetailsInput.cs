
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.Reimburses.ReimburseDetails;
using System;

namespace HC.AbpCore.Reimburses.ReimburseDetails.Dtos
{
    public class GetReimburseDetailsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
        /// 查询条件-报销ID
        /// </summary>
        public Guid? ReimburseId
        {
            get;set;
        }

    }
}
