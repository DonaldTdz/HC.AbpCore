

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Implements;

namespace HC.AbpCore.Implements.Dtos
{
    public class CreateOrUpdateImplementInput
    {
        [Required]
        public ImplementEditDto Implement { get; set; }

    }
}