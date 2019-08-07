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
                        group new { supplier.Name, bb,ff, supplier.CreationTime } by new { supplier.Id } into gtemp
                        select (new AccountsPayableListDto()
                        {
                            Id = gtemp.Key.Id,
                            Name = gtemp.Select(aa => aa.Name).FirstOrDefault(),
                            TotalAmount = gtemp.Sum(aa => (aa.bb==null?0:aa.bb.Num) * (aa.ff==null?0:aa.ff.Price) + (aa.bb == null ? 0 : aa.bb.Num) * (aa.ff == null ? 0 : aa.ff.Price)
                            * (Convert.ToDecimal((aa.ff == null ? "0" : aa.ff.TaxRate).Replace("%", "")) /100)),
                            CreationTime = gtemp.Select(aa => aa.CreationTime).FirstOrDefault(),
                        }) ;
            var count = await items.CountAsync();
            var entityList = await items
                            .OrderBy(input.Sorting)
                            .OrderByDescending(aa => aa.CreationTime).AsNoTracking()
                            //.PageBy(input)
                            .ToListAsync();
            entityList.Add(new AccountsPayableListDto() { Name = "合计", TotalAmount = entityList.Sum(aa => aa.TotalAmount) });

            return new PagedResultDto<AccountsPayableListDto>(count, entityList);
        }

        public async Task<PagedResultDto<AccountsPayableDetailDto>> GetSupplierPayableDetailAsync(GetAccountsPayableInput input)
        {
            var advancePaymentDetails = _advancePaymentDetailRepository.GetAll().Select(aa=>new { aa.Id,aa.PurchaseDetailId,aa.Amount,aa.AdvancePaymentId});
            var advancePayments = _advancePaymentRepository.GetAll().Select(aa => new { aa.Id, aa.PurchaseId,aa.Status,aa.PlanTime });
            var products = _productRepository.GetAll();
            var purchaseDetails = _purchaseDetailRepository.GetAll().Where(aa => aa.SupplierId == input.SupplierId.Value)
                .WhereIf(input.ProductId.HasValue,aa=>aa.ProductId==input.ProductId.Value)
                .Select(aa=>new {aa.Id,aa.SupplierId,aa.PurchaseId,aa.Num,aa.ProductId });
            var purchases = _purchaseRepository.GetAll().Select(aa=>aa.Id);
            var PaymentDetails = from advancePaymentDetail in advancePaymentDetails
                        join advancePayment in advancePayments on advancePaymentDetail.AdvancePaymentId equals advancePayment.Id
                        select new
                        {
                            advancePaymentId = advancePayment.Id,
                            advancePaymentDetailId=advancePaymentDetail.Id,
                            PurchaseId = advancePayment.PurchaseId,
                            purchaseDetailId=advancePaymentDetail.PurchaseDetailId,
                            Amount=advancePaymentDetail.Amount,
                            advancePayment.Status,
                            ExpectedPaymentDate=advancePayment.PlanTime
                        };


            var items =( from purchaseDetail in purchaseDetails
                        join product in products on purchaseDetail.ProductId equals product.Id
                        join purchase in purchases on purchaseDetail.PurchaseId equals purchase
                         join PaymentDetail in PaymentDetails on purchase equals PaymentDetail.PurchaseId into aa
                         from bb in aa.DefaultIfEmpty()
                             //where bb.purchaseDetailId==purchaseDetail.Id
                         group new { product.Name,product.Price,product.TaxRate,bb, purchaseDetail.Num, purchaseDetail.Id,product.CreationTime } by new { product.Id, bb.purchaseDetailId,bb.Status } into gtemp
                        select (new AccountsPayableDetailDto()
                        {
                            Id=gtemp.Key.Id,
                            Name=gtemp.Select(aa=>aa.Name).FirstOrDefault(),
                            Amount=gtemp.Sum(aa=>aa.Num*aa.Price+aa.Num*aa.Price* (Convert.ToDecimal(aa.TaxRate.Replace("%", ""))/100)),
                            //AcceptedAmount=gtemp.Where(aa=>aa.bb.Status==AdvancePaymentStatusEnum.已付款).Sum(aa=>aa.bb.Amount),
                            AcceptedAmount = gtemp.Key.purchaseDetailId == null?0:gtemp.Where(aa=>aa.Id==gtemp.Key.purchaseDetailId&&gtemp.Key.Status==AdvancePaymentStatusEnum.已付款).Sum(aa=>aa.bb.Amount),
                            //UncollectedAmount = gtemp.Where(aa => aa.bb.Status == AdvancePaymentStatusEnum.未付款).Sum(aa => aa.bb.Amount),
                            ExpectedPaymentDate =gtemp.Max(aa=>aa.CreationTime)
                        }));

            //on new { bb.Id, a = purchaseDetail.Id } 
            //equals new { advancePaymentDetail.AdvancePaymentId, id1=advancePaymentDetail.PurchaseDetailId} into cc
            //from dd in cc.
            var count = await items.CountAsync();
            var entityList = await items
                            .OrderBy(input.Sorting).AsNoTracking()
                            //.PageBy(input)
                            .ToListAsync();
            entityList.Add(new AccountsPayableDetailDto() { Name = "合计", Amount = entityList.Sum(aa => aa.Amount), AcceptedAmount=entityList.Sum(aa=>aa.AcceptedAmount),
            UncollectedAmount=entityList.Sum(aa=>aa.UncollectedAmount)});
            return new PagedResultDto<AccountsPayableDetailDto>(count, entityList);

        }
    }
}
