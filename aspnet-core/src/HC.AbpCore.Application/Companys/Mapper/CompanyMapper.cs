
using AutoMapper;
using HC.AbpCore.Companys;
using HC.AbpCore.Companys.Dtos;

namespace HC.AbpCore.Companys.Mapper
{

	/// <summary>
    /// 配置Company的AutoMapper
    /// </summary>
	internal static class CompanyMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <Company,CompanyListDto>();
            configuration.CreateMap <CompanyListDto,Company>();

            configuration.CreateMap <CompanyEditDto,Company>();
            configuration.CreateMap <Company,CompanyEditDto>();

        }
	}
}
