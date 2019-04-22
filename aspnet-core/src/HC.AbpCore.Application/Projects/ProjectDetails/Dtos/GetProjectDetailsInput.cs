
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.Projects.ProjectDetails;
using System;

namespace HC.AbpCore.Projects.ProjectDetails.Dtos
{
    public class GetProjectDetailsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
        /// 查询条件-项目明细分类
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 查询条件-商品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 查询条件-项目Id
        /// </summary>
        public Guid? ProjectId { get; set; }

    }
}
