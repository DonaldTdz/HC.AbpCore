
using AutoMapper;
using HC.AbpCore.Reimburses.ReimburseDetails;
using HC.AbpCore.Reimburses.ReimburseDetails.Dtos;

namespace HC.AbpCore.Reimburses.ReimburseDetails.Mapper
{

	/// <summary>
    /// 配置ReimburseDetail的AutoMapper
    /// </summary>
	internal static class ReimburseDetailMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <ReimburseDetail,ReimburseDetailListDto>();
            configuration.CreateMap <ReimburseDetailListDto,ReimburseDetail>();

            configuration.CreateMap <ReimburseDetailEditDto,ReimburseDetail>();
            configuration.CreateMap <ReimburseDetail,ReimburseDetailEditDto>();

        }
	}
}
