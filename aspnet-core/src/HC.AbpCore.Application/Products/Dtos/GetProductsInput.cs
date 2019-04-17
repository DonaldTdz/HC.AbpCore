
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.Products;

namespace HC.AbpCore.Products.Dtos
{
    public class GetProductsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
        /// 查询条件产品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 查询条件分类
        /// </summary>
        public int? Type { get; set; }

        /// <summary>
        /// 查询条件状态
        /// </summary>
        public bool? IsEnabled { get; set; }

    }
}
