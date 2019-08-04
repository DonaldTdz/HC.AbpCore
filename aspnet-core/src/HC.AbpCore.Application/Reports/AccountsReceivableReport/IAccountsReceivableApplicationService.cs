using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HC.AbpCore.Reports.AccountsReceivableReport.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HC.AbpCore.Reports.AccountsReceivableReport
{
    public interface IAccountsReceivableApplicationService : IApplicationService
    {
        /// <summary>
		/// 获取AccountsReceivableListDto的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<AccountsReceivableListDto>> GetAccountsReceivableAsync(GetAccountsReceivableInput input);

        /// <summary>
        /// 获取CustomerReceivablesDetail的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<CustomerReceivablesDetailDto>> GetCustomerReceivablesDetailAsync(GetAccountsReceivableInput input);
    }
}
