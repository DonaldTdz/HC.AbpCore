

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.Common;
using HC.AbpCore.Products;

namespace HC.AbpCore.Products.DomainService
{
    public interface IProductManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitProduct();


        /// <summary>
        /// 自动更新产品的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task AutoUpdateAsync(Product input);

    }
}
