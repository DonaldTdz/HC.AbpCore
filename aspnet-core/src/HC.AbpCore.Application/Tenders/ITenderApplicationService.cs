
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


using HC.AbpCore.Tenders.Dtos;
using HC.AbpCore.Tenders;

namespace HC.AbpCore.Tenders
{
    /// <summary>
    /// Tender应用层服务的接口方法
    ///</summary>
    public interface ITenderAppService : IApplicationService
    {
        /// <summary>
		/// 获取Tender的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<TenderListDto>> GetPagedAsync(GetTendersInput input);


        /// <summary>
        /// 通过指定id或者projectId获取TenderListDto信息
        /// </summary>
        Task<TenderListDto> GetByIdAsync(EntityDto<Guid?> input, Guid? projectId);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetTenderForEditOutput> GetForEditAsync(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改Tender的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateAsync(CreateOrUpdateTenderInput input);


        /// <summary>
        /// 删除Tender信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAsync(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除Tender
        /// </summary>
        Task BatchDeleteAsync(List<Guid> input);

        /// <summary>
        /// 发送钉钉工作通知
        /// </summary>
        /// <returns></returns>
        Task<List<GetTenderRemindListDto>> GetTenderRemindData();


        /// <summary>
        /// 导出Tender为excel表
        /// </summary>
        /// <returns></returns>
        //Task<FileDto> GetToExcel();

    }
}
