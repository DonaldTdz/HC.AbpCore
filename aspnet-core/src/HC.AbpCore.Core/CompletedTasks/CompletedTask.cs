using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using HC.AbpCore.CompletedTasks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.AbpCore.Tasks
{
    [Table("CompletedTasks")]
    public class CompletedTask : Entity<Guid>, IHasCreationTime //注意修改主键Id数据类型
    {
        [Required]
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [StringLength(500)]
        public string Content { get; set; }

        /// <summary>
        /// 是否已完成
        /// </summary>
        public bool? IsCompleted { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        public TaskStatusEnum? Status { get; set; }

        /// <summary>
        /// 关联Id
        /// </summary>
        [StringLength(36)]
        public Guid? RefId { get; set; }

        /// <summary>
        /// 任务负责人
        /// </summary>
        [StringLength(50)]
        public string EmployeeId { get; set; }

        /// <summary>
        /// 截止日期
        /// </summary>
        public DateTime ClosingDate { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [Required]
        public DateTime CreationTime { get; set; }
    }
}
