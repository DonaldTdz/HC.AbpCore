
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.Implements;
using System;

namespace HC.AbpCore.Implements.Dtos
{
    public class GetImplementsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
        /// 查询条件-项目Id
        /// </summary>
        public Guid? ProjectId { get; set; }

    }
}
