
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.DingTalk.DingTalkConfigs;

namespace HC.AbpCore.DingTalk.DingTalkConfigs.Dtos
{
    public class GetDingTalkConfigsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
