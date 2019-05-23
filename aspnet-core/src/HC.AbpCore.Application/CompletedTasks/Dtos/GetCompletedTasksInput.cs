
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.Tasks;

namespace HC.AbpCore.Tasks.Dtos
{
    public class GetCompletedTasksInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
        /// 查询条件-所属员工
        /// </summary>
        public string EmployeeId
        {
            get; set;
        }

    }
}
