

using System.Collections.Generic;
using Abp.Application.Services.Dto;
using HC.AbpCore.Invoices;

namespace HC.AbpCore.Invoices.Dtos
{
    public class GetInvoiceForEditOutput
    {

        public InvoiceEditDto Invoice { get; set; }

    }
}