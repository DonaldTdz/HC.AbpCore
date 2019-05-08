
using AutoMapper;
using HC.AbpCore.TimeSheets;
using HC.AbpCore.TimeSheets.Dtos;

namespace HC.AbpCore.TimeSheets.Mapper
{

	/// <summary>
    /// 配置TimeSheet的AutoMapper
    /// </summary>
	internal static class TimeSheetMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <TimeSheet,TimeSheetListDto>();
            configuration.CreateMap <TimeSheetListDto,TimeSheet>();

            configuration.CreateMap <TimeSheetEditDto,TimeSheet>();
            configuration.CreateMap <TimeSheet,TimeSheetEditDto>();

        }
	}
}
