

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
using HC.AbpCore.Purchases;
using HC.AbpCore.AdvancePayments;
using HC.AbpCore.Products;
using HC.AbpCore.Purchases.PurchaseDetails;

namespace HC.AbpCore.Purchases.DomainService
{
    /// <summary>
    /// Purchase领域层的业务管理
    ///</summary>
    public class PurchaseManager :AbpCoreDomainServiceBase, IPurchaseManager
    {
		
		private readonly IRepository<Purchase,Guid> _repository;
        private readonly IRepository<PurchaseDetail, Guid> _purchaseDetailRepository;
        private readonly IRepository<AdvancePayment, Guid> _advancePaymentRepository;
        private readonly IRepository<Product, int> _productrepository;

        /// <summary>
        /// Purchase的构造方法
        ///</summary>
        public PurchaseManager(
			IRepository<Purchase, Guid> repository
            , IRepository<PurchaseDetail, Guid> purchaseDetailRepository
            , IRepository<AdvancePayment, Guid> advancePaymentRepository
            , IRepository<Product, int> productrepository
        )
		{
			_repository =  repository;
            _purchaseDetailRepository = purchaseDetailRepository;
            _advancePaymentRepository = advancePaymentRepository;
            _productrepository = productrepository;
        }


		/// <summary>
		/// 初始化
		///</summary>
		public void InitPurchase()
		{
			throw new NotImplementedException();
		}

        /// <summary>
        ///  Web一键新增采购,采购明细,产品,预付款计划
        /// </summary>
        /// <param name="purchase"></param>
        /// <param name="purchaseDetailNews"></param>
        /// <param name="advancePayments"></param>
        /// <returns></returns>
        public async Task OnekeyCreateAsync(Purchase purchase, List<PurchaseDetailNew> purchaseDetailNews, List<AdvancePayment> advancePayments)
        {
            //新增采购
            var purchaseId = await _repository.InsertAndGetIdAsync(purchase);
            //新增采购明细和产品
            foreach (var item in purchaseDetailNews)
            {
                PurchaseDetail purchaseDetail = new PurchaseDetail();
                int productId;
                //如果有产品id则修改,无则新增
                if (item.ProductId.HasValue)
                {
                    var product = await _productrepository.FirstOrDefaultAsync(item.ProductId.Value);
                    if (product.Num.HasValue)
                        product.Num += item.Num;
                    else
                        product.Num = item.Num;
                    await _productrepository.UpdateAsync(product);
                    productId = product.Id;
                }
                else
                {
                    var product = await _productrepository.FirstOrDefaultAsync(aa => aa.Name == item.Name && aa.Specification == item.Specification
             && aa.Price == item.Price && aa.TaxRate == item.TaxRate);
                    //有则修改,无则更新
                    if (product != null)
                    {
                        if (product.Num.HasValue)
                            product.Num += item.Num;
                        else
                            product.Num = item.Num;
                        await _productrepository.UpdateAsync(product);
                        productId = product.Id;
                    }
                    else
                    {
                        Product product1 = new Product();
                        product1.Name = item.Name;
                        product1.Num = item.Num;
                        product1.Price = item.Price;
                        product1.Specification = item.Specification;
                        product1.TaxRate = item.TaxRate;
                        product1.IsEnabled = true;
                        product1.Type = 0;
                        productId= await _productrepository.InsertAndGetIdAsync(product1);
                    }
                }
                purchaseDetail.Num = item.Num;
                purchaseDetail.ProductId = productId;
                purchaseDetail.PurchaseId = purchaseId;
                purchaseDetail.SupplierId = item.SupplierId;
                await _purchaseDetailRepository.InsertAsync(purchaseDetail);
            }
            //新增预付款计划
            foreach (var item in advancePayments)
            {
                item.PurchaseId = purchaseId;
                await _advancePaymentRepository.InsertAsync(item);
            }
        }

        // TODO:编写领域业务代码







    }
}
