

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.Projects.ProjectDetails;


namespace HC.AbpCore.Projects.ProjectDetails.DomainService
{
    public interface IProjectDetailManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitProjectDetail();

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="projectDetail"></param>
        /// <returns></returns>
        //Task<ProjectDetail> CreateAsync(List<ProjectDetail> projectDetails);

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="projectDetail"></param>
        /// <returns></returns>
        //Task<ProjectDetail> UpdateAsync(ProjectDetail projectDetail);




    }
}
