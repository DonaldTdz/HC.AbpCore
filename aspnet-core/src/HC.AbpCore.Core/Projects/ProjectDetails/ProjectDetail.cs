using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.AbpCore.Projects.ProjectDetails
{
    [Table("ProjectDetails")]
    public class ProjectDetail : FullAuditedEntity<Guid> //注意修改主键Id数据类型
    {
        /// <summary>
        /// 项目Id 外键
        /// </summary>
        public virtual Guid? ProjectId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(200)]
        public virtual string Name { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public virtual decimal? Num { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public virtual decimal? Price { get; set; }
    }

}
