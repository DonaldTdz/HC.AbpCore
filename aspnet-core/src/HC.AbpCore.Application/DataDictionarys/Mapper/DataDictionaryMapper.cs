
using AutoMapper;
using HC.AbpCore.DataDictionarys;
using HC.AbpCore.DataDictionarys.Dtos;

namespace HC.AbpCore.DataDictionarys.Mapper
{

	/// <summary>
    /// 配置DataDictionary的AutoMapper
    /// </summary>
	internal static class DataDictionaryMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <DataDictionary,DataDictionaryListDto>();
            configuration.CreateMap <DataDictionaryListDto,DataDictionary>();

            configuration.CreateMap <DataDictionaryEditDto,DataDictionary>();
            configuration.CreateMap <DataDictionary,DataDictionaryEditDto>();

        }
	}
}
