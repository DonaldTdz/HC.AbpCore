using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.AbpCore.Messages
{
    [Table("Messages")]
    public class Message : Entity<Guid> //注意修改主键Id数据类型
    {
        /// <summary>
        /// 接收用户
        /// </summary>
        [Required]
        public virtual Guid EmployeeId { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public virtual DateTime? SendTime { get; set; }
        /// <summary>
        /// 发送内容
        /// </summary>
        [StringLength(500)]
        public virtual string Content { get; set; }
        /// <summary>
        /// 消息分类 枚举（0 暂无 以后扩展）
        /// </summary>
        public virtual int? Type { get; set; }
        /// <summary>
        /// 是否已读
        /// </summary>
        public virtual bool? IsRead { get; set; }
        /// <summary>
        /// 已读时间
        /// </summary>
        public virtual DateTime? ReadTime { get; set; }
    }

}
