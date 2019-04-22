
using AutoMapper;
using HC.AbpCore.Purchases.PurchaseDetails;
using HC.AbpCore.Purchases.PurchaseDetails.Dtos;

namespace HC.AbpCore.Purchases.PurchaseDetails.Mapper
{

	/// <summary>
    /// 配置PurchaseDetail的AutoMapper
    /// </summary>
	internal static class PurchaseDetailMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <PurchaseDetail,PurchaseDetailListDto>();
            configuration.CreateMap <PurchaseDetailListDto,PurchaseDetail>();

            configuration.CreateMap <PurchaseDetailEditDto,PurchaseDetail>();
            configuration.CreateMap <PurchaseDetail,PurchaseDetailEditDto>();

        }
	}
}
