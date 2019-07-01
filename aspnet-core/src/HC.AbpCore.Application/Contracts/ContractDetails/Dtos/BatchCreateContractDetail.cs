using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HC.AbpCore.Contracts.ContractDetails.Dtos
{
    public class BatchCreateContractDetail
    {
        public List<ContractDetailEditDto> ContractDetails { get; set; }

        [Required]
        public Guid ContractId { get; set; }
    }
}
