

using System.Collections.Generic;
using Abp.Application.Services.Dto;
using HC.AbpCore.DingTalk.DingTalkConfigs;

namespace HC.AbpCore.DingTalk.DingTalkConfigs.Dtos
{
    public class GetDingTalkConfigForEditOutput
    {

        public DingTalkConfigEditDto DingTalkConfig { get; set; }

    }
}