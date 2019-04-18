
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.Tenders;
using System;

namespace HC.AbpCore.Tenders.Dtos
{
    public class GetTendersInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
        /// 查询条件-项目id
        /// </summary>
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// 查询条件-负责人
        /// </summary>
        public string EmployeeId { get; set; }

    }
}
