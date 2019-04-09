using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.AbpCore.Companys
{
    [Table("Companys")]
    public class Company : Entity<int> //注意修改主键Id数据类型
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        [StringLength(50)]
        public virtual string Name { get; set; }
        /// <summary>
        /// 公司开户银行
        /// </summary>
        [StringLength(50)]
        public virtual string Bank { get; set; }
        /// <summary>
        /// 银行账号
        /// </summary>
        [StringLength(50)]
        public virtual string BankAccount { get; set; }
        /// <summary>
        /// 税号
        /// </summary>
        [StringLength(50)]
        public virtual string DutyNo { get; set; }
        /// <summary>
        /// 开户地址
        /// </summary>
        [StringLength(500)]
        public virtual string Address { get; set; }
        /// <summary>
        /// 公司电话
        /// </summary>
        [StringLength(20)]
        public virtual string Tel { get; set; }
        /// <summary>
        /// 账户余额
        /// </summary>
        public virtual decimal? Balance { get; set; }
        /// <summary>
        /// 附件（资质，营业执照等）格式：附件名:url,附件名:url
        /// </summary>
        [StringLength(500)]
        public virtual string Attachments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public virtual DateTime CreationTime { get; set; }
    }

}
