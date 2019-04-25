
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.Invoices.InvoiceDetails;

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

    }
}
