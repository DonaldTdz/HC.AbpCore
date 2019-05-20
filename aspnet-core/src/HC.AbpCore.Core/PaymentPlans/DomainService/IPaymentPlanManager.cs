

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.DingTalk;
using HC.AbpCore.PaymentPlans;


namespace HC.AbpCore.PaymentPlans.DomainService
{
    public interface IPaymentPlanManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitPaymentPlan();

        /// <summary>
        /// 回款提醒 
        /// </summary>
        /// <returns>employeeIds</returns>
        Task PaymentRemindAsync(string accessToken, DingDingAppConfig dingDingAppConfig);





    }
}
