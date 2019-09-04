
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.Reimburses;
using System;

namespace HC.AbpCore.Reimburses.Dtos
{
    public class GetReimbursesInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
        /// 查询条件-所属项目
        /// </summary>
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// 查询条件-所属员工
        /// </summary>
        public string EmployeeId
        {
            get; set;
        }

        /// <summary>
        /// 查询条件-状态
        /// </summary>
        public ReimburseStatusEnum? Status { get; set; }

        /// <summary>
        /// 查询条件-类型
        /// </summary>
        public ReimburseTypeEnum? Type { get; set; }

        /// <summary>
        /// 查询条件-申请日期
        /// </summary>
        public DateTime? SubmitDate { get; set; }

        public DateTime? StartSubmitDate
        {
            get
            {
                if (SubmitDate.HasValue)
                    return new DateTime(SubmitDate.Value.Year, 1, 1);
                else
                    return null;
            }
        }

        public DateTime? EndSubmitDate
        {
            get
            {
                if (SubmitDate.HasValue)
                    return new DateTime(SubmitDate.Value.Year + 1, 1, 1);
                else
                    return null;
            }
        }
    }
}
