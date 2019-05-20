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
    public class MondaySendWorkReminderMsgJob : JobBase, ITransientDependency
    {
        private readonly IDingTalkAppService _dingTalkAppService;

        public MondaySendWorkReminderMsgJob(IDingTalkAppService dingTalkAppService)
        {
            _dingTalkAppService = dingTalkAppService;
        }
        public override Task Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
