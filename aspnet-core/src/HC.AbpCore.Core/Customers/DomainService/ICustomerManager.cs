

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.Customers;


namespace HC.AbpCore.Customers.DomainService
{
    public interface ICustomerManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitCustomer();



		 
      
         

    }
}
