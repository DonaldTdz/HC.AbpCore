using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HC.AbpCore.DingTalk
{
    public interface IDingTalkManager : IDomainService
    {
        Task<string> GetAccessTokenByAppAsync(DingDingAppEnum app);

        Task<DingDingAppConfig> GetDingDingConfigByAppAsync(DingDingAppEnum app);

        string GetUserId(string accessToken, string code);
    }
}
