
using Abp.Runtime.Validation;
using HC.AbpCore.Dtos;
using HC.AbpCore.DataDictionarys;
using static HC.AbpCore.DataDictionarys.DataDictionaryBase;

namespace HC.AbpCore.DataDictionarys.Dtos
{
    public class GetDataDictionarysInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {

        /// <summary>
        /// 正常化排序使用
        /// </summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }

        /// <summary>
        /// 查询条件分组
        /// </summary>
        public DataDictionaryGroupEnum? Group { get; set; }

        /// <summary>
        /// 查询条件数据字典值
        /// </summary>
        public string Value { get; set; }

    }
}
