using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.AbpCore.Reports.AccountsPayableReport.Dtos
{
    public class GetAccountsPayableInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
        /// 查询条件-供应商Id
        /// </summary>
        public int? SupplierId { get; set; }

        /// <summary>
        /// 查询条件-产品Id
        /// </summary>
        public int? ProductId { get; set; }
    }
}
