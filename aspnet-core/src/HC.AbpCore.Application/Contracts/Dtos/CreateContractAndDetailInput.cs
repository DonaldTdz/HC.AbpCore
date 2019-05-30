using HC.AbpCore.Contracts.ContractDetails.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HC.AbpCore.Contracts.Dtos
{
    public class CreateContractAndDetailInput
    {
        /// <summary>
        /// h合同
        /// </summary>
        [Required]
        public ContractEditDto Contract { get; set; }

        /// <summary>
        /// 项目详情
        /// </summary>
        public List<ContractDetailEditDto> ContractDetails { get; set; }
    }
}
