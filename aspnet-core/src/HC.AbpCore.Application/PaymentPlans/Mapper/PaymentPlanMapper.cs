
using AutoMapper;
using HC.AbpCore.PaymentPlans;
using HC.AbpCore.PaymentPlans.Dtos;

namespace HC.AbpCore.PaymentPlans.Mapper
{

	/// <summary>
    /// 配置PaymentPlan的AutoMapper
    /// </summary>
	internal static class PaymentPlanMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <PaymentPlan,PaymentPlanListDto>();
            configuration.CreateMap <PaymentPlanListDto,PaymentPlan>();

            configuration.CreateMap <PaymentPlanEditDto,PaymentPlan>();
            configuration.CreateMap <PaymentPlan,PaymentPlanEditDto>();

        }
	}
}
