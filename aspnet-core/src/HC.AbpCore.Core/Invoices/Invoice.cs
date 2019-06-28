using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.AbpCore.Invoices
{
    [Table("Invoices")]
    public class Invoice : FullAuditedEntity<Guid> //注意修改主键Id数据类型
    {
        /// <summary>
        /// 发票分类 枚举（1 销项、2 进项）销项标书销售合同发票， 进项表示采购发票
        /// </summary>
        [Required]
        public virtual InvoiceTypeEnum Type { get; set; }
        /// <summary>
        /// 发票号
        /// </summary>
        [StringLength(200)]
        [Required]
        public virtual string Code { get; set; }
        /// <summary>
        /// 发票金额
        /// </summary>
        public virtual decimal? Amount { get; set; }
        /// <summary>
        /// 开票日期
        /// </summary>
        public virtual DateTime? SubmitDate { get; set; }
        /// <summary>
        /// 引用Id （项目Id，采购Id）
        /// </summary>
        public virtual Guid? RefId { get; set; }
    }

}
