using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HC.AbpCore.DingTalk
{
    public interface IDingTalkAppService: IApplicationService
    {
        /// <summary>
        /// 工作消息通知
        /// </summary>
        /// <returns></returns>
        Task AutoWorkNotificationMessageAsync();
    }
}
