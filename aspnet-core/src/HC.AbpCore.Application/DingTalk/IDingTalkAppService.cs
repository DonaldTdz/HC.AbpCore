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
        /// 工作消息通知  每天早上9点提醒
        /// </summary>
        /// <returns></returns>
        Task AutoWorkNotificationMessageAsync();

        /// <summary>
        /// 工作消息通知  周一早上9点提醒
        /// </summary>
        /// <returns></returns>
        Task MonWorkNotificationMessageAsync();
    }
}
