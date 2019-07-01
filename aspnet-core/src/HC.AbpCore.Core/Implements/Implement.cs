using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.AbpCore.Implements
{
    [Table("Implements")]
    public class Implement : FullAuditedEntity<Guid> //注意修改主键Id数据类型
    {
        /// <summary>
        /// 所属项目 外键
        /// </summary>
        public virtual Guid? ProjectId { get; set; }
        /// <summary>
        /// 执行过程名称
        /// </summary>
        [StringLength(50)]
        public virtual string Name { get; set; }
        /// <summary>
        /// 是否执行
        /// </summary>
        public virtual bool? IsImplement { get; set; }
        /// <summary>
        /// 附件 多个用逗号分隔
        /// </summary>
        [StringLength(500)]
        public virtual string Attachments { get; set; }
    }


}
