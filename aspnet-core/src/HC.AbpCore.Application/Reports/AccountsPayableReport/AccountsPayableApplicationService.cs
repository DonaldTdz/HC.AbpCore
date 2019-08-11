using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using HC.AbpCore.AdvancePayments;
using HC.AbpCore.AdvancePayments.AdvancePaymentDetails;
using HC.AbpCore.Products;
using HC.AbpCore.Purchases;
using HC.AbpCore.Purchases.PurchaseDetails;
using HC.AbpCore.Reports.AccountsPayableReport.Dtos;
using Microsoft.EntityFrameworkCore;
using HC.AbpCore.Suppliers;

namespace HC.AbpCore.Reports.AccountsPayableReport
{
    public class AccountsPayableApplicationService : IAccountsPayableApplicationService
    {
        private readonly IRepository<Supplier, int> _supplierRepository;
        private readonly IRepository<Product, int> _productRepository;
        private readonly IRepository<AdvancePaymentDetail, Guid> _advancePaymentDetailRepository;
        private readonly IRepository<PurchaseDetail, Guid> _purchaseDetailRepository;
        private readonly IRepository<Purchase, Guid> _purchaseRepository;
        private readonly IRepository<AdvancePayment, Guid> _advancePaymentRepository;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public AccountsPayableApplicationService(
          IRepository<Supplier, int> supplierRepository,
          IRepository<Product, int> productRepository,
          IRepository<AdvancePaymentDetail, Guid> advancePaymentDetailRepository,
          IRepository<PurchaseDetail, Guid> purchaseDetailRepository,
          IRepository<AdvancePayment, Guid> advancePaymentRepository,
          IRepository<Purchase, Guid> purchaseRepository
        )
        {
            _supplierRepository = supplierRepository;
            _productRepository = productRepository;
            _purchaseRepository = purchaseRepository;
            _advancePaymentDetailRepository = advancePaymentDetailRepository;
            _purchaseDetailRepository = purchaseDetailRepository;
            _advancePaymentRepository = advancePaymentRepository;
        }

        public async Task<PagedResultDto<AccountsPayableListDto>> GetAccountsPayableAsync(GetAccountsPayableInput input)
        {
            //var advancePaymentDetails = _advancePaymentDetailRepository.GetAll();
            //var advancePayments = _advancePaymentRepository.GetAll();
            var suppliers = _supplierRepository.GetAll().WhereIf(input.SupplierId.HasValue, aa => aa.Id == input.SupplierId.Value);
            var products = _productRepository.GetAll();
            var purchaseDetails = _purchaseDetailRepository.GetAll();
            var purchases = _purchaseRepository.GetAll();
            var items = from supplier in suppliers
                        join purchaseDetail in purchaseDetails on supplier.Id equals purchaseDetail.SupplierId into aa
                        from bb in aa.DefaultIfEmpty()
                        join purchase in purchases on bb.PurchaseId equals purchase.Id into cc
                        from dd in cc.DefaultIfEmpty()
                        join product in products on bb.ProductId equals product.Id into ee
                        from ff in ee.DefaultIfEmpty()
                        group new { supplier.Name, bb, ff, supplier.CreationTime } by new { supplier.Id } into gtemp
                        select (new AccountsPayableListDto()
                        {
                            Id = gtemp.Key.Id,
                            Name = gtemp.Select(aa => aa.Name).FirstOrDefault(),
                            TotalAmount = gtemp.Sum(aa => (aa.bb == null ? 0 : aa.bb.Num) * (aa.ff == null ? 0 : aa.ff.Price) + (aa.bb == null ? 0 : aa.bb.Num) * (aa.ff == null ? 0 : aa.ff.Price)
                            * (Convert.ToDecimal((aa.ff == null ? "0" : aa.ff.TaxRate).Replace("%", "")) / 100)),
                            CreationTime = gtemp.Select(aa => aa.CreationTime).FirstOrDefault(),
                        });
            var count = await items.CountAsync();
            var entityList = await items
                            .OrderBy(input.Sorting)
                            .OrderByDescending(aa => aa.CreationTime).AsNoTracking()
                            //.PageBy(input)
                            .ToListAsync();
            if (count > 0)
                entityList.Add(new AccountsPayableListDto() { Name = "合计", TotalAmount = entityList.Sum(aa => aa.TotalAmount) });

            return new PagedResultDto<AccountsPayableListDto>(count, entityList);
        }

        public async Task<PagedResultDto<AccountsPayableDetailDto>> GetSupplierPayableDetailAsync(GetAccountsPayableInput input)
        {
            var advancePaymentDetails = _advancePaymentDetailRepository.GetAll();
            var advancePayments = _advancePaymentRepository.GetAll().Select(aa => new { aa.Id, aa.PurchaseId, aa.Status, aa.PlanTime });
            var products = _productRepository.GetAll();
            var purchaseDetails = _purchaseDetailRepository.GetAll().WhereIf(input.SupplierId.HasValue, aa => aa.SupplierId == input.SupplierId.Value)
                .WhereIf(input.ProductId.HasValue, aa => aa.ProductId == input.ProductId.Value)
                .Select(aa => new { aa.Id, aa.PurchaseId, aa.Num, aa.ProductId });
            var purchases = _purchaseRepository.GetAll().Select(aa => aa.Id);

            var list1 = from purchase in purchases
                        join purchaseDetail in purchaseDetails on purchase equals purchaseDetail.PurchaseId
                        join advancePayment in advancePayments on purchase equals advancePayment.PurchaseId
                        select new
                        {
                            purchaseDetail.ProductId,
                            advancePayment.Id,
                            purchaseDetailId = purchaseDetail.Id,
                            advancePayment.Status,
                            advancePayment.PlanTime,
                        };

            var list2 = from list in list1
                        join advancePaymentDetail in advancePaymentDetails on list.Id equals advancePaymentDetail.AdvancePaymentId
                        where advancePaymentDetail.PurchaseDetailId == list.purchaseDetailId
                        select new
                        {
                            list.ProductId,
                            list.Status,
                            list.PlanTime,
                            Amount = advancePaymentDetail.Amount ?? 0
                        };

            var items = from product in products
                        join purchaseDetail in purchaseDetails on product.Id equals purchaseDetail.ProductId
                        join purchase in purchases on purchaseDetail.PurchaseId equals purchase
                        group new { product, purchaseDetail.Num } by product.Id into gtem
                        select new AccountsPayableDetailDto()
                        {
                            Id = gtem.Key,
                            Name = gtem.Select(aa => aa.product.Name).FirstOrDefault(),
                            Amount = gtem.Sum(aa => aa.Num * aa.product.Price + aa.Num * aa.product.Price * (Convert.ToDecimal(aa.product.TaxRate.Replace("%", "")) / 100)),
                            AcceptedAmount = 0,
                            UncollectedAmount = 0,
                        };
            if (await list2.CountAsync() > 0)
            {
                items = from product in items
                        join list in list2 on product.Id equals list.ProductId into aa
                        from bb in aa.DefaultIfEmpty()
                        group new { product, bb } by product.Id into gtem
                        select new AccountsPayableDetailDto()
                        {
                            Id = gtem.Key,
                            Name = gtem.Select(aa => aa.product.Name).FirstOrDefault(),
                            Amount = gtem.Select(aa => aa.product.Amount).FirstOrDefault(),
                            AcceptedAmount = gtem.Where(aa => aa.bb.Status == AdvancePaymentStatusEnum.已付款).Sum(aa => aa.bb.Amount),
                            UncollectedAmount = gtem.Where(Aa => Aa.bb.Status == AdvancePaymentStatusEnum.未付款).Sum(aa => aa.bb.Amount),
                            ExpectedPaymentDate = gtem.Max(aa => aa.bb.PlanTime)
                        };
            }
            //return null;
            var count = await items.CountAsync();
            var entityList = await items
                            .OrderBy(input.Sorting).AsNoTracking()
                            //.PageBy(input)
                            .ToListAsync();
            if (count > 0)
            {
                entityList.Add(new AccountsPayableDetailDto()
                {
                    Name = "合计",
                    Amount = entityList.Sum(aa => aa.Amount),
                    AcceptedAmount = entityList.Sum(aa => aa.AcceptedAmount),
                    UncollectedAmount = entityList.Sum(aa => aa.UncollectedAmount)
                });
            }
            return new PagedResultDto<AccountsPayableDetailDto>(count, entityList);

        }
    }
}
