
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.Contracts;

namespace HC.AbpCore.Contracts.Dtos
{
    public class GetContractsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
