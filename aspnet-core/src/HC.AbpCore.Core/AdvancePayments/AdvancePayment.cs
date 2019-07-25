using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.AbpCore.AdvancePayments
{
    [Table("AdvancePayments")]
    public class AdvancePayment : Entity<Guid>, IHasCreationTime //注意修改主键Id数据类型
    {
        /// <summary>
        /// 所属采购 外键
        /// </summary>
        [Required]
        public virtual Guid PurchaseId { get; set; }
        /// <summary>
        /// 计划时间，提前进行付款提醒（提前量与代姐确认）提醒财务配置人员数据字典配置
        /// </summary>
        [Required]
        public virtual DateTime PlanTime { get; set; }
        /// <summary>
        /// 付款描述
        /// </summary>
        [StringLength(500)]
        public virtual string Desc { get; set; }
        /// <summary>
        /// 付款比例   %
        /// </summary>
        public virtual int? Ratio { get; set; }
        /// <summary>
        /// 付款金额
        /// </summary>
        public virtual decimal? Amount { get; set; }
        /// <summary>
        /// 付款状态 枚举（0、未付款 1、已付款）已付款需要更新公司账户
        /// </summary>
        public virtual AdvancePaymentStatusEnum? Status { get; set; }
        /// <summary>
        /// 付款时间
        /// </summary>
        public virtual DateTime? PaymentTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime CreationTime { get; set; }
    }

}
