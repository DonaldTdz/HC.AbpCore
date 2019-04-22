

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Contracts;

namespace HC.AbpCore.Contracts.Dtos
{
    public class CreateOrUpdateContractInput
    {
        [Required]
        public ContractEditDto Contract { get; set; }

    }
}