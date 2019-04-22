using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static HC.AbpCore.Contracts.ContractEnum;

namespace HC.AbpCore.Contracts
{
    [Table("Contracts")]
    public class Contract : FullAuditedEntity<Guid> //注意修改主键Id数据类型
    {
        /// <summary>
        /// 合同分类 枚举（1 销项 2 进项）
        /// </summary>
        [Required]
        public ContractTypeEnum Type { get; set; }
        /// <summary>
        /// 合同编号 按一定规则生成 也可以手动填写
        /// </summary>
        [StringLength(50)]
        [Required]
        public virtual string ContractCode { get; set; }
        /// <summary>
        /// 项目合同 为 项目Id  采购合同为采购单Id
        /// </summary>
        public virtual Guid? RefId { get; set; }
        /// <summary>
        /// 签订日期
        /// </summary>
        public virtual DateTime? SignatureTime { get; set; }
        /// <summary>
        /// 合同金额（合同明细添加项目明细计算而得）
        /// </summary>
        public virtual decimal? Amount { get; set; }
        /// <summary>
        /// 合同描述
        /// </summary>
        [StringLength(500)]
        public virtual string Desc { get; set; }
        /// <summary>
        /// 合同附件 多个 逗号分隔
        /// </summary>
        [StringLength(500)]
        public virtual string Attachments { get; set; }
    }

}
