using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.AbpCore.Reimburses.ReimburseDetails
{
    [Table("ReimburseDetails")]
    public class ReimburseDetail : Entity<Guid>, IHasCreationTime //注意修改主键Id数据类型
    {
        /// <summary>
        /// 报销Id 外键
        /// </summary>
        [Required]
        public virtual Guid ReimburseId { get; set; }
        /// <summary>
        /// 发生日期
        /// </summary>
        [Required]
        public virtual DateTime HappenDate { get; set; }
        /// <summary>
        /// 数据字典（餐饮费、交通费等）
        /// </summary>
        [StringLength(50)]
        [Required]
        public virtual string Type { get; set; }
        /// <summary>
        /// 发生地点
        /// </summary>
        [StringLength(200)]
        [Required]
        public virtual string Place { get; set; }
        /// <summary>
        /// 客户
        /// </summary>
        [StringLength(200)]
        public virtual string Customer { get; set; }
        /// <summary>
        /// 费用说明
        /// </summary>
        [StringLength(500)]
        public virtual string Desc { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        [Required]
        public virtual decimal Amount { get; set; }
        /// <summary>
        /// 票据张数
        /// </summary>
        public virtual int? InvoiceNum { get; set; }
        /// <summary>
        /// 附件 多个用逗号分隔
        /// </summary>
        [StringLength(500)]
        public virtual string Attachments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public virtual DateTime CreationTime { get; set; }
    }

}
