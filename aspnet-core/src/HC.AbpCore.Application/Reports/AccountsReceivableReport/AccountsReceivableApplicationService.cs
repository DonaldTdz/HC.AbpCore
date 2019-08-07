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
using HC.AbpCore.Contracts;
using HC.AbpCore.Customers;
using HC.AbpCore.PaymentPlans;
using HC.AbpCore.Projects;
using HC.AbpCore.Reports.AccountsReceivableReport.Dtos;
using Microsoft.EntityFrameworkCore;
using static HC.AbpCore.Contracts.ContractEnum;

namespace HC.AbpCore.Reports.AccountsReceivableReport
{
    public class AccountsReceivableApplicationService : IAccountsReceivableApplicationService
    {
        private readonly IRepository<Project, Guid> _projectRepository;
        private readonly IRepository<Customer, int> _customerRepository;
        private readonly IRepository<PaymentPlan, Guid> _paymentPlanRepository;
        private readonly IRepository<Contract, Guid> _contractRepository;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public AccountsReceivableApplicationService(
          IRepository<Project, Guid> projectRepository,
          IRepository<Customer, int> customerRepository,
          IRepository<PaymentPlan, Guid> paymentPlanRepository,
          IRepository<Contract, Guid> contractRepository
        )
        {
            _contractRepository = contractRepository;
            _projectRepository = projectRepository;
            _customerRepository = customerRepository;
            _paymentPlanRepository = paymentPlanRepository;
        }

        /// <summary>
        /// 获取AccountsReceivableListDto的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<AccountsReceivableListDto>> GetAccountsReceivableAsync(GetAccountsReceivableInput input)
        {
            var customers = _customerRepository.GetAll().WhereIf(input.CustomerId.HasValue, aa => aa.Id == input.CustomerId.Value);
            var projects = _projectRepository.GetAll();
            var paymentPlans = _paymentPlanRepository.GetAll();

            var items = from paymentPlan in paymentPlans
                        join project in projects on paymentPlan.ProjectId equals project.Id
                        join customer in customers on project.CustomerId equals customer.Id
                        group new { customer.Name,paymentPlan.Amount,customer.CreationTime } by new { customer.Id } into gtemp
                        select (new AccountsReceivableListDto()
                        {
                            Id = gtemp.Key.Id,
                            Name = gtemp.Select(aa=>aa.Name).First(),
                            CreationTime = gtemp.Select(aa => aa.CreationTime).First(),
                            TotalAmount = gtemp.Sum(aa => aa.Amount)
                        });
            var count = await items.CountAsync();
            var entityList = await items
                            .OrderBy(input.Sorting)
                            .OrderByDescending(aa=>aa.CreationTime).AsNoTracking()
                            //.PageBy(input)
                            .ToListAsync();
            entityList.Add(new AccountsReceivableListDto() { Name = "合计", TotalAmount = entityList.Sum(aa => aa.TotalAmount) });

            return new PagedResultDto<AccountsReceivableListDto>(count, entityList);

        }

        /// <summary>
        /// 获取CustomerReceivablesDetail的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<CustomerReceivablesDetailDto>> GetCustomerReceivablesDetailAsync(GetAccountsReceivableInput input)
        {
            var projects = _projectRepository.GetAll().WhereIf(input.CustomerId.HasValue,aa=>aa.CustomerId==input.CustomerId.Value)
                .WhereIf(input.ProjectId.HasValue,aa=>aa.Id==input.ProjectId.Value);
            var paymentPlans = _paymentPlanRepository.GetAll();
            var contracts = _contractRepository.GetAll().Where(aa=>aa.Type==ContractTypeEnum.销项);
            var items = from paymentPlan in paymentPlans
                        join project in projects on paymentPlan.ProjectId equals project.Id
                        join contract in contracts on project.Id equals contract.RefId
                        group new { project.Id, project.Name,project.ProjectCode, project.CreationTime, contract.Amount, paymentPlan } by new { project.Id } into gtemp
                        select (new CustomerReceivablesDetailDto()
                        {
                            Id = gtemp.Key.Id,
                            Name = gtemp.Select(aa => aa.Name).FirstOrDefault() + "(" + gtemp.Select(aa => aa.ProjectCode).FirstOrDefault() + ")",
                            CreationTime = gtemp.Select(aa => aa.CreationTime).FirstOrDefault(),
                            AcceptedAmount = gtemp.Where(aa => aa.paymentPlan.Status == PaymentPlanStatusEnum.已回款).Sum(aa => aa.paymentPlan.Amount),
                            UncollectedAmount = gtemp.Where(aa => aa.paymentPlan.Status == PaymentPlanStatusEnum.未回款).Sum(aa => aa.paymentPlan.Amount),
                            ExpectedPaymentDate = gtemp.Max(aa => aa.CreationTime),
                            Amount = gtemp.Select(aa => aa.Amount).FirstOrDefault()
                        });

            var count = await items.CountAsync();
            var entityList = await items
                            .OrderBy(input.Sorting)
                            .OrderByDescending(aa => aa.CreationTime).AsNoTracking()
                            //.PageBy(input)
                            .ToListAsync();
            entityList.Add(new CustomerReceivablesDetailDto()
            {
                Name = "合计",
                AcceptedAmount = entityList.Sum(aa => aa.AcceptedAmount),
                UncollectedAmount = entityList.Sum(aa => aa.UncollectedAmount),
                Amount = entityList.Sum(aa => aa.Amount)
            });
            return new PagedResultDto<CustomerReceivablesDetailDto>(count, entityList);
        }
    }
}
