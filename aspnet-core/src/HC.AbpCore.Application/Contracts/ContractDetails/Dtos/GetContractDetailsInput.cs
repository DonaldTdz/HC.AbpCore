
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.Contracts.ContractDetails;
using System;

namespace HC.AbpCore.Contracts.ContractDetails.Dtos
{
    public class GetContractDetailsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {

        /// <summary>
        /// 正常化排序使用
        /// </summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }

        /// <summary>
        /// 查询条件-合同ID
        /// </summary>
        public Guid? ContractId { get; set; }

    }
}
