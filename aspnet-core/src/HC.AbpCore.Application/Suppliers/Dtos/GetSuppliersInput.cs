
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.Suppliers;

namespace HC.AbpCore.Suppliers.Dtos
{
    public class GetSuppliersInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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


        public string Name { get; set; }

    }
}
