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
        /// 保证金
        /// </summary>
        public virtual decimal? Bond { get; set; }
        /// <summary>
        /// 保证金截止时间 （提前2天提醒）页面标注
        /// </summary>
        public virtual DateTime? BondTime { get; set; }
        /// <summary>
        /// 是否缴纳保证金
        /// </summary>
        public virtual bool? IsPayBond { get; set; }
        /// <summary>
        /// 是否完成准备
        /// </summary>
        public virtual bool? IsReady { get; set; }
        /// <summary>
        /// 购买标书开始日期
        /// </summary>
        public virtual DateTime? PurchaseStartDate { get; set; }
        /// <summary>
        /// 购买标书结束日期
        /// </summary>
        public virtual DateTime? PurchaseEndDate { get; set; }
        /// <summary>
        /// 标书购买人
        /// </summary>
        [StringLength(50)]
        public virtual string BidPurchaser { get; set; }
        /// <summary>
        /// 购买资料 多个 逗号分隔
        /// </summary>
        [StringLength(500)]
        public virtual string PurchaseInformation { get; set; }
        /// <summary>
        /// 购买标书负责人
        /// </summary>
        [StringLength(50)]
        public virtual string BuyBidingPerson { get; set; }
        /// <summary>
        /// 招标准备负责人
        /// </summary>
        [StringLength(50)]
        public virtual string PreparationPerson { get; set; }
        /// <summary>
        /// 资质证明
        /// </summary>
        [StringLength(500)]
        public virtual string Qualification { get; set; }
        /// <summary>
        /// 投标文件书写，整理人
        /// </summary>
        [StringLength(50)]
        public virtual string Organizer { get; set; }
        /// <summary>
        /// 投标文件检查、核对
        /// </summary>
        [StringLength(50)]
        public virtual string Inspector { get; set; }
        /// <summary>
        /// 投标文件装订
        /// </summary>
        [StringLength(50)]
        public virtual string Binder { get; set; }
        /// <summary>
        /// 是否中标
        /// </summary>
        public virtual bool IsWinbid { get; set; }
        /// <summary>
        /// 标书附件 多个逗号分隔
        /// </summary>
        [StringLength(500)]
        public virtual string Attachments { get; set; }
        /// <summary>
        /// 标书凭证 多个逗号分隔
        /// </summary>
        [StringLength(500)]
        public virtual string Voucher { get; set; }
    }

}
