
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.Invoices;
using System;

namespace HC.AbpCore.Invoices.Dtos
{
    public class GetInvoicesInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
        /// 查询条件-发票分类
        /// </summary>
        public InvoiceTypeEnum? Type { get; set; }

        /// <summary>
        /// 查询条件发票号
        /// </summary>
        public string Code { get; set; }


        /// <summary>
        /// 查询条件refId
        /// </summary>
        public Guid? RefId { get; set; }

        /// <summary>
        /// 查询条件-发票抬头
        /// </summary>
        public string Title { get; set; }

    }
}
