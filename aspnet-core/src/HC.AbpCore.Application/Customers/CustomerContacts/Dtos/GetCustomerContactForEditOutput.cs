

using System.Collections.Generic;
using Abp.Application.Services.Dto;
using HC.AbpCore.Customers.CustomerContacts;

namespace HC.AbpCore.Customers.CustomerContacts.Dtos
{
    public class GetCustomerContactForEditOutput
    {

        public CustomerContactEditDto CustomerContact { get; set; }

    }
}