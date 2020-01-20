
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


using HC.AbpCore.Reimburses.ReimburseDetails.Dtos;
using HC.AbpCore.Reimburses.ReimburseDetails;

namespace HC.AbpCore.Reimburses.ReimburseDetails
{
    /// <summary>
    /// ReimburseDetail应用层服务的接口方法
    ///</summary>
    public interface IReimburseDetailAppService : IApplicationService
    {
        /// <summary>
		/// 获取ReimburseDetail的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ReimburseDetailListDto>> GetPagedAsync(GetReimburseDetailsInput input);


		/// <summary>
		/// 通过指定id获取ReimburseDetailListDto信息
		/// </summary>
		Task<ReimburseDetailListDto> GetByIdAsync(EntityDto<Guid> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetReimburseDetailForEditOutput> GetForEditAsync(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改ReimburseDetail的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<decimal> CreateOrUpdateAsync(CreateOrUpdateReimburseDetailInput input);


        /// <summary>
        /// 删除ReimburseDetail信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<decimal> DeleteAsync(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除ReimburseDetail
        /// </summary>
        Task BatchDeleteAsync(List<Guid> input);


		/// <summary>
        /// 导出ReimburseDetail为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetToExcel();

    }
}
