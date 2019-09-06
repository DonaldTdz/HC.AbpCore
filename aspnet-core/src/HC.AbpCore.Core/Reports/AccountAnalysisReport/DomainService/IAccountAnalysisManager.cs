

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.Reports.AccountAnalysisReport;


namespace HC.AbpCore.Reports.AccountAnalysisReport.DomainService
{
    public interface IAccountAnalysisManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitAccountAnalysis();




        Task<ResultAccountAnalysis> GetAccountAnalysesAsync(TypeEnum type, int? refId);
         

    }
}
