
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.Contracts.ContractDetails;

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

    }
}
