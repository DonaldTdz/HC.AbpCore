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
        /// 产品Id外键 
        /// </summary>
        public virtual int? ProductId { get; set; }
        /// <summary>
        /// 采购数量
        /// </summary>
        public virtual int? Num { get; set; }
    }

    public class PurchaseDetailNew
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }


        /// <summary>
        /// PurchaseId
        /// </summary>
        public Guid? PurchaseId { get; set; }



        /// <summary>
        /// SupplierId
        /// </summary>
        public int? SupplierId { get; set; }



        /// <summary>
        /// ProductId
        /// </summary>
        public int? ProductId { get; set; }



        /// <summary>
        /// Num
        /// </summary>
        public int? Num { get; set; }



        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }



        /// <summary>
        /// Specification
        /// </summary>
        public string Specification { get; set; }



        /// <summary>
        /// TaxRate
        /// </summary>
        public string TaxRate { get; set; }



        /// <summary>
        /// Price
        /// </summary>
        public decimal Price { get; set; }
    }

}
