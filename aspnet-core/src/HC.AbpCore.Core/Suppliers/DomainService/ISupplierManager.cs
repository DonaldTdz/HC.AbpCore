

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.Suppliers;


namespace HC.AbpCore.Suppliers.DomainService
{
    public interface ISupplierManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitSupplier();



		 
      
         

    }
}
