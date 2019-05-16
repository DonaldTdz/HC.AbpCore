

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.Messages;


namespace HC.AbpCore.Messages.DomainService
{
    public interface IMessageManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitMessage();



		 
      
         

    }
}
