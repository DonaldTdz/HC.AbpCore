using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.AbpCore.Reports.ProfitStatistics
{
    [Table("ProfitStatistics")]
    public class ProfitStatistic : Entity<Guid>, IHasCreationTime //注意修改主键Id数据类型
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 项目金额
        /// </summary>
        public virtual decimal? ContractAmount { get; set; }

        /// <summary>
        /// 报销金额
        /// </summary>
        public virtual decimal? ReimburseAmount { get; set; }

        /// <summary>
        /// 工时成本金额
        /// </summary>
        public virtual decimal? TimesheetAmount { get; set; }

        /// <summary>
        /// 商品成本
        /// </summary>
        public virtual decimal? CommodityCostAmount { get; set; }

        /// <summary>
        /// 销项税额
        /// </summary>
        public virtual decimal? SaleTaxAmount { get; set; }

        /// <summary>
        /// 进项税额
        /// </summary>
        public virtual decimal? IncomeTaxAmount { get; set; }

        /// <summary>
        /// 商品成本总条数
        /// </summary>
        public virtual int CostCount { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public virtual DateTime CreationTime { get; set; }
    }
}
