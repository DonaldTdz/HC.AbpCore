
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.Invoices.InvoiceDetails;
using System;

namespace HC.AbpCore.Invoices.InvoiceDetails.Dtos
{
    public class GetInvoiceDetailsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
        /// 查询条件-发票ID
        /// </summary>
        public Guid? InvoiceId { get; set; }

        /// <summary>
        /// 查询条件-发票类型
        /// </summary>
        public InvoiceTypeEnum? Type { get; set; }

    }
}
