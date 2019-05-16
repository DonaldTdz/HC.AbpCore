
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.Messages;

namespace HC.AbpCore.Messages.Dtos
{
    public class GetMessagesInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
