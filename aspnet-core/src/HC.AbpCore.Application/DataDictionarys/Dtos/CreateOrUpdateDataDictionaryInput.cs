

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.DataDictionarys;

namespace HC.AbpCore.DataDictionarys.Dtos
{
    public class CreateOrUpdateDataDictionaryInput
    {
        [Required]
        public DataDictionaryEditDto DataDictionary { get; set; }

    }
}