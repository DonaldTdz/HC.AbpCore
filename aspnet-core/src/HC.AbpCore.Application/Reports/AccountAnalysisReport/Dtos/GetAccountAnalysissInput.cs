
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.Reports.AccountAnalysisReport;

namespace HC.AbpCore.Reports.AccountAnalysisReport.Dtos
{
    public class GetAccountAnalysissInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
        /// 查询条件-类型
        /// </summary>
        public TypeEnum Type { get; set; }

        /// <summary>
        /// 查询条件-客户/供应商Id
        /// </summary>
        public int? RefId { get; set; }

    }
}
