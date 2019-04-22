
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


using HC.AbpCore.Products.Dtos;
using HC.AbpCore.Products;
using HC.AbpCore.Dtos;

namespace HC.AbpCore.Products
{
    /// <summary>
    /// Product应用层服务的接口方法
    ///</summary>
    public interface IProductAppService : IApplicationService
    {
        /// <summary>
		/// 获取Product的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ProductListDto>> GetPagedAsync(GetProductsInput input);


		/// <summary>
		/// 通过指定id获取ProductListDto信息
		/// </summary>
		Task<ProductListDto> GetByIdAsync(EntityDto<int> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetProductForEditOutput> GetForEditAsync(NullableIdDto<int> input);


        /// <summary>
        /// 添加或者修改Product的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<APIResultDto> CreateOrUpdateAsync(CreateOrUpdateProductInput input);


        /// <summary>
        /// 删除Product信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAsync(EntityDto<int> input);


        /// <summary>
        /// 批量删除Product
        /// </summary>
        Task BatchDeleteAsync(List<int> input);
        

        /// <summary>
        /// 导出Product为excel表
        /// </summary>
        /// <returns></returns>
        //Task<FileDto> GetToExcel();

    }
}
