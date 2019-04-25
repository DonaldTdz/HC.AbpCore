
using AutoMapper;
using HC.AbpCore.Invoices.InvoiceDetails;
using HC.AbpCore.Invoices.InvoiceDetails.Dtos;

namespace HC.AbpCore.Invoices.InvoiceDetails.Mapper
{

	/// <summary>
    /// 配置InvoiceDetail的AutoMapper
    /// </summary>
	internal static class InvoiceDetailMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <InvoiceDetail,InvoiceDetailListDto>();
            configuration.CreateMap <InvoiceDetailListDto,InvoiceDetail>();

            configuration.CreateMap <InvoiceDetailEditDto,InvoiceDetail>();
            configuration.CreateMap <InvoiceDetail,InvoiceDetailEditDto>();

        }
	}
}
