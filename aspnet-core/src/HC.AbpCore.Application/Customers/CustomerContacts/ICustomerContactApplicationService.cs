
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


using HC.AbpCore.Customers.CustomerContacts.Dtos;
using HC.AbpCore.Customers.CustomerContacts;
using HC.AbpCore.Dtos;

namespace HC.AbpCore.Customers.CustomerContacts
{
    /// <summary>
    /// CustomerContact应用层服务的接口方法
    ///</summary>
    public interface ICustomerContactAppService : IApplicationService
    {
        /// <summary>
		/// 获取CustomerContact的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<CustomerContactListDto>> GetPagedAsync(GetCustomerContactsInput input);


		/// <summary>
		/// 通过指定id获取CustomerContactListDto信息
		/// </summary>
		Task<CustomerContactListDto> GetByIdAsync(EntityDto<int> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetCustomerContactForEditOutput> GetForEditAsync(NullableIdDto<int> input);


        /// <summary>
        /// 添加或者修改CustomerContact的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateAsync(CreateOrUpdateCustomerContactInput input);


        /// <summary>
        /// 删除CustomerContact信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAsync(EntityDto<int> input);


        /// <summary>
        /// 批量删除CustomerContact
        /// </summary>
        Task BatchDeleteAsync(List<int> input);


        /// <summary>
        /// 根据客户Id获取客户联系人下拉列表
        /// </summary>
        /// <returns></returns>
        Task<List<DropDownDto>> GetContactByCustomerIdAsync(int customerId);


        /// <summary>
        /// 导出CustomerContact为excel表
        /// </summary>
        /// <returns></returns>
        //Task<FileDto> GetToExcel();

    }
}
