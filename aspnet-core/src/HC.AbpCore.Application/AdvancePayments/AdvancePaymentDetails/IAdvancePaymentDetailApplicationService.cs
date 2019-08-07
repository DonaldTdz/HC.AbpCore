
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


using HC.AbpCore.AdvancePayments.AdvancePaymentDetails.Dtos;
using HC.AbpCore.AdvancePayments.AdvancePaymentDetails;

namespace HC.AbpCore.AdvancePayments.AdvancePaymentDetails
{
    /// <summary>
    /// AdvancePaymentDetail应用层服务的接口方法
    ///</summary>
    public interface IAdvancePaymentDetailAppService : IApplicationService
    {
        /// <summary>
		/// 获取AdvancePaymentDetail的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<AdvancePaymentDetailListDto>> GetPagedAsync(GetAdvancePaymentDetailsInput input);


		/// <summary>
		/// 通过指定id获取AdvancePaymentDetailListDto信息
		/// </summary>
		Task<AdvancePaymentDetailListDto> GetByIdAsync(EntityDto<Guid> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetAdvancePaymentDetailForEditOutput> GetForEditAsync(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改AdvancePaymentDetail的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateAsync(CreateOrUpdateAdvancePaymentDetailInput input);


        /// <summary>
        /// 删除AdvancePaymentDetail信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAsync(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除AdvancePaymentDetail
        /// </summary>
        Task BatchDeleteAsync(List<Guid> input);


		/// <summary>
        /// 导出AdvancePaymentDetail为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetToExcel();

    }
}
