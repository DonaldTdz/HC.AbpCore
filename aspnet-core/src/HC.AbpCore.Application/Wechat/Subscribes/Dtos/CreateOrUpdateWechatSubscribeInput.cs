

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HC.AbpCore.Wechat.Subscribes.Dtos
{
    public class CreateOrUpdateWechatSubscribeInput
    {
        [Required]
        public WechatSubscribeEditDto WechatSubscribe { get; set; }

    }
}