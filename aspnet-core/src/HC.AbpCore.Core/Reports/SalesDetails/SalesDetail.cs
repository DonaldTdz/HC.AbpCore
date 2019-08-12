using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.AbpCore.Reports.SalesDetails
{
    [Table("SalesDetails")]
    public class SalesDetail : Entity<Guid>, IHasCreationTime //注意修改主键Id数据类型
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public virtual string projectName { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        public virtual string projectCode { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public virtual string customerName { get; set; }

        /// <summary>
        /// 签定日期
        /// </summary>
        public virtual DateTime? signatureTime { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public virtual string employeeName { get; set; }

        /// <summary>
        /// 项目金额
        /// </summary>
        public virtual decimal? contractAmount { get; set; }

        /// <summary>
        /// 收款条款
        /// </summary>
        public virtual string paymentCondition { get; set; }

        /// <summary>
        /// 已收金额
        /// </summary>
        public virtual decimal? acceptedAmount { get; set; }

        /// <summary>
        /// 未收金额
        /// </summary>
        public virtual decimal? uncollectedAmount { get; set; }

        /// <summary>
        /// 收款日期
        /// </summary>
        public virtual DateTime? paymentTime { get; set; }

        /// <summary>
        /// 发票开具
        /// </summary>
        public virtual int? invoiceCount { get; set; }

        /// <summary>
        /// 利润
        /// </summary>
        public virtual decimal? Profit { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public virtual DateTime CreationTime { get; set; }
    }
}
