

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Customers.CustomerContacts;

namespace HC.AbpCore.Customers.CustomerContacts.Dtos
{
    public class CreateOrUpdateCustomerContactInput
    {
        [Required]
        public CustomerContactEditDto CustomerContact { get; set; }

    }
}