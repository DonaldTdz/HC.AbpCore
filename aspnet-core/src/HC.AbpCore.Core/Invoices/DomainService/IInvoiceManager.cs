

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.Invoices;


namespace HC.AbpCore.Invoices.DomainService
{
    public interface IInvoiceManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitInvoice();



		 
      
         

    }
}
