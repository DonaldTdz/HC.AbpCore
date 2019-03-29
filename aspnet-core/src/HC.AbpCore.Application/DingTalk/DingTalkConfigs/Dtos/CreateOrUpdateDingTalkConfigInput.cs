

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.DingTalk.DingTalkConfigs;

namespace HC.AbpCore.DingTalk.DingTalkConfigs.Dtos
{
    public class CreateOrUpdateDingTalkConfigInput
    {
        [Required]
        public DingTalkConfigEditDto DingTalkConfig { get; set; }

    }
}