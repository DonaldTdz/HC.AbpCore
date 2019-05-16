using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.AbpCore.Reimburses
{
    [Table("Reimburses")]
    public class Reimburse : Entity<Guid>, IHasCreationTime //注意修改主键Id数据类型
    {
        /// <summary>
        /// 所属项目 选择当前未完成项目
        /// </summary>
        public virtual Guid? ProjectId { get; set; }
        /// <summary>
        /// 报销人 及部门 外键
        /// </summary>
        [StringLength(50)]
        public virtual string EmployeeId { get; set; }
        /// <summary>
        /// 报销总金额 报销明细计算
        /// </summary>
        public virtual decimal? Amount { get; set; }
        /// <summary>
        /// 状态枚举（1 提交 2 审批通过 3 取消）
        /// </summary>
        public virtual ReimburseStatusEnum? Status { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        public virtual DateTime? SubmitDate { get; set; }
        /// <summary>
        /// 审批人
        /// </summary>
        [StringLength(50)]
        public virtual string ApproverId { get; set; }
        /// <summary>
        /// 审批时间
        /// </summary>
        public virtual DateTime? ApprovalTime { get; set; }
        /// <summary>
        /// 审批实例Id
        /// </summary>
        public virtual string ProcessInstanceId { get; set; }
        /// <summary>
        /// 取消时间
        /// </summary>
        public virtual DateTime? CancelTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public virtual DateTime CreationTime { get; set; }

    }

}
