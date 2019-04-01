
using AutoMapper;
using HC.AbpCore.EssentialData.Customers;
using HC.AbpCore.EssentialData.Customers.Dtos;

namespace HC.AbpCore.EssentialData.Customers.Mapper
{

	/// <summary>
    /// 配置Customer的AutoMapper
    /// </summary>
	internal static class CustomerMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <Customer,CustomerListDto>();
            configuration.CreateMap <CustomerListDto,Customer>();

            configuration.CreateMap <CustomerEditDto,Customer>();
            configuration.CreateMap <Customer,CustomerEditDto>();

        }
	}
}
