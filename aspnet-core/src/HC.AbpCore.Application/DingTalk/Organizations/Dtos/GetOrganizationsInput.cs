
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.DingTalk.Organizations;

namespace HC.AbpCore.DingTalk.Organizations.Dtos
{
    public class GetOrganizationsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
