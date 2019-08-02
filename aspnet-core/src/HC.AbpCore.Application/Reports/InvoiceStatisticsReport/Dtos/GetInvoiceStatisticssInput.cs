
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.Invoices;
using HC.AbpCore.Reports.InvoiceStatisticsReport;
using System;

namespace HC.AbpCore.Reports.InvoiceStatisticsReport.Dtos
{
    public class GetInvoiceStatisticssInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
        /// 查询条件-开票日期
        /// </summary>
        public DateTime SubmitDate { get; set; }

        /// <summary>
        /// 查询条件-发票类型
        /// </summary>
        public InvoiceTypeEnum? Type { get; set; }

        public DateTime? StartSubmitDate
        {
            get { return new DateTime(SubmitDate.Year, 1, 1); }
        }

        public DateTime? EndSubmitDate
        {
            get
            {
                return new DateTime(SubmitDate.Year+1, 1, 1);
            }
        }

    }
}
