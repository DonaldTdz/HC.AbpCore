
using AutoMapper;
using HC.AbpCore.Reimburses;
using HC.AbpCore.Reimburses.Dtos;

namespace HC.AbpCore.Reimburses.Mapper
{

	/// <summary>
    /// 配置Reimburse的AutoMapper
    /// </summary>
	internal static class ReimburseMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <Reimburse,ReimburseListDto>();
            configuration.CreateMap <ReimburseListDto,Reimburse>();

            configuration.CreateMap <ReimburseEditDto,Reimburse>();
            configuration.CreateMap <Reimburse,ReimburseEditDto>();

        }
	}
}
