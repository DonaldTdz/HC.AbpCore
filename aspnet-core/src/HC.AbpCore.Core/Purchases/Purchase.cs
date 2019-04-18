using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.AbpCore.Purchases
{
    [Table("Purchases")]
    public class Purchase : FullAuditedEntity<Guid> //注意修改主键Id数据类型
    {
        /// <summary>
        /// 采购编号 按一定规则生成 也可以手动修改填写
        /// </summary>
        [StringLength(50)]
        public virtual string Code { get; set; }
        /// <summary>
        /// 项目Id 外键
        /// </summary>
        public virtual Guid? ProjectId { get; set; }
        /// <summary>
        /// 采购负责人
        /// </summary>
        [StringLength(50)]
        public virtual string EmployeeId { get; set; }
        /// <summary>
        /// 采购日期
        /// </summary>
        public virtual DateTime? PurchaseDate { get; set; }
        /// <summary>
        /// 采购描述
        /// </summary>
        [StringLength(500)]
        public virtual string Desc { get; set; }
    }

}
