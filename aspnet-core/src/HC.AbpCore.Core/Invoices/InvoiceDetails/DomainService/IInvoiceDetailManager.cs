

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.Invoices.InvoiceDetails;


namespace HC.AbpCore.Invoices.InvoiceDetails.DomainService
{
    public interface IInvoiceDetailManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitInvoiceDetail();



		 
      
         

    }
}
