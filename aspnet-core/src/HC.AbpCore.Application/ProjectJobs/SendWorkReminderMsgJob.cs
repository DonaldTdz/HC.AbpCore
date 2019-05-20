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
    public class SendWorkReminderMsgJob:JobBase, ITransientDependency
    {
        private readonly IDingTalkAppService _dingTalkAppService;

        public SendWorkReminderMsgJob(IDingTalkAppService dingTalkAppService)
        {
            _dingTalkAppService = dingTalkAppService;
        }

        public override async Task Execute(IJobExecutionContext context)
        {
            await _dingTalkAppService.AutoWorkNotificationMessageAsync();
        }
    }

}
