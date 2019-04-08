
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.Customers;

namespace HC.AbpCore.Customers.Dtos
{
    public class GetCustomersInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
