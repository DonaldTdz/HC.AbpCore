﻿using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static HC.AbpCore.Customers.CustomerBase;

namespace HC.AbpCore.Customers
{
    [Table("Customers")]
    public class Customer : FullAuditedEntity //注意修改主键Id数据类型
    {
        /// <summary>
        /// 客户名称
        /// </summary>
        [StringLength(100)]
        [Required]
        public virtual string Name { get; set; }
        /// <summary>
        /// 枚举（企业、个人、其它）
        /// </summary>
        public virtual CustomerEnum Type { get; set; }
        /// <summary>
        /// 客户地址
        /// </summary>
        [StringLength(500)]
        public virtual string Address { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        [StringLength(20)]
        public virtual string ZipCode { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [StringLength(20)]
        public virtual string Tel { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(500)]
        public virtual string Remark { get; set; }

        public virtual string typeName
        {
            get
            {
                return Type.ToString();
            }
        }

    }

}
