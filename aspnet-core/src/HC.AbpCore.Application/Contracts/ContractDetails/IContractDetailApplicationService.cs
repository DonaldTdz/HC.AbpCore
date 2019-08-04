
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


using HC.AbpCore.Contracts.ContractDetails.Dtos;
using HC.AbpCore.Contracts.ContractDetails;
using HC.AbpCore.Dtos;

namespace HC.AbpCore.Contracts.ContractDetails
{
    /// <summary>
    /// ContractDetail应用层服务的接口方法
    ///</summary>
    public interface IContractDetailAppService : IApplicationService
    {
        /// <summary>
		/// 获取ContractDetail的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ContractDetailListDto>> GetPagedAsync(GetContractDetailsInput input);


		/// <summary>
		/// 通过指定id获取ContractDetailListDto信息
		/// </summary>
		Task<ContractDetailListDto> GetByIdAsync(EntityDto<Guid> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetContractDetailForEditOutput> GetForEditAsync(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改ContractDetail的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ContractDetail> CreateOrUpdateAsync(CreateOrUpdateContractDetailInput input);


        /// <summary>
        /// 删除ContractDetail信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAsync(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除ContractDetail
        /// </summary>
        Task BatchDeleteAsync(List<Guid> input);


        /// <summary>
        /// 批量新增ContractDetail
        /// </summary>
        Task BatchCreateAsync(BatchCreateContractDetail input);


        /// <summary>
        /// 获取合同明细选择列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<DropDownDto>> GetDetailSelectAsync(EntityDto<Guid> input);


        /// <summary>
        /// 导出ContractDetail为excel表
        /// </summary>
        /// <returns></returns>
        //Task<FileDto> GetToExcel();

    }
}
