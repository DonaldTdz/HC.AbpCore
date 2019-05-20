using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Modules;
using Abp.Quartz;
using Abp.Quartz.Configuration;
using Abp.Reflection.Extensions;
using Abp.Threading;
using Abp.Threading.BackgroundWorkers;
using HC.AbpCore.Authorization;
using HC.AbpCore.ProjectJobs;
using Quartz;

namespace HC.AbpCore
{
    [DependsOn(
        typeof(AbpCoreCoreModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpQuartzModule))]
    public class AbpCoreApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<AbpCoreAuthorizationProvider>();

            Configuration.Modules.AbpQuartz().Scheduler.JobFactory = new AbpQuartzJobFactory(IocManager);
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(AbpCoreApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddProfiles(thisAssembly)
            );
        }

        public override void PostInitialize()
        {
            IocManager.RegisterIfNot<IJobListener, AbpQuartzJobListener>();

            Configuration.Modules.AbpQuartz().Scheduler.ListenerManager.AddJobListener(IocManager.Resolve<IJobListener>());

            if (Configuration.BackgroundJobs.IsJobExecutionEnabled)
            {
                IocManager.Resolve<IBackgroundWorkerManager>().Add(IocManager.Resolve<IQuartzScheduleJobManager>());
                ConfigureQuartzScheduleJobs();
            }
        }

        private void ConfigureQuartzScheduleJobs()
        {
           var jobManager = IocManager.Resolve<IQuartzScheduleJobManager>();
            AsyncHelper.RunSync(() => jobManager.ScheduleAsync<SendWorkReminderMsgJob>(job =>
              {
                  job.WithIdentity("SendWorkReminderMsgJob", "WorkGroup").WithDescription("A job to send msg.");
              },
              trigger =>
              {
                  trigger//.StartAt(new DateTimeOffset(startTime))
                   .StartNow()//一旦加入scheduler，立即生效
                              .WithCronSchedule("0 0 9 * * ?")//每天上午9点执行
                              .Build();
              }));

            AsyncHelper.RunSync(() => jobManager.ScheduleAsync<MondaySendWorkReminderMsgJob>(job =>
            {
                job.WithIdentity("MondaySendWorkReminderMsgJob", "WorkGroup").WithDescription("A job to send msg.");
            },
           trigger =>
           {
               trigger//.StartAt(new DateTimeOffset(startTime))
                .StartNow()//一旦加入scheduler，立即生效
                           .WithCronSchedule("0 0 9 ? * MON")//每周一上午9点
                           .Build();
           }));
        }
    }
}
