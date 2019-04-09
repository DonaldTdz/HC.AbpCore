using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.AbpCore.Companys.Accounts
{
    [Table("Accounts")]
    public class Account : Entity<long> //注意修改主键Id数据类型
    {
        /// <summary>
        /// 公司账户Id
        /// </summary>
        [Required]
        public virtual int CompanyId { get; set; }
        /// <summary>
        /// 类型 枚举(1 入账，2 出账)
        /// </summary>
        [Required]
        public virtual AccountType Type { get; set; }
        /// <summary>
        /// 期初余额
        /// </summary>
        public virtual decimal? Initial { get; set; }
        /// <summary>
        /// 发生金额 入账为正 出账为负
        /// </summary>
        public virtual decimal? Amount { get; set; }
        /// <summary>
        /// 期末余额
        /// </summary>
        public virtual decimal? Ending { get; set; }
        /// <summary>
        /// 描述，可以预先设置如：发工资，项目收款等
        /// </summary>
        [StringLength(500)]
        public virtual string Desc { get; set; }
        /// <summary>
        /// 引用Id，如回款计划Id
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
