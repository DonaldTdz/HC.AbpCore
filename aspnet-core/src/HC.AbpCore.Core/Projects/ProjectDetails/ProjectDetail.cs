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
        /// 明细分类 枚举（硬件采购、软件开发、运营服务、其它 数据字典维护）硬件采购从产品表选 或添加到产品表
        /// </summary>
        [StringLength(50)]
        public virtual string Type { get; set; }
        /// <summary>
        /// 商品或服务名称
        /// </summary>
        [StringLength(200)]
        public virtual string Name { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        [StringLength(200)]
        public virtual string Specification { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        [StringLength(50)]
        public virtual string Unit { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public virtual decimal? Num { get; set; }
        /// <summary>
        /// 销售单价
        /// </summary>
        public virtual decimal? Price { get; set; }
        /// <summary>
        /// 外键 可选择已有产品Id
        /// </summary>
        public virtual int? ProductId { get; set; }
    }

}
