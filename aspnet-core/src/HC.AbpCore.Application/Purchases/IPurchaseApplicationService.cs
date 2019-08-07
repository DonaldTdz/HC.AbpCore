
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


using HC.AbpCore.Purchases.Dtos;
using HC.AbpCore.Purchases;

namespace HC.AbpCore.Purchases
{
    /// <summary>
    /// Purchase应用层服务的接口方法
    ///</summary>
    public interface IPurchaseAppService : IApplicationService
    {
        /// <summary>
		/// 获取Purchase的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<PurchaseListDto>> GetPagedAsync(GetPurchasesInput input);


		/// <summary>
		/// 通过指定id获取PurchaseListDto信息
		/// </summary>
		Task<PurchaseListDto> GetByIdAsync(EntityDto<Guid> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetPurchaseForEditOutput> GetForEditAsync(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改Purchase的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateAsync(CreateOrUpdatePurchaseInput input);


        /// <summary>
        /// 删除Purchase信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAsync(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除Purchase
        /// </summary>
        Task BatchDeleteAsync(List<Guid> input);


        /// <summary>
        /// Web一键新增采购,采购明细,产品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<string> OnekeyCreateAsync(OnekeyCreatePurchaseInput input);


        /// <summary>
        /// 自动生成采购编号
        /// </summary>
        /// <returns></returns>
        Task<string> GetGeneratePurchaseCodeAsync();


        /// <summary>
        /// 导出Purchase为excel表
        /// </summary>
        /// <returns></returns>
        //Task<FileDto> GetToExcel();

    }
}
