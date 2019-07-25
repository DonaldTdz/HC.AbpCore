using HC.AbpCore.Reimburses.ReimburseDetails.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HC.AbpCore.Reimburses.Dtos
{
    public class CreateReimburseAndDetailInput
    {
        [Required]
        public ReimburseEditDto Reimburse { get; set; }


        [Required]
        public List<ReimburseDetailEditDto> ReimburseDetails { get; set; }
    }
}
