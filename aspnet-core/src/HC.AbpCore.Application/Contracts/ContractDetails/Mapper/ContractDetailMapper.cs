
using AutoMapper;
using HC.AbpCore.Contracts.ContractDetails;
using HC.AbpCore.Contracts.ContractDetails.Dtos;

namespace HC.AbpCore.Contracts.ContractDetails.Mapper
{

	/// <summary>
    /// 配置ContractDetail的AutoMapper
    /// </summary>
	internal static class ContractDetailMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <ContractDetail,ContractDetailListDto>();
            configuration.CreateMap <ContractDetailListDto,ContractDetail>();

            configuration.CreateMap <ContractDetailEditDto,ContractDetail>();
            configuration.CreateMap <ContractDetail,ContractDetailEditDto>();

        }
	}
}
