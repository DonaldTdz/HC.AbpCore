using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.AbpCore.InventoryFlows
{
    [Table("InventoryFlows")]
    public class InventoryFlow : Entity<long>, IHasCreationTime //注意修改主键Id数据类型
    {
        /// <summary>
        /// 库存产品Id
        /// </summary>
        [Required]
        public virtual int ProductId { get; set; }
        /// <summary>
        /// 类型 枚举(1 入库，2 出库)
        /// </summary>
        [Required]
        public virtual InventoryFlowType Type { get; set; }
        /// <summary>
        /// 期初库存
        /// </summary>
        public virtual int? Initial { get; set; }
        /// <summary>
        /// 发生库存 入库为正 出库为负
        /// </summary>
        public virtual int? StreamNumber { get; set; }
        /// <summary>
        /// 期末库存
        /// </summary>
        public virtual int? Ending { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(500)]
        public virtual string Desc { get; set; }
        /// <summary>
        /// 引用Id，如采购id
        /// </summary>
        [StringLength(100)]
        public virtual string RefId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public virtual DateTime CreationTime { get; set; }
    }

}
