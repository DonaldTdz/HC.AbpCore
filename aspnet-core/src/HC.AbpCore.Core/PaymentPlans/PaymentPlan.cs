using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.AbpCore.PaymentPlans
{
    [Table("PaymentPlans")]
    public class PaymentPlan : Entity<Guid>, IHasCreationTime //注意修改主键Id数据类型
    {
        /// <summary>
        /// 所属项目 外键
        /// </summary>
        [Required]
        public virtual Guid ProjectId { get; set; }
        /// <summary>
        /// 计划时间，提前进行回款提醒（提前量与代姐确认）提醒财务配置人员数据字典配置
        /// </summary>
        [Required]
        public virtual DateTime PlanTime { get; set; }
        /// <summary>
        /// 回款比率  5
        /// </summary>
        [Required]
        public virtual int Ratio { get; set; }
        /// <summary>
        /// 回款条件
        /// </summary>
        [StringLength(500)]
        public virtual string PaymentCondition { get; set; }
        /// <summary>
        /// 回款金额
        /// </summary>
        public virtual decimal? Amount { get; set; }
        /// <summary>
        /// 回款状态 枚举（1、未回款 2、已回款）已回款需要更新公司账户
        /// </summary>
        public virtual PaymentPlanStatusEnum? Status { get; set; }
        /// <summary>
        /// 回款时间
        /// </summary>
        public virtual DateTime? PaymentTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        public virtual DateTime CreationTime { get; set; }
    }

}
