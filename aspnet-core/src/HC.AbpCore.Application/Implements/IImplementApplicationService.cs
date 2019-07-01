
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


using HC.AbpCore.Implements.Dtos;
using HC.AbpCore.Implements;

namespace HC.AbpCore.Implements
{
    /// <summary>
    /// Implement应用层服务的接口方法
    ///</summary>
    public interface IImplementAppService : IApplicationService
    {
        /// <summary>
		/// 获取Implement的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ImplementListDto>> GetPagedAsync(GetImplementsInput input);


		/// <summary>
		/// 通过指定id获取ImplementListDto信息
		/// </summary>
		Task<ImplementListDto> GetByIdAsync(EntityDto<Guid> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetImplementForEditOutput> GetForEditAsync(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改Implement的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ImplementEditDto> CreateOrUpdateAsync(CreateOrUpdateImplementInput input);


        /// <summary>
        /// 删除Implement信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAsync(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除Implement
        /// </summary>
        Task BatchDeleteAsync(List<Guid> input);


        /// <summary>
        /// 批量新增Implement
        /// </summary>
        Task BatchCreateOrUpdateAsync(BatchCreateOrUpdateImplementInput input);


        /// <summary>
        /// 导出Implement为excel表
        /// </summary>
        /// <returns></returns>
        //Task<FileDto> GetToExcel();

    }
}
