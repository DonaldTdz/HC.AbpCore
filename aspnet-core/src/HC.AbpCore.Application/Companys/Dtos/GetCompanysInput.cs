
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.Companys;

namespace HC.AbpCore.Companys.Dtos
{
    public class GetCompanysInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
