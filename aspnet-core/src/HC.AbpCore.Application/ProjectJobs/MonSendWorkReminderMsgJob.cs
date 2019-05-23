using Abp.Dependency;
using Abp.Quartz;
using HC.AbpCore.DingTalk;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HC.AbpCore.ProjectJobs
{
    public class MonSendWorkReminderMsgJob : JobBase, ITransientDependency
    {
        private readonly IDingTalkAppService _dingTalkAppService;

        public MonSendWorkReminderMsgJob(IDingTalkAppService dingTalkAppService)
        {
            _dingTalkAppService = dingTalkAppService;
        }
        public override async Task Execute(IJobExecutionContext context)
        {
            await _dingTalkAppService.MonWorkNotificationMessageAsync();
        }
    }
}
