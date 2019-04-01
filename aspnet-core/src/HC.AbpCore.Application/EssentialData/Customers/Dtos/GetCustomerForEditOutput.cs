

using System.Collections.Generic;
using Abp.Application.Services.Dto;
using HC.AbpCore.EssentialData.Customers;

namespace HC.AbpCore.EssentialData.Customers.Dtos
{
    public class GetCustomerForEditOutput
    {

        public CustomerEditDto Customer { get; set; }

    }
}