

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.Reimburses;


namespace HC.AbpCore.Reimburses.DomainService
{
    public interface IReimburseManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitReimburse();



		 
      
         

    }
}
