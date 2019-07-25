
using AutoMapper;
using HC.AbpCore.AdvancePayments;
using HC.AbpCore.AdvancePayments.Dtos;

namespace HC.AbpCore.AdvancePayments.Mapper
{

	/// <summary>
    /// 配置AdvancePayment的AutoMapper
    /// </summary>
	internal static class AdvancePaymentMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <AdvancePayment,AdvancePaymentListDto>();
            configuration.CreateMap <AdvancePaymentListDto,AdvancePayment>();

            configuration.CreateMap <AdvancePaymentEditDto,AdvancePayment>();
            configuration.CreateMap <AdvancePayment,AdvancePaymentEditDto>();

        }
	}
}
