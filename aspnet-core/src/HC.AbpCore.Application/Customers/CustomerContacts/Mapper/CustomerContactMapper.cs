
using AutoMapper;
using HC.AbpCore.Customers.CustomerContacts;
using HC.AbpCore.Customers.CustomerContacts.Dtos;

namespace HC.AbpCore.Customers.CustomerContacts.Mapper
{

	/// <summary>
    /// 配置CustomerContact的AutoMapper
    /// </summary>
	internal static class CustomerContactMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <CustomerContact,CustomerContactListDto>();
            configuration.CreateMap <CustomerContactListDto,CustomerContact>();

            configuration.CreateMap <CustomerContactEditDto,CustomerContact>();
            configuration.CreateMap <CustomerContact,CustomerContactEditDto>();

        }
	}
}
