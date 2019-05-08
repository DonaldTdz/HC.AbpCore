

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Reimburses;

namespace HC.AbpCore.Reimburses.Dtos
{
    public class CreateOrUpdateReimburseInput
    {
        [Required]
        public ReimburseEditDto Reimburse { get; set; }

    }
}