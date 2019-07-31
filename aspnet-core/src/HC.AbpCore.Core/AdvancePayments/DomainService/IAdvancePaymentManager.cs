

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.AdvancePayments;
using HC.AbpCore.DingTalk;

namespace HC.AbpCore.AdvancePayments.DomainService
{
    public interface IAdvancePaymentManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitAdvancePayment();


        /// <summary>
        /// 添加AdvancePayment并更新公司流水的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<AdvancePayment> CreateAndmodifyAccountAsync(AdvancePayment input);


        /// <summary>
        /// 修改AdvancePayment并更新公司流水的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<AdvancePayment> UpdateAndmodifyAccountAsync(AdvancePayment input);


        /// <summary>
        /// 删除AdvancePayment并且删除账户流水信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAndDeleteAccountAsync(Guid input);


        /// <summary>
        /// 付款提醒 
        /// </summary>
        /// <returns>employeeIds</returns>
        Task PaymentRemindAsync(string accessToken, DingDingAppConfig dingDingAppConfig);

    }
}
