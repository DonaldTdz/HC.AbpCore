
using AutoMapper;
using HC.AbpCore.Projects.ProjectDetails;
using HC.AbpCore.Projects.ProjectDetails.Dtos;

namespace HC.AbpCore.Projects.ProjectDetails.Mapper
{

	/// <summary>
    /// 配置ProjectDetail的AutoMapper
    /// </summary>
	internal static class ProjectDetailMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <ProjectDetail,ProjectDetailListDto>();
            configuration.CreateMap <ProjectDetailListDto,ProjectDetail>();

            configuration.CreateMap <ProjectDetailEditDto,ProjectDetail>();
            configuration.CreateMap <ProjectDetail,ProjectDetailEditDto>();

        }
	}
}
