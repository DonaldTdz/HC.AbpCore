

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Customers;

namespace HC.AbpCore.Customers.Dtos
{
    public class CreateOrUpdateCustomerInput
    {
        public CustomerEditDto Customer { get; set; }

    }
}