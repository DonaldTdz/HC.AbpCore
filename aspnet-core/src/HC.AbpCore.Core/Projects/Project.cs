using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static HC.AbpCore.Projects.ProjectBase;

namespace HC.AbpCore.Projects
{
    [Table("Projects")]
    public class Project : FullAuditedEntity<Guid> //注意修改主键Id数据类型
    {
        /// <summary>
        /// 项目模式 枚举（1 内部、2 合伙、 3 外部）
        /// </summary>
        [Required]
        public virtual ProjectMode? Mode { get; set; }
        /// <summary>
        /// 收益比例（合伙项目填写）
        /// </summary>
        public virtual decimal? ProfitRatio { get; set; }
        /// <summary>
        /// 过单费用
        /// </summary>
        public virtual decimal? BillCost { get; set; }
        /// <summary>
        /// 项目分类 数据字典
        /// </summary>
        [StringLength(50)]
        public virtual string Type { get; set; }
        /// <summary>
        /// 项目编号 按一定规则生成
        /// </summary>
        [StringLength(50)]
        public virtual string ProjectCode { get; set; }
        /// <summary>
        /// 项目销售
        /// </summary>
        [StringLength(50)]
        public virtual string ProjectSalesId { get; set; }
        /// <summary>
        /// 销售助理
        /// </summary>
        [StringLength(50)]
        public virtual string SalesAssistantId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [StringLength(200)]
        public virtual string Name { get; set; }
        /// <summary>
        /// 所属客户（外键）
        /// </summary>
        public virtual int? CustomerId { get; set; }
        /// <summary>
        /// 所属客户联系人
        /// </summary>
        [StringLength(50)]
        public virtual string CustomerContact { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public virtual DateTime? StartDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public virtual DateTime? EndDate { get; set; }
        /// <summary>
        /// 预算金额
        /// </summary>
        public virtual decimal? Budget { get; set; }
        /// <summary>
        /// 执行金额
        /// </summary>
        public virtual decimal? ImplementMoney { get; set; }
        /// <summary>
        /// 项目状态 枚举（1 线索、2 立项、3、进行中、4、已完成、5 已回款、0 取消）
        /// </summary>
        public virtual ProjectStatus? Status { get; set; }
        /// <summary>
        /// 项目描述
        /// </summary>
        [StringLength(500)]
        public virtual string Desc { get; set; }
    }

}
