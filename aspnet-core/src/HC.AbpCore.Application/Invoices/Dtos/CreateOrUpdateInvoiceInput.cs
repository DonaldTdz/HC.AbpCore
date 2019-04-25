

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Invoices;

namespace HC.AbpCore.Invoices.Dtos
{
    public class CreateOrUpdateInvoiceInput
    {
        [Required]
        public InvoiceEditDto Invoice { get; set; }

    }
}