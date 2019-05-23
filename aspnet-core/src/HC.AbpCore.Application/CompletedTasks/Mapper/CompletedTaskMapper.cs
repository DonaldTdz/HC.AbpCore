
using AutoMapper;
using HC.AbpCore.Tasks;
using HC.AbpCore.Tasks.Dtos;

namespace HC.AbpCore.Tasks.Mapper
{

	/// <summary>
    /// 配置CompletedTask的AutoMapper
    /// </summary>
	internal static class CompletedTaskMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <CompletedTask,CompletedTaskListDto>();
            configuration.CreateMap <CompletedTaskListDto,CompletedTask>();

            configuration.CreateMap <CompletedTaskEditDto,CompletedTask>();
            configuration.CreateMap <CompletedTask,CompletedTaskEditDto>();

        }
	}
}
