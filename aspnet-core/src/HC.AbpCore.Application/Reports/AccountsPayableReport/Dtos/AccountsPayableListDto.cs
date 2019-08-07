using System;
using System.Collections.Generic;
using System.Text;

namespace HC.AbpCore.Reports.AccountsPayableReport.Dtos
{
    public class AccountsPayableListDto
    {
        /// <summary>
        /// 供应商Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 应付款款合计
        /// </summary>
        public decimal? TotalAmount { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreationTime { get; set; }
    }

    public class AccountsPayableDetailDto
    {

        /// <summary>
        /// 产品Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 项目金额
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// 已付金额
        /// </summary>
        public decimal? AcceptedAmount { get; set; }

        /// <summary>
        /// 未付金额
        /// </summary>
        public decimal? UncollectedAmount { get; set; }

        /// <summary>
        /// 预计付款日期
        /// </summary>
        public DateTime? ExpectedPaymentDate { get; set; }
    }
}
