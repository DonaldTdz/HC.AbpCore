
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


using HC.AbpCore.Contracts.Dtos;
using HC.AbpCore.Contracts;
using HC.AbpCore.Dtos;
using static HC.AbpCore.Contracts.ContractEnum;

namespace HC.AbpCore.Contracts
{
    /// <summary>
    /// Contract应用层服务的接口方法
    ///</summary>
    public interface IContractAppService : IApplicationService
    {
        /// <summary>
		/// 获取Contract的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ContractListDto>> GetPagedAsync(GetContractsInput input);


		/// <summary>
		/// 通过指定id获取ContractListDto信息
		/// </summary>
		Task<ContractListDto> GetByIdAsync(EntityDto<Guid> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetContractForEditOutput> GetForEditAsync(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改Contract的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<APIResultDto> CreateOrUpdateAsync(CreateOrUpdateContractInput input);


        /// <summary>
        /// 添加Contract以及ContractDetail的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<APIResultDto> CreateContractAndDetailAsync(CreateContractAndDetailInput input);


        /// <summary>
        /// 删除Contract信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAsync(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除Contract
        /// </summary>
        Task BatchDeleteAsync(List<Guid> input);


        /// <summary>
        /// 根据编号分类获取自动生成的合同编号
        /// </summary>
        /// <param name="CodeType"></param>
        /// <returns></returns>
        Task<string> GetContractCodeAsync(CodeTypeEnum CodeType);

        /// <summary>
        /// 导出Contract为excel表
        /// </summary>
        /// <returns></returns>
        //Task<FileDto> GetToExcel();

    }
}
