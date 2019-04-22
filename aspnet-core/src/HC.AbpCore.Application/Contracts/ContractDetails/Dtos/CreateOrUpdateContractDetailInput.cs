

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Contracts.ContractDetails;

namespace HC.AbpCore.Contracts.ContractDetails.Dtos
{
    public class CreateOrUpdateContractDetailInput
    {
        [Required]
        public ContractDetailEditDto ContractDetail { get; set; }

    }
}