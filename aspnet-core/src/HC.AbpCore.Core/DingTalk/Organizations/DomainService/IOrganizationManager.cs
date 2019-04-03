

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.DingTalk.Organizations;


namespace HC.AbpCore.DingTalk.Organizations.DomainService
{
    public interface IOrganizationManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitOrganization();



		 
      
         

    }
}
