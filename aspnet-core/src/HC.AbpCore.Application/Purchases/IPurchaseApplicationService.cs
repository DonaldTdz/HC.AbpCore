
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
using HC.AbpCore.Dtos;

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
        Task<APIResultDto> CreateOrUpdateAsync(CreateOrUpdatePurchaseInput input);


        /// <summary>
        /// 添加Purchase以及PurchaseDetail的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<APIResultDto> CreatePurchaseAndDetailAsync(CreatePurchaseAndDetailInput input);


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
        /// 获取采购下拉列表
        /// </summary>
        /// <returns></returns>
        Task<List<DropDownDto>> GetDropDownsAsync();

        /// <summary>
        /// 根据采购分类获取自动生成的采购编号
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<string> GetPurchaseCodeAsync(PurchaseTypeEnum type);

        /// <summary>
        /// 导出Purchase为excel表
        /// </summary>
        /// <returns></returns>
        //Task<FileDto> GetToExcel();

    }
}
