using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.AbpCore.DingTalk.Employees
{
    [Table("Employees")]
    public class Employee : Entity<string> //注意修改主键Id数据类型
    {
        /// <summary>
        /// 钉钉unionid详细查看钉钉
        /// </summary>
        [StringLength(200)]
        public virtual string Unionid { get; set; }
        /// <summary>
        /// 钉钉name
        /// </summary>
        [StringLength(50)]
        public virtual string Name { get; set; }
        /// <summary>
        /// 钉钉mobile
        /// </summary>
        [StringLength(11)]
        public virtual string Mobile { get; set; }
        /// <summary>
        /// 钉钉email
        /// </summary>
        [StringLength(100)]
        public virtual string Email { get; set; }
        /// <summary>
        /// 钉钉active
        /// </summary>
        public virtual bool? Active { get; set; }
        /// <summary>
        /// 钉钉department，部门Id，多部门用逗号分隔, 原来是用[]分隔
        /// </summary>
        [StringLength(300)]
        public virtual string Department { get; set; }
        /// <summary>
        /// 钉钉isLeaderInDepts，格式{key：部门Id，val: true/false 是/否}
        /// </summary>
        [StringLength(300)]
        public virtual string IsLeaderInDepts { get; set; }
        /// <summary>
        /// 钉钉position
        /// </summary>
        [StringLength(100)]
        public virtual string Position { get; set; }
        /// <summary>
        /// 钉钉avatar
        /// </summary>
        [StringLength(200)]
        public virtual string Avatar { get; set; }
        /// <summary>
        /// 钉钉hiredDate
        /// </summary>
        public virtual DateTime? HiredDate { get; set; }
        /// <summary>
        /// 钉钉jobnumber
        /// </summary>
        [StringLength(100)]
        public virtual string JobNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public virtual DateTime CreationTime { get; set; }
    }

}
