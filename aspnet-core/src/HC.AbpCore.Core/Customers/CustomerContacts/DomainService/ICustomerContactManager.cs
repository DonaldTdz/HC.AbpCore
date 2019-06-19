

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.Customers.CustomerContacts;


namespace HC.AbpCore.Customers.CustomerContacts.DomainService
{
    public interface ICustomerContactManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitCustomerContact();



		 
      
         

    }
}
