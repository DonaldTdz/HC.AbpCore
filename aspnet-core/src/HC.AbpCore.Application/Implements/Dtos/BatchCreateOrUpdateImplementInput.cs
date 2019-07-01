using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HC.AbpCore.Implements.Dtos
{
    public class BatchCreateOrUpdateImplementInput
    {
        [Required]
        public List<ImplementEditDto> Implements { get; set; }
    }
}
