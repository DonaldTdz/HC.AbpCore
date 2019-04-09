

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Companys.Accounts;

namespace HC.AbpCore.Companys.Accounts.Dtos
{
    public class CreateOrUpdateAccountInput
    {
        [Required]
        public AccountEditDto Account { get; set; }

    }
}