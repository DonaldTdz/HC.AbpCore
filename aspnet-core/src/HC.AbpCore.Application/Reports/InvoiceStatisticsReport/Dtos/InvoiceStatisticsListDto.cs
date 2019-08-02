

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Reports.InvoiceStatisticsReport;
using HC.AbpCore.Invoices;

namespace HC.AbpCore.Reports.InvoiceStatisticsReport.Dtos
{
    public class InvoiceStatisticsListDto  
    {
        /// <summary>
        /// 发票明细Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 发票分类 枚举（1 销项、2 进项）销项标书销售合同发票， 进项表示采购发票
        /// </summary>
        [Required]
        public virtual InvoiceTypeEnum Type { get; set; }
        /// <summary>
        /// 发票号
        /// </summary>
        [StringLength(200)]
        [Required]
        public virtual string Code { get; set; }
        /// <summary>
        /// 开票日期
        /// </summary>
        public virtual DateTime? SubmitDate { get; set; }
        /// <summary>
        /// 引用Id （项目Id，采购Id）
        /// </summary>
        public virtual Guid? RefId { get; set; }
        /// <summary>
        /// 发票Id 外键
        /// </summary>
        public virtual Guid? InvoiceId { get; set; }
        /// <summary>
        /// 项目明细或采购明细Id
        /// </summary>
        public virtual Guid? DetailRefId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
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
        /// 单价
        /// </summary>
        public virtual decimal? Price { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public virtual decimal? Amount { get; set; }
        /// <summary>
        /// 税率 数据字典配置，如：16%
        /// </summary>
        [StringLength(50)]
        public virtual string TaxRate { get; set; }
        /// <summary>
        /// 税额
        /// </summary>
        public virtual decimal? TaxAmount { get; set; }
        /// <summary>
        /// 合计
        /// </summary>
        public virtual decimal? TotalAmount { get; set; }
        /// <summary>
        /// 开票单位或对方单位
        /// </summary>
        public virtual string InvoiceUnit { get; set; }


    }
}