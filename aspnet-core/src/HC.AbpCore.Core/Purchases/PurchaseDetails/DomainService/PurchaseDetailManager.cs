

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
using HC.AbpCore.Purchases.PurchaseDetails;
using HC.AbpCore.Products;

namespace HC.AbpCore.Purchases.PurchaseDetails.DomainService
{
    /// <summary>
    /// PurchaseDetail领域层的业务管理
    ///</summary>
    public class PurchaseDetailManager :AbpCoreDomainServiceBase, IPurchaseDetailManager
    {
		
		private readonly IRepository<PurchaseDetail,Guid> _repository;
        private readonly IRepository<Product, int> _productRepository;

        /// <summary>
        /// PurchaseDetail的构造方法
        ///</summary>
        public PurchaseDetailManager(
			IRepository<PurchaseDetail, Guid> repository
            , IRepository<Product, int> productRepository
        )
		{
            _productRepository = productRepository;
            _repository =  repository;
		}

        /// <summary>
        /// 新增明细并更新产品表
        /// </summary>
        /// <param name="purchaseDetailNew"></param>
        /// <returns></returns>
        public async Task<PurchaseDetail> CreateAsync(PurchaseDetailNew purchaseDetailNew)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            int productId=0;
            if (purchaseDetailNew.ProductId.HasValue)
            {
                var product = await _productRepository.GetAsync(purchaseDetailNew.ProductId.Value);
                if (product.Num.HasValue)
                    product.Num += purchaseDetailNew.Num;
                else
                    product.Num = purchaseDetailNew.Num;
                await _productRepository.UpdateAsync(product);
                productId = product.Id;
            }
            else
            {
                //查询有无完全一致的产品
                var product = await _productRepository.FirstOrDefaultAsync(aa => aa.Name == purchaseDetailNew.Name && aa.Specification == purchaseDetailNew.Specification
                 && aa.Price == purchaseDetailNew.Price && aa.TaxRate == purchaseDetailNew.TaxRate);
                //有则修改,无则更新
                if (product != null)
                {
                    if (product.Num.HasValue)
                        product.Num += purchaseDetailNew.Num;
                    else
                        product.Num = purchaseDetailNew.Num;
                    await _productRepository.UpdateAsync(product);
                    productId = product.Id;
                }
                else
                {
                    Product productNew = new Product();
                    productNew.Name = purchaseDetailNew.Name;
                    productNew.Num = purchaseDetailNew.Num;
                    productNew.Price = purchaseDetailNew.Price;
                    productNew.Specification = purchaseDetailNew.Specification;
                    productNew.TaxRate = purchaseDetailNew.TaxRate;
                    productNew.Type = 0;
                    productNew.IsEnabled = true;
                    productId = await _productRepository.InsertAndGetIdAsync(productNew);
                    //productId = productId;
                }
            }
            PurchaseDetail purchaseDetail = new PurchaseDetail();
            purchaseDetail.Num = purchaseDetailNew.Num;
            purchaseDetail.ProductId = productId;
            purchaseDetail.PurchaseId = purchaseDetailNew.PurchaseId;
            purchaseDetail.SupplierId = purchaseDetailNew.SupplierId;
            purchaseDetail = await _repository.InsertAsync(purchaseDetail);
            return purchaseDetail;
        }

        /// <summary>
        /// 删除明细并更新产品表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Guid id)
        {
            var entity = await _repository.GetAsync(id);
            var product = await _productRepository.GetAsync(entity.ProductId.Value);
            product.Num -= entity.Num;
            await _productRepository.UpdateAsync(product);
            await _repository.DeleteAsync(id);
        }


        /// <summary>
        /// 初始化
        ///</summary>
        public void InitPurchaseDetail()
		{
			throw new NotImplementedException();
		}

        /// <summary>
        /// 编辑明细并更新产品表
        /// </summary>
        /// <param name="purchaseDetail"></param>
        /// <returns></returns>
        public async Task UpdateAsync(PurchaseDetail purchaseDetail)
        {
            var entity = await _repository.GetAsync(purchaseDetail.Id);
            var product = await _productRepository.GetAsync(purchaseDetail.ProductId.Value);
            if (entity.ProductId == purchaseDetail.ProductId)
            {
                if (product.Num.HasValue)
                    product.Num += purchaseDetail.Num;
                else
                    product.Num = purchaseDetail.Num;
                product.Num -= entity.Num;
                await _productRepository.UpdateAsync(product);
                entity.Num = purchaseDetail.Num;
                await _repository.UpdateAsync(entity);
            }
            else
            {
                var productOld = await _productRepository.GetAsync(entity.ProductId.Value);
                productOld.Num -= purchaseDetail.Num;
                await _productRepository.UpdateAsync(productOld);
                if (product.Num.HasValue)
                    product.Num += purchaseDetail.Num;
                else
                    product.Num = purchaseDetail.Num;
                await _productRepository.UpdateAsync(product);
                entity.Num = purchaseDetail.Num;
                entity.ProductId = purchaseDetail.ProductId;
                await _repository.UpdateAsync(entity);
            }
        }

        // TODO:编写领域业务代码







    }
}
