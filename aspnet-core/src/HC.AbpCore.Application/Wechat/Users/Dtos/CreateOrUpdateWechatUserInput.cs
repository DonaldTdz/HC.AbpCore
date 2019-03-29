

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HC.AbpCore.Wechat.Users.Dtos
{
    public class CreateOrUpdateWechatUserInput
    {
        [Required]
        public WechatUserEditDto WechatUser { get; set; }

    }
}