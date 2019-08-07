using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.AbpCore.AdvancePayments.AdvancePaymentDetails
{
    [Table("AdvancePaymentDetails")]
    public class AdvancePaymentDetail : Entity<Guid>, IHasCreationTime //注意修改主键Id数据类型
    {

        /// <summary>
        /// 预付款计划Id
        /// </summary>
        [Required]
        public virtual Guid AdvancePaymentId { get; set; }
        /// <summary>
        /// 采购明细外键Id
        /// </summary>
        public virtual Guid? PurchaseDetailId { get; set; }
        /// <summary>
        /// 付款比例   %
        /// </summary>
        public virtual int? Ratio { get; set; }
        /// <summary>
        /// 付款金额
        /// </summary>
        public virtual decimal? Amount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public virtual DateTime CreationTime { get; set; }
    }

}
