
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


using HC.AbpCore.InventoryFlows.Dtos;
using HC.AbpCore.InventoryFlows;

namespace HC.AbpCore.InventoryFlows
{
    /// <summary>
    /// InventoryFlow应用层服务的接口方法
    ///</summary>
    public interface IInventoryFlowAppService : IApplicationService
    {
        /// <summary>
		/// 获取InventoryFlow的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<InventoryFlowListDto>> GetPagedAsync(GetInventoryFlowsInput input);


		/// <summary>
		/// 通过指定id获取InventoryFlowListDto信息
		/// </summary>
		Task<InventoryFlowListDto> GetByIdAsync(EntityDto<long> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetInventoryFlowForEditOutput> GetForEditAsync(NullableIdDto<long> input);


        /// <summary>
        /// 添加或者修改InventoryFlow的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateAsync(CreateOrUpdateInventoryFlowInput input);


        /// <summary>
        /// 删除InventoryFlow信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAsync(EntityDto<long> input);


        /// <summary>
        /// 批量删除InventoryFlow
        /// </summary>
        Task BatchDeleteAsync(List<long> input);


		/// <summary>
        /// 导出InventoryFlow为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetToExcel();

    }
}
