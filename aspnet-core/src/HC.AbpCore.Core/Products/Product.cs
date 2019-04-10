using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.AbpCore.Products
{
    [Table("Products")]
    public class Product : Entity<int> //注意修改主键Id数据类型
    {
        /// <summary>
        /// 分类 枚举（暂无）
        /// </summary>
        public virtual int? Type { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        [StringLength(200)]
        [Required]
        public virtual string Name { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        [StringLength(200)]
        [Required]
        public virtual string Specification { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        [StringLength(50)]
        public virtual string Unit { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public virtual bool? IsEnabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public virtual DateTime CreationTime { get; set; }
        
    }

}
