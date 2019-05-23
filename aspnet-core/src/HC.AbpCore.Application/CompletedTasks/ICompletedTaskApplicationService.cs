
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


using HC.AbpCore.Tasks.Dtos;
using HC.AbpCore.Tasks;

namespace HC.AbpCore.Tasks
{
    /// <summary>
    /// CompletedTask应用层服务的接口方法
    ///</summary>
    public interface ICompletedTaskAppService : IApplicationService
    {
        /// <summary>
		/// 获取CompletedTask的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<CompletedTaskListDto>> GetPagedAsync(GetCompletedTasksInput input);


		/// <summary>
		/// 通过指定id获取CompletedTaskListDto信息
		/// </summary>
		Task<CompletedTaskListDto> GetByIdAsync(EntityDto<Guid> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetCompletedTaskForEditOutput> GetForEditAsync(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改CompletedTask的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateAsync(CreateOrUpdateCompletedTaskInput input);


        /// <summary>
        /// 删除CompletedTask信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAsync(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除CompletedTask
        /// </summary>
        Task BatchDeleteAsync(List<Guid> input);


		/// <summary>
        /// 导出CompletedTask为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetToExcel();

    }
}
