using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HC.AbpCore.DingTalk
{

    /// <summary>
    /// DingTalk应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class DingTalkAppService : AbpCoreAppServiceBase, IDingTalkAppService
    {
        private readonly IDingTalkManager _dingTalkManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public DingTalkAppService(
        IDingTalkManager dingTalkManager
        )
        {
            _dingTalkManager = dingTalkManager;
        }


        /// <summary>
        /// 工作消息通知
        /// </summary>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [Audited]
        public async Task AutoWorkNotificationMessageAsync()
        {
            await _dingTalkManager.AutoWorkNotificationMessageAsync();
        }
    }
}

