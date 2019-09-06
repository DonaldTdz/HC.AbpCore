

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
using HC.AbpCore.Reports.AccountAnalysisReport;
using HC.AbpCore.Projects;
using HC.AbpCore.Customers;
using HC.AbpCore.PaymentPlans;
using HC.AbpCore.Purchases;
using HC.AbpCore.Purchases.PurchaseDetails;
using HC.AbpCore.AdvancePayments;
using HC.AbpCore.AdvancePayments.AdvancePaymentDetails;
using HC.AbpCore.Suppliers;

namespace HC.AbpCore.Reports.AccountAnalysisReport.DomainService
{
    /// <summary>
    /// AccountAnalysis领域层的业务管理
    ///</summary>
    public class AccountAnalysisManager : AbpCoreDomainServiceBase, IAccountAnalysisManager
    {
        private readonly IRepository<Project, Guid> _projectRepository;
        private readonly IRepository<Customer, int> _customerRepository;
        private readonly IRepository<PaymentPlan, Guid> _paymentPlanRepository;
        private readonly IRepository<Purchase, Guid> _purchaseRepository;
        private readonly IRepository<PurchaseDetail, Guid> _PurchaseDetailRepository;
        private readonly IRepository<AdvancePayment, Guid> _apRepository;
        private readonly IRepository<AdvancePaymentDetail, Guid> _apDetailRepository;
        private readonly IRepository<Supplier, int> _supplierRepository;

        /// <summary>
        /// AccountAnalysis的构造方法
        ///</summary>
        public AccountAnalysisManager(
            IRepository<Project, Guid> projectRepository,
            IRepository<PaymentPlan, Guid> paymentPlanRepository,
            IRepository<Customer, int> customerRepository,
            IRepository<Purchase, Guid> purchaseRepository,
            IRepository<PurchaseDetail, Guid> PurchaseDetailRepository,
            IRepository<AdvancePayment, Guid> apRepository,
            IRepository<AdvancePaymentDetail, Guid> apDetailRepository,
            IRepository<Supplier, int> supplierRepository
        )
        {
            _projectRepository = projectRepository;
            _paymentPlanRepository = paymentPlanRepository;
            _customerRepository = customerRepository;
            _purchaseRepository = purchaseRepository;
            _PurchaseDetailRepository = PurchaseDetailRepository;
            _apRepository = apRepository;
            _apDetailRepository = apDetailRepository;
            _supplierRepository = supplierRepository;
        }

        public async Task<ResultAccountAnalysis> GetAccountAnalysesAsync(TypeEnum type, int? refId)
        {
            if (type == TypeEnum.应收)
                return await GetReceivablesAsync(refId);
            else
                return await GetPayablesAsync(refId);
        }

        /// <summary>
        /// 获取应付账款
        /// </summary>
        /// <param name="refId"></param>
        /// <returns></returns>
        private async Task<ResultAccountAnalysis> GetPayablesAsync(int? refId)
        {
            ResultAccountAnalysis resultAccountAnalysis = new ResultAccountAnalysis();
            var advancePayments = _apRepository.GetAll().Where(aa => aa.Status == AdvancePaymentStatusEnum.未付款).Select(aa => new { aa.Id, aa.PlanTime, aa.Desc });
            var advancePaymentDetails = _apDetailRepository.GetAll().Select(aa => new { aa.Id, aa.AdvancePaymentId, aa.Amount, aa.PurchaseDetailId });
            var purchases = _purchaseRepository.GetAll().Select(aa => aa.Id);
            var purchaseDetails = _PurchaseDetailRepository.GetAll().Select(aa => new { aa.Id, aa.PurchaseId, aa.SupplierId });
            var purchaseDetailList = from purchaseDetail in purchaseDetails
                                     join purchase in purchases on purchaseDetail.PurchaseId equals purchase
                                     select new
                                     {
                                         purchaseDetail.Id,
                                         purchaseDetail.SupplierId
                                     };
            var entitys = from advancePaymentDetail in advancePaymentDetails
                          join advancePayment in advancePayments on advancePaymentDetail.AdvancePaymentId equals advancePayment.Id
                          join purchaseDetail in purchaseDetailList on advancePaymentDetail.PurchaseDetailId equals purchaseDetail.Id
                          group new { purchaseDetail, advancePayment, advancePaymentDetail } by new { purchaseDetail.SupplierId, PlanTime = advancePayment.PlanTime.ToString("yyyy-MM") } into gtemp
                          let remarkss = gtemp.Select(b => b.advancePayment.Desc).ToArray()
                          select (new
                          {
                              gtemp.Key.PlanTime,
                              gtemp.Key.SupplierId,
                              Amount = gtemp.Sum(aa => aa.advancePaymentDetail.Amount ?? 0),
                              Remarks = string.Join(",", remarkss)
                          });
            var items = await entitys.AsNoTracking().ToListAsync();

            var suppliers = await _supplierRepository.GetAll().WhereIf(refId.HasValue, aa => aa.Id == refId.Value).Select(aa => new { aa.Id, aa.Name }).AsNoTracking().ToListAsync();
            List<AccountAnalysis> AccountAnalysisList = new List<AccountAnalysis>();
            var planTimes = items.OrderBy(aa => aa.PlanTime).GroupBy(aa => aa.PlanTime).Select(aa => aa.Key).ToList();

            AccountAnalysis accountAnalysis1Total = new AccountAnalysis();
            List<decimal> amountTotal = new List<decimal>();
            accountAnalysis1Total.Name = "合计";
            foreach (var planTime in planTimes)
            {
                Detail detail = new Detail();
                if (refId.HasValue)
                    amountTotal.Add(items.Where(aa => aa.PlanTime == planTime && aa.SupplierId == refId.Value).Sum(aa => aa.Amount));
                else
                    amountTotal.Add(items.Where(aa => aa.PlanTime == planTime).Sum(aa => aa.Amount));
            }
            accountAnalysis1Total.Amount = amountTotal;
            foreach (var supplier in suppliers)
            {
                if (items?.Count(aa => aa.SupplierId == supplier.Id) > 0)
                {
                    List<decimal> amount = new List<decimal>();
                    AccountAnalysis accountAnalysis = new AccountAnalysis();
                    accountAnalysis.Id = supplier.Id;
                    accountAnalysis.Name = supplier.Name;
                    foreach (var planTime in planTimes)
                    {
                        Detail detail = new Detail();
                        amount.Add(items.Where(aa => aa.PlanTime == planTime && aa.SupplierId == supplier.Id).Sum(aa => aa.Amount));
                    }
                    accountAnalysis.Amount = amount;
                    var remarksArray = items.Where(aa => aa.SupplierId == supplier.Id).Select(aa => aa.Remarks).ToArray();
                    accountAnalysis.Remarks = string.Join(",", remarksArray);
                    AccountAnalysisList.Add(accountAnalysis);
                }
            }
            AccountAnalysisList.Add(accountAnalysis1Total);
            resultAccountAnalysis.AccountAnalyses = AccountAnalysisList;
            resultAccountAnalysis.PlanTimes = planTimes;
            return resultAccountAnalysis;
        }

        /// <summary>
        /// 获取应收账款
        /// </summary>
        /// <param name="refId"></param>
        /// <returns></returns>
        private async Task<ResultAccountAnalysis> GetReceivablesAsync(int? refId)
        {
            ResultAccountAnalysis resultAccountAnalysis = new ResultAccountAnalysis();
            var projects = _projectRepository.GetAll().Select(aa => new { aa.Id, aa.CustomerId });
            var paymentPlans = _paymentPlanRepository.GetAll().Where(aa => aa.Status == PaymentPlanStatusEnum.未回款)
                .Select(aa => new { aa.Id, aa.ProjectId, aa.Amount, aa.PaymentCondition, aa.PlanTime });

            var entitys = from paymentPlan in paymentPlans
                          join project in projects on paymentPlan.ProjectId equals project.Id
                          group new { paymentPlan, project } by new { project.CustomerId, PlanTime = paymentPlan.PlanTime.ToString("yyyy-MM") } into gtemp
                          let remarkss = gtemp.Select(b => b.paymentPlan.PaymentCondition).ToArray()
                          select (new
                          {
                              gtemp.Key.PlanTime,
                              gtemp.Key.CustomerId,
                              Amount = gtemp.Sum(aa => aa.paymentPlan.Amount ?? 0),
                              Remarks = string.Join(",", remarkss)
                          });
            var items = await entitys.ToListAsync();

            var customers = await _customerRepository.GetAll().WhereIf(refId.HasValue, aa => aa.Id == refId.Value)
                .Select(aa => new { aa.Id, aa.Name }).ToListAsync();
            List<AccountAnalysis> AccountAnalysisList = new List<AccountAnalysis>();
            var planTimes = items.OrderBy(aa => aa.PlanTime).GroupBy(aa => aa.PlanTime).Select(aa => aa.Key).ToList();

            AccountAnalysis accountAnalysis1Total = new AccountAnalysis();
            List<decimal> amountTotal = new List<decimal>();
            accountAnalysis1Total.Name = "合计";
            foreach (var planTime in planTimes)
            {
                Detail detail = new Detail();
                if (refId.HasValue)
                    amountTotal.Add(items.Where(aa => aa.PlanTime == planTime && aa.CustomerId == refId.Value).Sum(aa => aa.Amount));
                else
                    amountTotal.Add(items.Where(aa => aa.PlanTime == planTime).Sum(aa => aa.Amount));
            }
            accountAnalysis1Total.Amount = amountTotal;
            foreach (var customer in customers)
            {
                if (items?.Count(aa => aa.CustomerId == customer.Id) > 0)
                {
                    List<decimal> amount = new List<decimal>();
                    AccountAnalysis accountAnalysis = new AccountAnalysis();
                    accountAnalysis.Id = customer.Id;
                    accountAnalysis.Name = customer.Name;
                    foreach (var planTime in planTimes)
                    {
                        Detail detail = new Detail();
                        amount.Add(items.Where(aa => aa.PlanTime == planTime && aa.CustomerId == customer.Id).Sum(aa => aa.Amount));
                    }
                    accountAnalysis.Amount = amount;
                    var remarksArray = items.Where(aa => aa.CustomerId == customer.Id).Select(aa => aa.Remarks).ToArray();
                    accountAnalysis.Remarks = string.Join(",", remarksArray);
                    AccountAnalysisList.Add(accountAnalysis);
                }
            }
            AccountAnalysisList.Add(accountAnalysis1Total);
            resultAccountAnalysis.AccountAnalyses = AccountAnalysisList;
            resultAccountAnalysis.PlanTimes = planTimes;
            return resultAccountAnalysis;
        }


        /// <summary>
        /// 初始化
        ///</summary>
        public void InitAccountAnalysis()
        {
            throw new NotImplementedException();
        }

        // TODO:编写领域业务代码







    }
}
