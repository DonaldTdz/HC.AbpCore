using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.AbpCore.Reports.AccountAnalysisReport
{
    public class AccountAnalysis : Entity<int?>
    {
        public string Name { get; set; }

        public List<decimal> Amount { get; set; }

        public string Remarks { get; set; }
    }

    public class Detail
    {
        public decimal? Amount { get; set; }
    }

    public class ResultAccountAnalysis
    {
        public List<AccountAnalysis> AccountAnalyses { get; set; }
        public List<string> PlanTimes { get; set; }
    }
}
