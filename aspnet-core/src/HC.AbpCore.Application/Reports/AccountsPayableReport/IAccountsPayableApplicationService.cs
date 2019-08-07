using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HC.AbpCore.Reports.AccountsPayableReport.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HC.AbpCore.Reports.AccountsPayableReport
{
    public interface IAccountsPayableApplicationService : IApplicationService
    {
        /// <summary>
        /// 获取AccountsPayable的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<AccountsPayableListDto>> GetAccountsPayableAsync(GetAccountsPayableInput input);

        /// <summary>
        /// 获取SupplierPayableDetail的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<AccountsPayableDetailDto>> GetSupplierPayableDetailAsync(GetAccountsPayableInput input);
    }
}
