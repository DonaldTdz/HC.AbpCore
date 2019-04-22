using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
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
        /// 项目明细或采购明细Id
        /// </summary>
        public virtual Guid? RefDetailId { get; set; }
        /// <summary>
        /// 交货日期
        /// </summary>
        public virtual DateTime? DeliveryDate { get; set; }
    }

}
