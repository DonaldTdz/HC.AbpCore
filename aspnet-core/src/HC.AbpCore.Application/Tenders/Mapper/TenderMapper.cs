
using AutoMapper;
using HC.AbpCore.Tenders;
using HC.AbpCore.Tenders.Dtos;

namespace HC.AbpCore.Tenders.Mapper
{

	/// <summary>
    /// 配置Tender的AutoMapper
    /// </summary>
	internal static class TenderMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <Tender,TenderListDto>();
            configuration.CreateMap <TenderListDto,Tender>();

            configuration.CreateMap <TenderEditDto,Tender>();
            configuration.CreateMap <Tender,TenderEditDto>();

        }
	}
}
