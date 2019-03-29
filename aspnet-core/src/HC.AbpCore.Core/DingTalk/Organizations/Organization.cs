using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.AbpCore.DingTalk.Organizations
{
    [Table("Organizations")]
    public class Organization : Entity<long> //注意修改主键Id数据类型
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        [StringLength(100)]
        [Required]
        public virtual string DepartmentName { get; set; }
        /// <summary>
        /// 父部门Id（外键）
        /// </summary>
        public virtual long? ParentId { get; set; }
        /// <summary>
        /// 排序，之前批量获取不到，可以看一下接口当前是否可以获取
        /// </summary>
        public virtual long? Order { get; set; }
        /// <summary>
        /// 钉钉组织架构同步自动
        /// </summary>
        public virtual bool? DeptHiding { get; set; }
        /// <summary>
        /// 钉钉组织架构同步自动
        /// </summary>
        [StringLength(100)]
        public virtual string OrgDeptOwner { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public virtual DateTime CreationTime { get; set; }
    }

}
