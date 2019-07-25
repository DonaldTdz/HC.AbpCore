

using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.Extensions;
using Abp.UI;
using Abp.Domain.Repositories;
using Abp.Domain.Services;

using HC.AbpCore;
using HC.AbpCore.Products;
using HC.AbpCore.Common;
using HC.AbpCore.Projects;

namespace HC.AbpCore.Products.DomainService
{
    /// <summary>
    /// Product领域层的业务管理
    ///</summary>
    public class ProductManager : AbpCoreDomainServiceBase, IProductManager
    {

        private readonly IRepository<Product, int> _repository;

        /// <summary>
        /// Product的构造方法
        ///</summary>
        public ProductManager(
            IRepository<Product, int> repository
        )
        {
            _repository = repository;
        }

        /// <summary>
        /// 自动更新产品的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task AutoUpdateAsync(Product input)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            var product = await _repository.FirstOrDefaultAsync(aa => aa.Name == input.Name && aa.Specification == input.Specification
             && aa.Price == input.Price && aa.TaxRate == input.TaxRate);
            //有则修改,无则更新
            if (product != null)
            {
                if (product.Num.HasValue)
                    product.Num += input.Num;
                else
                    product.Num = input.Num;
                await _repository.UpdateAsync(product);
            }
            else
            {
                input.IsEnabled = true;
                await _repository.InsertAsync(input);
            }
        }


        /// <summary>
        /// 初始化
        ///</summary>
        public void InitProduct()
        {
            throw new NotImplementedException();
        }

        // TODO:编写领域业务代码







    }
}
