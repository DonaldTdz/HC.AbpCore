
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.Reports.ProfitStatistics;
using System;

namespace HC.AbpCore.Reports.ProfitStatistics.Dtos
{
    public class GetProfitStatisticsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
        /// 查询条件-项目Id
        /// </summary>
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// 查询条件-创建日期
        /// </summary>
        public DateTime CreationTime { get; set; }

        public DateTime? StartCreationTime
        {
            get { return new DateTime(CreationTime.Year, 1, 1); }
        }

        public DateTime? EndCreationTime
        {
            get
            {
                return new DateTime(CreationTime.Year + 1, 1, 1);
            }
        }

    }
}
