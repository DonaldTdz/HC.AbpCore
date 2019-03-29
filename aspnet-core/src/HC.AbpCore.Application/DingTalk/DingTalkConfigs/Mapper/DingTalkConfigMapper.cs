
using AutoMapper;
using HC.AbpCore.DingTalk.DingTalkConfigs;
using HC.AbpCore.DingTalk.DingTalkConfigs.Dtos;

namespace HC.AbpCore.DingTalk.DingTalkConfigs.Mapper
{

	/// <summary>
    /// 配置DingTalkConfig的AutoMapper
    /// </summary>
	internal static class DingTalkConfigMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <DingTalkConfig,DingTalkConfigListDto>();
            configuration.CreateMap <DingTalkConfigListDto,DingTalkConfig>();

            configuration.CreateMap <DingTalkConfigEditDto,DingTalkConfig>();
            configuration.CreateMap <DingTalkConfig,DingTalkConfigEditDto>();

        }
	}
}
