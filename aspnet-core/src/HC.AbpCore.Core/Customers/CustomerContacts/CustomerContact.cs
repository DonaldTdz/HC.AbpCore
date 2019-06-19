using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.AbpCore.Customers.CustomerContacts
{
    [Table("CustomerContacts")]
    public class CustomerContact : FullAuditedEntity //注意修改主键Id数据类型
    {
        /// <summary>
        /// 客户Id外键
        /// </summary>
        public virtual int? CustomerId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(200)]
        public virtual string Name { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        [StringLength(100)]
        public virtual string DeptName { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        [StringLength(50)]
        public virtual string Position { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>
        [StringLength(50)]
        public virtual string Phone { get; set; }
    }

}
