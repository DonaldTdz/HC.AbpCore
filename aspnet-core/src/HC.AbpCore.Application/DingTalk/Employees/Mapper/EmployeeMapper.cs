
using AutoMapper;
using HC.AbpCore.DingTalk.Employees;
using HC.AbpCore.DingTalk.Employees.Dtos;

namespace HC.AbpCore.DingTalk.Employees.Mapper
{

	/// <summary>
    /// 配置Employee的AutoMapper
    /// </summary>
	internal static class EmployeeMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <Employee,EmployeeListDto>();
            configuration.CreateMap <EmployeeListDto,Employee>();

            configuration.CreateMap <EmployeeEditDto,Employee>();
            configuration.CreateMap <Employee,EmployeeEditDto>();

        }
	}
}
