
using AutoMapper;
using HC.AbpCore.InventoryFlows;
using HC.AbpCore.InventoryFlows.Dtos;

namespace HC.AbpCore.InventoryFlows.Mapper
{

	/// <summary>
    /// 配置InventoryFlow的AutoMapper
    /// </summary>
	internal static class InventoryFlowMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <InventoryFlow,InventoryFlowListDto>();
            configuration.CreateMap <InventoryFlowListDto,InventoryFlow>();

            configuration.CreateMap <InventoryFlowEditDto,InventoryFlow>();
            configuration.CreateMap <InventoryFlow,InventoryFlowEditDto>();

        }
	}
}
