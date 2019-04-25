
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


using HC.AbpCore.Invoices.InvoiceDetails.Dtos;
using HC.AbpCore.Invoices.InvoiceDetails;

namespace HC.AbpCore.Invoices.InvoiceDetails
{
    /// <summary>
    /// InvoiceDetail应用层服务的接口方法
    ///</summary>
    public interface IInvoiceDetailAppService : IApplicationService
    {
        /// <summary>
		/// 获取InvoiceDetail的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<InvoiceDetailListDto>> GetPagedAsync(GetInvoiceDetailsInput input);


		/// <summary>
		/// 通过指定id获取InvoiceDetailListDto信息
		/// </summary>
		Task<InvoiceDetailListDto> GetByIdAsync(EntityDto<Guid> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetInvoiceDetailForEditOutput> GetForEditAsync(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改InvoiceDetail的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateAsync(CreateOrUpdateInvoiceDetailInput input);


        /// <summary>
        /// 删除InvoiceDetail信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAsync(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除InvoiceDetail
        /// </summary>
        Task BatchDeleteAsync(List<Guid> input);


		/// <summary>
        /// 导出InvoiceDetail为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetToExcel();

    }
}
