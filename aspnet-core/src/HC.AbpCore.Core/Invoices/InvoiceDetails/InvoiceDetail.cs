using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.AbpCore.Invoices.InvoiceDetails
{
    [Table("InvoiceDetails")]
    public class InvoiceDetail : FullAuditedEntity<Guid> //注意修改主键Id数据类型
    {
        /// <summary>
        /// 发票Id 外键
        /// </summary>
        public virtual Guid? InvoiceId { get; set; }
        /// <summary>
        /// 项目明细或采购明细Id
        /// </summary>
        public virtual Guid? RefId { get; set; }
        /// <summary>
        /// 劳务或服务名称
        /// </summary>
        [StringLength(50)]
        [Required]
        public virtual string Name { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        [StringLength(200)]
        public virtual string Specification { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        [StringLength(50)]
        public virtual string Unit { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public virtual decimal? Num { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public virtual decimal? Price { get; set; }
        /// <summary>
        /// 税率 数据字典配置，如：16%
        /// </summary>
        [StringLength(50)]
        public virtual string TaxRate { get; set; }
    }

}
