
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


using HC.AbpCore.Customers.Dtos;
using HC.AbpCore.Customers;
using HC.AbpCore.Dtos;

namespace HC.AbpCore.Customers
{
    /// <summary>
    /// Customer应用层服务的接口方法
    ///</summary>
    public interface ICustomerAppService : IApplicationService
    {
        /// <summary>
		/// 获取Customer的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<CustomerListDto>> GetPagedAsync(GetCustomersInput input);


		/// <summary>
		/// 通过指定id获取CustomerListDto信息
		/// </summary>
		Task<CustomerListDto> GetByIdAsync(EntityDto<int> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetCustomerForEditOutput> GetForEditAsync(NullableIdDto<int> input);


        /// <summary>
        /// 获取客户下拉列表
        /// </summary>
        /// <returns></returns>
        Task<List<DropDownDto>> GetDropDownDtosAsync();


        /// <summary>
        /// 添加或者修改Customer的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<APIResultDto> CreateOrUpdateAsync(CustomerEditDto input);


        /// <summary>
        /// 删除Customer信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAsync(EntityDto<int> input);


        /// <summary>
        /// 批量删除Customer
        /// </summary>
        Task BatchDelete(List<int> input);


		/// <summary>
        /// 导出Customer为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetToExcel();

    }
}
