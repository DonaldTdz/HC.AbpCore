
using AutoMapper;
using HC.AbpCore.DingTalk.Organizations;
using HC.AbpCore.DingTalk.Organizations.Dtos;

namespace HC.AbpCore.DingTalk.Organizations.Mapper
{

	/// <summary>
    /// 配置Organization的AutoMapper
    /// </summary>
	internal static class OrganizationMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <Organization,OrganizationListDto>();
            configuration.CreateMap <OrganizationListDto,Organization>();

            configuration.CreateMap <OrganizationEditDto,Organization>();
            configuration.CreateMap <Organization,OrganizationEditDto>();

        }
	}
}
