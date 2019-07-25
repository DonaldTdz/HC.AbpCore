

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.AdvancePayments;


namespace HC.AbpCore.AdvancePayments.DomainService
{
    public interface IAdvancePaymentManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitAdvancePayment();




    }
}
