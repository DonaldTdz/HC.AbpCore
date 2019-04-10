

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.Products;


namespace HC.AbpCore.Products.DomainService
{
    public interface IProductManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitProduct();



		 
      
         

    }
}
