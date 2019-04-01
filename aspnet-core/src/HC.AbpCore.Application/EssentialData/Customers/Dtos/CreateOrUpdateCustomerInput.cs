

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.EssentialData.Customers;

namespace HC.AbpCore.EssentialData.Customers.Dtos
{
    public class CreateOrUpdateCustomerInput
    {
        [Required]
        public CustomerEditDto Customer { get; set; }

    }
}