

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.Common;
using HC.AbpCore.Customers;


namespace HC.AbpCore.Customers.DomainService
{
    public interface ICustomerManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitCustomer();



        /// <summary>
        /// 根据Id修改联系人名称
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Contact"></param>
        /// <returns></returns>
        //Task<ResultCode> UpdateContactById(int id, string Contact);




    }
}
