
using AutoMapper;
using HC.AbpCore.AdvancePayments.AdvancePaymentDetails;
using HC.AbpCore.AdvancePayments.AdvancePaymentDetails.Dtos;

namespace HC.AbpCore.AdvancePayments.AdvancePaymentDetails.Mapper
{

	/// <summary>
    /// 配置AdvancePaymentDetail的AutoMapper
    /// </summary>
	internal static class AdvancePaymentDetailMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <AdvancePaymentDetail,AdvancePaymentDetailListDto>();
            configuration.CreateMap <AdvancePaymentDetailListDto,AdvancePaymentDetail>();

            configuration.CreateMap <AdvancePaymentDetailEditDto,AdvancePaymentDetail>();
            configuration.CreateMap <AdvancePaymentDetail,AdvancePaymentDetailEditDto>();

        }
	}
}
