

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HC.AbpCore.Wechat.Messages.Dtos
{
    public class CreateOrUpdateWechatMessageInput
    {
        [Required]
        public WechatMessageEditDto WechatMessage { get; set; }

    }
}