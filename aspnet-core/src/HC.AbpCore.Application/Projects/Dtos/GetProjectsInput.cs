
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.Projects;
using System;
using static HC.AbpCore.Projects.ProjectBase;

namespace HC.AbpCore.Projects.Dtos
{
    public class GetProjectsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
        /// 查询条件-所属客户
        /// </summary>
        public int? CustomerId { get; set; }

        /// <summary>
        /// 查询条件-Id
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 查询条件-项目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 查询条件-项目状态
        /// </summary>
        public ProjectStatus? Status { get; set; }

    }
}
