using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.AbpCore.Reports.AccountsReceivableReport.Dtos
{
    public class GetAccountsReceivableInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
        /// 查询条件-客户Id
        /// </summary>
        public int? CustomerId { get; set; }
    }
}
