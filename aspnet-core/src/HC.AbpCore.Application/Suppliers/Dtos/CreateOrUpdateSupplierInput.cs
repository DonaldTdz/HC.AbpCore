

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Suppliers;

namespace HC.AbpCore.Suppliers.Dtos
{
    public class CreateOrUpdateSupplierInput
    {
        [Required]
        public SupplierEditDto Supplier { get; set; }

    }
}