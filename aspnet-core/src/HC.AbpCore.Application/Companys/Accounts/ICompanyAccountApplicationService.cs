
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


using HC.AbpCore.Companys.Accounts.Dtos;
using HC.AbpCore.Companys.Accounts;

namespace HC.AbpCore.Companys.Accounts
{
    /// <summary>
    /// Account应用层服务的接口方法
    ///</summary>
    public interface ICompanyAccountApplicationService : IApplicationService
    {
        /// <summary>
		/// 获取Account的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<AccountListDto>> GetPagedAsync(GetAccountsInput input);


		/// <summary>
		/// 通过指定id获取AccountListDto信息
		/// </summary>
		Task<AccountListDto> GetByIdAsync(EntityDto<long> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetAccountForEditOutput> GetForEditAsync(NullableIdDto<long> input);


        /// <summary>
        /// 添加或者修改Account的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateAsync(CreateOrUpdateAccountInput input);


        /// <summary>
        /// 删除Account信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAsync(EntityDto<long> input);


        /// <summary>
        /// 批量删除Account
        /// </summary>
        Task BatchDeleteAsync(List<long> input);


		/// <summary>
        /// 导出Account为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetToExcel();

    }
}
