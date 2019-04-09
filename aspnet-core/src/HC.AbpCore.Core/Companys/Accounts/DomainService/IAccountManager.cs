

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.Companys.Accounts;


namespace HC.AbpCore.Companys.Accounts.DomainService
{
    public interface IAccountManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitAccount();



		 
      
         

    }
}
