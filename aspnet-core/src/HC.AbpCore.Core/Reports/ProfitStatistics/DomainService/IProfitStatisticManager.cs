

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.Reports.ProfitStatistics;


namespace HC.AbpCore.Reports.ProfitStatistics.DomainService
{
    public interface IProfitStatisticManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitProfitStatistic();



		 
      
         

    }
}
