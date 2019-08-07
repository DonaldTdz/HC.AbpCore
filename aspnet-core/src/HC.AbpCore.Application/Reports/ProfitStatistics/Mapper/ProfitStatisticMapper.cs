
using AutoMapper;
using HC.AbpCore.Reports.ProfitStatistics;
using HC.AbpCore.Reports.ProfitStatistics.Dtos;

namespace HC.AbpCore.Reports.ProfitStatistics.Mapper
{

	/// <summary>
    /// 配置ProfitStatistic的AutoMapper
    /// </summary>
	internal static class ProfitStatisticMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <ProfitStatistic,ProfitStatisticListDto>();
            configuration.CreateMap <ProfitStatisticListDto,ProfitStatistic>();

        }
	}
}
