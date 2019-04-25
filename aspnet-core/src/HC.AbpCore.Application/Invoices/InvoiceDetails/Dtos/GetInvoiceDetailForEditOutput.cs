

using System.Collections.Generic;
using Abp.Application.Services.Dto;
using HC.AbpCore.Invoices.InvoiceDetails;

namespace HC.AbpCore.Invoices.InvoiceDetails.Dtos
{
    public class GetInvoiceDetailForEditOutput
    {

        public InvoiceDetailEditDto InvoiceDetail { get; set; }

    }
}