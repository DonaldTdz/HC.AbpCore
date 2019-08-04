using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.AbpCore.Reports.AccountsReceivableReport.Dtos
{
    public class AccountsReceivableListDto: FullAuditedEntityDto
    {
        /// <summary>
        /// 客户名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 应收款合计
        /// </summary>
        public decimal? TotalAmount { get; set; }
    }

    public class CustomerReceivablesDetailDto: FullAuditedEntityDto<Guid?>
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 项目金额
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// 已收金额
        /// </summary>
        public string AcceptedAmount { get; set; }

        /// <summary>
        /// 未收金额
        /// </summary>
        public string UncollectedAmount { get; set; }

        /// <summary>
        /// 预计付款日期
        /// </summary>
        public DateTime? ExpectedPaymentDate { get; set; }
    }
}
