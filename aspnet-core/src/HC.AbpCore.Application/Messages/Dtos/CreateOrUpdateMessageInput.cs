

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Messages;

namespace HC.AbpCore.Messages.Dtos
{
    public class CreateOrUpdateMessageInput
    {
        [Required]
        public MessageEditDto Message { get; set; }

    }
}