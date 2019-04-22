

using System.Collections.Generic;
using Abp.Application.Services.Dto;
using HC.AbpCore.Contracts.ContractDetails;

namespace HC.AbpCore.Contracts.ContractDetails.Dtos
{
    public class GetContractDetailForEditOutput
    {

        public ContractDetailEditDto ContractDetail { get; set; }

    }
}