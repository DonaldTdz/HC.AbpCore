

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.PaymentPlans;


namespace HC.AbpCore.PaymentPlans.DomainService
{
    public interface IPaymentPlanManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitPaymentPlan();



		 
      
         

    }
}
