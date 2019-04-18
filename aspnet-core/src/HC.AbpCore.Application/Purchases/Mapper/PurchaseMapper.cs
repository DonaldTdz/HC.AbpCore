
using AutoMapper;
using HC.AbpCore.Purchases;
using HC.AbpCore.Purchases.Dtos;

namespace HC.AbpCore.Purchases.Mapper
{

	/// <summary>
    /// 配置Purchase的AutoMapper
    /// </summary>
	internal static class PurchaseMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <Purchase,PurchaseListDto>();
            configuration.CreateMap <PurchaseListDto,Purchase>();

            configuration.CreateMap <PurchaseEditDto,Purchase>();
            configuration.CreateMap <Purchase,PurchaseEditDto>();

        }
	}
}
