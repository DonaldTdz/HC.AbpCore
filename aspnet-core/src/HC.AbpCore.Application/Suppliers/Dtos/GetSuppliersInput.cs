
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

        /// <summary>
        /// 查询条件-供应商名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
       /// 查询条件-供应商ID
       /// </summary>
        public int Id { get; set; }

    }
}
