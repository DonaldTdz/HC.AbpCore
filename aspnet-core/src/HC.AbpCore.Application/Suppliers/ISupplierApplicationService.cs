
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


using HC.AbpCore.Suppliers.Dtos;
using HC.AbpCore.Suppliers;

namespace HC.AbpCore.Suppliers
{
    /// <summary>
    /// Supplier应用层服务的接口方法
    ///</summary>
    public interface ISupplierAppService : IApplicationService
    {
        /// <summary>
		/// 获取Supplier的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<SupplierListDto>> GetPagedAsync(GetSuppliersInput input);


		/// <summary>
		/// 通过指定id获取SupplierListDto信息
		/// </summary>
		Task<SupplierListDto> GetByIdAsync(EntityDto<int> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetSupplierForEditOutput> GetForEditAsync(NullableIdDto<int> input);


        /// <summary>
        /// 添加或者修改Supplier的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateAsync(CreateOrUpdateSupplierInput input);


        /// <summary>
        /// 删除Supplier信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAsync(EntityDto<int> input);


        /// <summary>
        /// 批量删除Supplier
        /// </summary>
        Task BatchDeleteAsync(List<int> input);


		/// <summary>
        /// 导出Supplier为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetToExcel();

    }
}
