using Abp.Application.Services;
using HC.AbpCore.Reports.ProjectProfitReport.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HC.AbpCore.Reports.ProjectProfitReport
{
    public interface IProjectProfitReportApplicationService : IApplicationService
    {
        /// <summary>
        /// 获取单个项目利润
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ProjectProfitListDto> GetProjectProfitByIdAsync(GetProjectProfitInput input);
    }
}
