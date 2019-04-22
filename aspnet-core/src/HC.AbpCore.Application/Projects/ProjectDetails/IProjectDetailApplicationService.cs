
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


using HC.AbpCore.Projects.ProjectDetails.Dtos;
using HC.AbpCore.Projects.ProjectDetails;
using HC.AbpCore.Dtos;

namespace HC.AbpCore.Projects.ProjectDetails
{
    /// <summary>
    /// ProjectDetail应用层服务的接口方法
    ///</summary>
    public interface IProjectDetailAppService : IApplicationService
    {
        /// <summary>
		/// 获取ProjectDetail的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ProjectDetailListDto>> GetPagedAsync(GetProjectDetailsInput input);


		/// <summary>
		/// 通过指定id获取ProjectDetailListDto信息
		/// </summary>
		Task<ProjectDetailListDto> GetByIdAsync(EntityDto<Guid> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetProjectDetailForEditOutput> GetForEditAsync(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改ProjectDetail的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateAsync(CreateOrUpdateProjectDetailInput input);


        /// <summary>
        /// 删除ProjectDetail信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAsync(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除ProjectDetail
        /// </summary>
        Task BatchDeleteAsync(List<Guid> input);

        /// <summary>
        /// 根据项目id获取下拉列表
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Task<List<DropDownDto>> GetDropDownsByProjectIdAsync(Guid projectId);


        /// <summary>
        /// 导出ProjectDetail为excel表
        /// </summary>
        /// <returns></returns>
        //Task<FileDto> GetToExcel();

    }
}
