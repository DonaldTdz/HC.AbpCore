

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.TimeSheets;

namespace HC.AbpCore.TimeSheets.Dtos
{
    public class CreateOrUpdateTimeSheetInput
    {
        [Required]
        public TimeSheetEditDto TimeSheet { get; set; }

        public Guid? messageId { get; set; }
    }
}