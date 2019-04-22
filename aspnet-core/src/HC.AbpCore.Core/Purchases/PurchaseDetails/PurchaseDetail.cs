using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.AbpCore.Purchases.PurchaseDetails
{
    [Table("PurchaseDetails")]
    public class PurchaseDetail : FullAuditedEntity<Guid> //注意修改主键Id数据类型
    {
        /// <summary>
        /// 采购Id 外键
        /// </summary>
        public virtual Guid? PurchaseId { get; set; }
        /// <summary>
        /// 供应商Id 外键
        /// </summary>
        public virtual int? SupplierId { get; set; }
        /// <summary>
        /// 采购单价
        /// </summary>
        public virtual decimal? Price { get; set; }
        /// <summary>
        /// 项目明细Id 外键
        /// </summary>
        public virtual Guid? ProjectDetailId { get; set; }
    }

}
