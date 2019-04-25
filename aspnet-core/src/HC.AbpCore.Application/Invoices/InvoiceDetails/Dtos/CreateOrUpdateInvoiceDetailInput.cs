

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Invoices.InvoiceDetails;

namespace HC.AbpCore.Invoices.InvoiceDetails.Dtos
{
    public class CreateOrUpdateInvoiceDetailInput
    {
        [Required]
        public InvoiceDetailEditDto InvoiceDetail { get; set; }

    }
}