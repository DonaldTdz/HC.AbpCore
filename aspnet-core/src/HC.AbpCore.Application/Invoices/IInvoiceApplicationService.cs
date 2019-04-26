
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


using HC.AbpCore.Invoices.Dtos;
using HC.AbpCore.Invoices;

namespace HC.AbpCore.Invoices
{
    /// <summary>
    /// Invoice应用层服务的接口方法
    ///</summary>
    public interface IInvoiceAppService : IApplicationService
    {
        /// <summary>
		/// 获取Invoice的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<InvoiceListDto>> GetPagedAsync(GetInvoicesInput input);


		/// <summary>
		/// 通过指定id获取InvoiceListDto信息
		/// </summary>
		Task<InvoiceListDto> GetByIdAsync(EntityDto<Guid> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetInvoiceForEditOutput> GetForEditAsync(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改Invoice的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateAsync(CreateOrUpdateInvoiceInput input);

        /// <summary>
        /// 获取发票抬头
        /// </summary>
        /// <param name="type"></param>
        /// <param name="refId"></param>
        /// <returns></returns>
        Task<string> GetTitleByTypeAndRefIdAsync(InvoiceTypeEnum type, Guid refId);


        /// <summary>
        /// 删除Invoice信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAsync(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除Invoice
        /// </summary>
        Task BatchDeleteAsync(List<Guid> input);


		/// <summary>
        /// 导出Invoice为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetToExcel();

    }
}
