using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.AbpCore.TimeSheets
{
    [Table("TimeSheets")]
    public class TimeSheet : Entity<Guid>, IHasCreationTime //注意修改主键Id数据类型
    {
        /// <summary>
        /// 所属项目 外键
        /// </summary>
        [Required]
        public virtual Guid ProjectId { get; set; }
        /// <summary>
        /// 工作日期
        /// </summary>
        [Required]
        public virtual DateTime WorkeDate { get; set; }
        /// <summary>
        /// 员工Id
        /// </summary>
        [StringLength(50)]
        [Required]
        public virtual string EmployeeId { get; set; }
        /// <summary>
        /// 工时
        /// </summary>
        public virtual int? Hour { get; set; }
        /// <summary>
        /// 工作内容
        /// </summary>
        [StringLength(50)]
        public virtual string Content { get; set; }
        /// <summary>
        /// 状态枚举（1 提交 2 审批通过）
        /// </summary>
        public virtual TimeSheetStatusEnum? Status { get; set; }
        /// <summary>
        /// 审批人Id
        /// </summary>
        [StringLength(50)]
        public virtual string ApproverId { get; set; }
        /// <summary>
        /// 审批时间
        /// </summary>
        public virtual DateTime? ApprovalTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        public virtual DateTime CreationTime { get; set; }
    }

}
