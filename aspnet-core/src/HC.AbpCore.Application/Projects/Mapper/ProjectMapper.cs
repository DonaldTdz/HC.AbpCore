
using AutoMapper;
using HC.AbpCore.Projects;
using HC.AbpCore.Projects.Dtos;

namespace HC.AbpCore.Projects.Mapper
{

	/// <summary>
    /// 配置Project的AutoMapper
    /// </summary>
	internal static class ProjectMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <Project,ProjectListDto>();
            configuration.CreateMap <ProjectListDto,Project>();

            configuration.CreateMap <ProjectEditDto,Project>();
            configuration.CreateMap <Project,ProjectEditDto>();

        }
	}
}
