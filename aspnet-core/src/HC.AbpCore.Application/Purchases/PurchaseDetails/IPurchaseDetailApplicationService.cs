
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


using HC.AbpCore.Purchases.PurchaseDetails.Dtos;
using HC.AbpCore.Purchases.PurchaseDetails;
using HC.AbpCore.Dtos;

namespace HC.AbpCore.Purchases.PurchaseDetails
{
    /// <summary>
    /// PurchaseDetail应用层服务的接口方法
    ///</summary>
    public interface IPurchaseDetailAppService : IApplicationService
    {
        /// <summary>
		/// 获取PurchaseDetail的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<PurchaseDetailListDtoNew>> GetPagedAsync(GetPurchaseDetailsInput input);


		/// <summary>
		/// 通过指定id获取PurchaseDetailListDto信息
		/// </summary>
		Task<PurchaseDetailListDto> GetByIdAsync(EntityDto<Guid> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetPurchaseDetailForEditOutput> GetForEditAsync(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改PurchaseDetail的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateAsync(CreateOrUpdatePurchaseDetailInput input);


        /// <summary>
        /// 删除PurchaseDetail信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAsync(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除PurchaseDetail
        /// </summary>
        Task BatchDeleteAsync(List<Guid> input);


        /// <summary>
        /// 添加PurchaseDetail并且更新Products的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateDetailAndUpdateproductAsync(CreatePurchaseDetailAndUpdateproductInput input);


        /// <summary>
        /// 删除PurchaseDetail并更新Products的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAndUpdatePurchaseAsync(EntityDto<Guid> input);


        /// <summary>
        /// 更新PurchaseDetail并且更新Products的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ModifyDetailAndUpdateproductAsync(CreateOrUpdatePurchaseDetailInput input);


        /// <summary>
        /// 获取采购明细选择列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<DropDownDto>> GetDetailSelectAsync(EntityDto<Guid> input);


        /// <summary>
        /// 导出PurchaseDetail为excel表
        /// </summary>
        /// <returns></returns>
        //Task<FileDto> GetToExcel();

    }
}
