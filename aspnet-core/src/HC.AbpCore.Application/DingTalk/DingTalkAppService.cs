using Abp.Application.Services;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using HC.AbpCore.AdvancePayments.DomainService;
using HC.AbpCore.DingTalk.Employees.DomainService;
using HC.AbpCore.PaymentPlans.DomainService;
using HC.AbpCore.Projects.DomainService;
using HC.AbpCore.Reimburses.DomainService;
using HC.AbpCore.Tasks.DomainService;
using HC.AbpCore.Tenders.DomainService;
using HC.AbpCore.TimeSheets.DomainService;
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
        private readonly IPaymentPlanManager _paymentPlanManager;
        private readonly IReimburseManager _reimburseManager;
        private readonly ITimeSheetManager _timeSheetManager;
        private readonly IProjectManager _projectManager;
        private readonly IEmployeeManager _employeeManager;
        private readonly ICompletedTaskManager _completedTaskManager;
        private readonly IAdvancePaymentManager _advancePaymentManager;
        private readonly ITenderManager _tenderManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public DingTalkAppService(
        IDingTalkManager dingTalkManager,
        IPaymentPlanManager paymentPlanManager,
        ITimeSheetManager timeSheetManager,
        IEmployeeManager employeeManager,
        IProjectManager projectManager,
        IReimburseManager reimburseManager,
        ICompletedTaskManager completedTaskManager,
        IAdvancePaymentManager advancePaymentManager,
        ITenderManager tenderManager
        )
        {
            _tenderManager = tenderManager;
            _advancePaymentManager = advancePaymentManager;
            _completedTaskManager = completedTaskManager;
            _dingTalkManager = dingTalkManager;
            _employeeManager = employeeManager;
            _projectManager = projectManager;
            _timeSheetManager = timeSheetManager;
            _reimburseManager = reimburseManager;
            _paymentPlanManager = paymentPlanManager;
        }


        /// <summary>
        /// 工作消息通知  每天早上9点提醒
        /// </summary>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [Audited]
        [RemoteService(false)]
        public async Task AutoWorkNotificationMessageAsync()
        {
            var accessToken = await _dingTalkManager.GetAccessTokenByAppAsync(DingDingAppEnum.智能办公);
            var ddConfig = await _dingTalkManager.GetDingDingConfigByAppAsync(DingDingAppEnum.智能办公);
            //催款提醒
            await _paymentPlanManager.PaymentRemindAsync(accessToken, ddConfig);
            //报销审批提醒
            await _reimburseManager.ReimburseApprovalRemind(accessToken, ddConfig);
            //工时审批提醒
            await _timeSheetManager.TimeSheetApprovalRemind(accessToken, ddConfig);
            //任务提醒
            await _completedTaskManager.TaskRemindAsync(accessToken, ddConfig);
            //付款提醒
            await _advancePaymentManager.PaymentRemindAsync(accessToken, ddConfig);
            //招标提醒
            await _tenderManager.TenderRemindAsync(accessToken, ddConfig);
        }

        /// <summary>
        /// 工作消息通知  周一早上9点提醒
        /// </summary>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [Audited]
        [RemoteService(false)]
        public async Task MonWorkNotificationMessageAsync()
        {
            var accessToken = await _dingTalkManager.GetAccessTokenByAppAsync(DingDingAppEnum.智能办公);
            var ddConfig = await _dingTalkManager.GetDingDingConfigByAppAsync(DingDingAppEnum.智能办公);
            //项目进度提醒
            await _projectManager.ProjectStatusRemind(accessToken, ddConfig);
            //周报提醒  
            await _employeeManager.EmployeeWeeklyRemind(accessToken, ddConfig);
        }
    }
}

