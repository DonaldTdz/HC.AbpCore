
using AutoMapper;
using HC.AbpCore.Implements;
using HC.AbpCore.Implements.Dtos;

namespace HC.AbpCore.Implements.Mapper
{

	/// <summary>
    /// 配置Implement的AutoMapper
    /// </summary>
	internal static class ImplementMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <Implement,ImplementListDto>();
            configuration.CreateMap <ImplementListDto,Implement>();

            configuration.CreateMap <ImplementEditDto,Implement>();
            configuration.CreateMap <Implement,ImplementEditDto>();

        }
	}
}
