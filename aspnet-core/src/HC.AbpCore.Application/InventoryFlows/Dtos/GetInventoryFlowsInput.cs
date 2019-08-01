
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.InventoryFlows;

namespace HC.AbpCore.InventoryFlows.Dtos
{
    public class GetInventoryFlowsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
