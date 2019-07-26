using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.AbpCore.Contracts.ContractDetails
{
    [Table("ContractDetails")]
    public class ContractDetail : FullAuditedEntity<Guid> //注意修改主键Id数据类型
    {
        /// <summary>
        /// 合同Id 外键
        /// </summary>
        public virtual Guid? ContractId { get; set; }
        /// <summary>
        /// 产品Id
        /// </summary>
        [Required]
        public int ProductId { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        [StringLength(100)]
        public virtual string Name { get; set; }
        /// <summary>
        /// 型号
        /// </summary>
        [StringLength(100)]
        public virtual string Model { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public virtual int? Num { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public virtual decimal? Price { get; set; }
    }

}
