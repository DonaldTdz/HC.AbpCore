
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.Projects.ProjectDetails;

namespace HC.AbpCore.Projects.ProjectDetails.Dtos
{
    public class GetProjectDetailsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
