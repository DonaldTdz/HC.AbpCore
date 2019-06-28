

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.Implements;


namespace HC.AbpCore.Implements.DomainService
{
    public interface IImplementManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitImplement();



		 
      
         

    }
}
