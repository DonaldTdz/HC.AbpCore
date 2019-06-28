using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HC.AbpCore.Contracts.ContractDetails.Dtos
{
    public class BatchCreateContractDetail
    {
        [Required]
        public List<ContractDetailEditDto> ContractDetails { get; set; }

        public Guid ContractId { get; set; }
    }
}
