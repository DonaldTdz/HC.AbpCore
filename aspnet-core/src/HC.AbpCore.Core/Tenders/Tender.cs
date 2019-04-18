using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.AbpCore.Tenders
{
    [Table("Tenders")]
    public class Tender : FullAuditedEntity<Guid> //注意修改主键Id数据类型
    {
        /// <summary>
        /// 项目Id 外键
        /// </summary>
        [Required]
        public virtual Guid ProjectId { get; set; }
        /// <summary>
        /// 招标时间
        /// </summary>
        public virtual DateTime? TenderTime { get; set; }
        /// <summary>
        /// 保证金截止时间 （提前2天提醒）页面标注
        /// </summary>
        public virtual DateTime? BondTime { get; set; }
        /// <summary>
        /// 准备完成时间 （完成时间提醒 比如提前4提醒）页面标注
        /// </summary>
        public virtual DateTime? ReadyTime { get; set; }
        /// <summary>
        /// 招标负责人，准备完成时间提醒
        /// </summary>
        [StringLength(500)]
        public virtual string EmployeeId { get; set; }
        /// <summary>
        /// 标书准备人 多个
        /// </summary>
        [StringLength(500)]
        public virtual string ReadyEmployeeIds { get; set; }
        /// <summary>
        /// 是否中标
        /// </summary>
        public virtual bool? IsWinbid { get; set; }
        /// <summary>
        /// 标书附件 多个 逗号分隔
        /// </summary>
        [StringLength(500)]
        public virtual string Attachments { get; set; }
    }

}
