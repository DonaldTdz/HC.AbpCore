
using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using Abp.UI;
using Abp.AutoMapper;
using Abp.Authorization;
using Abp.Linq.Extensions;
using Abp.Domain.Repositories;
using Abp.Application.Services;
using Abp.Application.Services.Dto;


using HC.AbpCore.Projects.Dtos;
using HC.AbpCore.Projects;
using HC.AbpCore.Dtos;
using static HC.AbpCore.Projects.ProjectBase;

namespace HC.AbpCore.Projects
{
    /// <summary>
    /// Project应用层服务的接口方法
    ///</summary>
    public interface IProjectAppService : IApplicationService
    {
        /// <summary>
		/// 获取Project的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ProjectListDto>> GetPagedAsync(GetProjectsInput input);


		/// <summary>
		/// 通过指定id获取ProjectListDto信息
		/// </summary>
		Task<ProjectListDto> GetByIdAsync(EntityDto<Guid> input);


        /// <summary>
        /// 获取项目下拉列表
        /// </summary>
        /// <returns></returns>
        Task<List<DropDownDto>> GetDropDownsAsync();


        /// <summary>
        /// 自动生成项目编号
        /// </summary>
        /// <returns></returns>
        Task<string> GenerateProjectCodeAsync(string type);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetProjectForEditOutput> GetForEditAsync(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改Project的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<APIResultDto> CreateOrUpdateAsync(CreateOrUpdateProjectInput input);


        /// <summary>
        /// 添加project以及projectDetail的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<APIResultDto> CreateProjectAndDetailAsync(CreateProjectAndDetailInput input);


        /// <summary>
        /// 删除Project信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAsync(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除Project
        /// </summary>
        Task BatchDeleteAsync(List<Guid> input);


        /// <summary>
        /// 修改项目状态
        /// </summary>
        /// <param name="input"></param>
        /// <param name="projectStatus"></param>
        /// <returns></returns>
        Task ModifyProjectStatusAsync(Guid id, int projectStatus);

        /// <summary>
        /// 导出Project为excel表
        /// </summary>
        /// <returns></returns>
        //Task<FileDto> GetToExcel();

    }
}
