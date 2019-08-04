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
using HC.AbpCore.Customers;
using HC.AbpCore.PaymentPlans;
using HC.AbpCore.Projects;
using HC.AbpCore.Reports.AccountsReceivableReport.Dtos;
using Microsoft.EntityFrameworkCore;

namespace HC.AbpCore.Reports.AccountsReceivableReport
{
    public class AccountsReceivableApplicationService : IAccountsReceivableApplicationService
    {
        private readonly IRepository<Project, Guid> _projectRepository;
        private readonly IRepository<Customer, int> _customerRepository;
        private readonly IRepository<PaymentPlan, Guid> _paymentPlanRepository;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public AccountsReceivableApplicationService(
          IRepository<Project, Guid> projectRepository,
          IRepository<Customer, int> customerRepository,
          IRepository<PaymentPlan, Guid> paymentPlanRepository
        )
        {
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
            var customers = _customerRepository.GetAll().WhereIf(input.CustomerId.HasValue, aa => aa.Id == input.CustomerId);
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
                            .PageBy(input)
                            .ToListAsync();

            return new PagedResultDto<AccountsReceivableListDto>(count, entityList);

        }

        /// <summary>
        /// 获取CustomerReceivablesDetail的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<CustomerReceivablesDetailDto>> GetCustomerReceivablesDetailAsync(GetAccountsReceivableInput input)
        {
            throw new NotImplementedException();
        }
    }
}
