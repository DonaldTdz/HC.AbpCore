
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


using HC.AbpCore.Reimburses.Dtos;
using HC.AbpCore.Reimburses;

namespace HC.AbpCore.Reimburses
{
    /// <summary>
    /// Reimburse应用层服务的接口方法
    ///</summary>
    public interface IReimburseAppService : IApplicationService
    {
        /// <summary>
		/// 获取Reimburse的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ReimburseListDto>> GetPagedAsync(GetReimbursesInput input);


		/// <summary>
		/// 通过指定id获取ReimburseListDto信息
		/// </summary>
		Task<ReimburseListDto> GetByIdAsync(EntityDto<Guid> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetReimburseForEditOutput> GetForEditAsync(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改Reimburse的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateAsync(CreateOrUpdateReimburseInput input);


        /// <summary>
        /// 删除Reimburse信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAsync(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除Reimburse
        /// </summary>
        Task BatchDeleteAsync(List<Guid> input);


		/// <summary>
        /// 导出Reimburse为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetToExcel();

    }
}
