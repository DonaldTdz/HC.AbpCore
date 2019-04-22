
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
        Task<PagedResultDto<ContractListDto>> GetPaged(GetContractsInput input);


		/// <summary>
		/// 通过指定id获取ContractListDto信息
		/// </summary>
		Task<ContractListDto> GetById(EntityDto<Guid> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetContractForEditOutput> GetForEdit(NullableIdDto<Guid> input);


        /// <summary>
        /// 添加或者修改Contract的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateContractInput input);


        /// <summary>
        /// 删除Contract信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);


        /// <summary>
        /// 批量删除Contract
        /// </summary>
        Task BatchDelete(List<Guid> input);


		/// <summary>
        /// 导出Contract为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetToExcel();

    }
}
