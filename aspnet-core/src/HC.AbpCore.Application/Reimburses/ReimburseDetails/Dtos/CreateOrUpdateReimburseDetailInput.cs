

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Reimburses.ReimburseDetails;

namespace HC.AbpCore.Reimburses.ReimburseDetails.Dtos
{
    public class CreateOrUpdateReimburseDetailInput
    {
        [Required]
        public ReimburseDetailEditDto ReimburseDetail { get; set; }

    }
}