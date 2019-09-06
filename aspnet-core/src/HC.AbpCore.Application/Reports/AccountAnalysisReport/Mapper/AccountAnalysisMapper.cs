
using AutoMapper;
using HC.AbpCore.Reports.AccountAnalysisReport;
using HC.AbpCore.Reports.AccountAnalysisReport.Dtos;

namespace HC.AbpCore.Reports.AccountAnalysisReport.Mapper
{

	/// <summary>
    /// 配置AccountAnalysis的AutoMapper
    /// </summary>
	internal static class AccountAnalysisMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <AccountAnalysis,AccountAnalysisListDto>();
            configuration.CreateMap <AccountAnalysisListDto,AccountAnalysis>();

        }
	}
}
