
using AutoMapper;
using HC.AbpCore.Contracts;
using HC.AbpCore.Contracts.Dtos;

namespace HC.AbpCore.Contracts.Mapper
{

	/// <summary>
    /// 配置Contract的AutoMapper
    /// </summary>
	internal static class ContractMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <Contract,ContractListDto>();
            configuration.CreateMap <ContractListDto,Contract>();

            configuration.CreateMap <ContractEditDto,Contract>();
            configuration.CreateMap <Contract,ContractEditDto>();

        }
	}
}
