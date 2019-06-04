

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Tasks;

namespace HC.AbpCore.Tasks.Dtos
{
    public class CreateOrUpdateCompletedTaskInput
    {
        [Required]
        public CompletedTaskEditDto CompletedTask { get; set; }

        public string bond { get; set; }

        public bool? isWinbid { get; set; }

        public Guid? messageId { get; set; }
    }
}