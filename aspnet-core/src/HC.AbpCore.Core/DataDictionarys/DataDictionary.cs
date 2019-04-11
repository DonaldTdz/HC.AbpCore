using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static HC.AbpCore.DataDictionarys.DataDictionaryBase;

namespace HC.AbpCore.DataDictionarys
{
    [Table("DataDictionarys")]
    public class DataDictionary : Entity //注意修改主键Id数据类型
    {
        /// <summary>
        /// 数据分组 枚举（如：项目分类 等）事先定义好分类，维护值
        /// </summary>
        public virtual DataDictionaryGroupEnum? Group { get; set; }
        /// <summary>
        /// 数据字典code
        /// </summary>
        [StringLength(50)]
        public virtual string Code { get; set; }
        /// <summary>
        /// 数据字典值
        /// </summary>
        [StringLength(200)]
        public virtual string Value { get; set; }
        /// <summary>
        /// 数据字典描述
        /// </summary>
        [StringLength(200)]
        public virtual string Desc { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public virtual int? Seq { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public virtual DateTime CreationTime { get; set; }
    }

}
