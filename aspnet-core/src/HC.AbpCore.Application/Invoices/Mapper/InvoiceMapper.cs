
using AutoMapper;
using HC.AbpCore.Invoices;
using HC.AbpCore.Invoices.Dtos;

namespace HC.AbpCore.Invoices.Mapper
{

	/// <summary>
    /// 配置Invoice的AutoMapper
    /// </summary>
	internal static class InvoiceMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <Invoice,InvoiceListDto>();
            configuration.CreateMap <InvoiceListDto,Invoice>();

            configuration.CreateMap <InvoiceEditDto,Invoice>();
            configuration.CreateMap <Invoice,InvoiceEditDto>();

        }
	}
}
