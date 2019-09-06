

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Reports.AccountAnalysisReport;
using System.Collections.Generic;

namespace HC.AbpCore.Reports.AccountAnalysisReport.Dtos
{
    public class AccountAnalysisListDto : EntityDto<int?>
    {

        
		/// <summary>
		/// Name
		/// </summary>
		public string Name { get; set; }



		/// <summary>
		/// Remarks
		/// </summary>
		public string Remarks { get; set; }



        /// <summary>
        /// Amount
        /// </summary>
        public List<decimal?> Amount { get; set; }




    }

    public class ResultAccountAnalysisDto
    {
        public List<AccountAnalysis> AccountAnalyses { get; set; }
        public List<string> PlanTimes { get; set; }
    }
}