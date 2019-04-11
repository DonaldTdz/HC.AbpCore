using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.AbpCore.Suppliers
{
    [Table("Suppliers")]
    public class Supplier : FullAuditedEntity //注意修改主键Id数据类型
    {
        /// <summary>
        /// 供应商类型 枚举（暂无） 页面暂不显示
        /// </summary>
        public virtual int? Type { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        [StringLength(50)]
        [Required]
        public virtual string Name { get; set; }
        /// <summary>
        /// 供应商地址
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
        /// 联系人
        /// </summary>
        [StringLength(50)]
        public virtual string Contact { get; set; }
        /// <summary>
        /// 联系人职位
        /// </summary>
        [StringLength(50)]
        public virtual string Position { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>
        [StringLength(20)]
        public virtual string Phone { get; set; }
        /// <summary>
        /// 描述，经营范围
        /// </summary>
        [StringLength(500)]
        public virtual string Desc { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(500)]
        public virtual string Remark { get; set; }
        /// <summary>
        /// 附件（资质，营业执照等）格式：附件名:url,附件名:url
        /// </summary>
        [StringLength(500)]
        public virtual string Attachments { get; set; }
    }

}
