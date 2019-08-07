

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.AdvancePayments.AdvancePaymentDetails;


namespace HC.AbpCore.AdvancePayments.AdvancePaymentDetails.DomainService
{
    public interface IAdvancePaymentDetailManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitAdvancePaymentDetail();



		 
      
         

    }
}
