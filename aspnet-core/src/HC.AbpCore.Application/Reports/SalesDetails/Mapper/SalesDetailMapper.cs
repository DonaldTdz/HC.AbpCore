
using AutoMapper;
using HC.AbpCore.Reports.SalesDetails;
using HC.AbpCore.Reports.SalesDetails.Dtos;

namespace HC.AbpCore.Reports.SalesDetails.Mapper
{

	/// <summary>
    /// 配置SalesDetail的AutoMapper
    /// </summary>
	internal static class SalesDetailMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <SalesDetail,SalesDetailListDto>();
            configuration.CreateMap <SalesDetailListDto,SalesDetail>();

        }
	}
}
