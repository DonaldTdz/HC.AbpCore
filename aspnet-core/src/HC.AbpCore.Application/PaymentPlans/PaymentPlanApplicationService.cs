
using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using Abp.UI;
using Abp.AutoMapper;
using Abp.Extensions;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Application.Services.Dto;
using Abp.Linq.Extensions;


using HC.AbpCore.PaymentPlans;
using HC.AbpCore.PaymentPlans.Dtos;
using HC.AbpCore.PaymentPlans.DomainService;
using HC.AbpCore.Companys;
using HC.AbpCore.Companys.Accounts;
using HC.AbpCore.Companys.Accounts.Dtos;

namespace HC.AbpCore.PaymentPlans
{
    /// <summary>
    /// PaymentPlan应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class PaymentPlanAppService : AbpCoreAppServiceBase, IPaymentPlanAppService
    {
        private readonly IRepository<PaymentPlan, Guid> _entityRepository;
        private readonly IRepository<Company, int> _companyRepository;
        private readonly IRepository<Account, long> _accountRepository;
        private readonly IPaymentPlanManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public PaymentPlanAppService(
        IRepository<PaymentPlan, Guid> entityRepository
            , IRepository<Company, int> companyRepository
            , IRepository<Account, long> accountRepository
        , IPaymentPlanManager entityManager
        )
        {
            _accountRepository = accountRepository;
            _companyRepository = companyRepository;
            _entityRepository = entityRepository;
            _entityManager = entityManager;
        }


        /// <summary>
        /// 获取PaymentPlan的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<PagedResultDto<PaymentPlanListDto>> GetPagedAsync(GetPaymentPlansInput input)
        {

            var query = _entityRepository.GetAll()
                .WhereIf(input.ProjectId.HasValue, aa => aa.ProjectId == input.ProjectId.Value);
            // TODO:根据传入的参数添加过滤条件


            var count = await query.CountAsync();

            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .OrderByDescending(aa => aa.PaymentTime)
                    //.PageBy(input)
                    .ToListAsync();

            // var entityListDtos = ObjectMapper.Map<List<PaymentPlanListDto>>(entityList);
            var entityListDtos = entityList.MapTo<List<PaymentPlanListDto>>();

            return new PagedResultDto<PaymentPlanListDto>(count, entityListDtos);
        }


        /// <summary>
        /// 通过指定id获取PaymentPlanListDto信息
        /// </summary>

        public async Task<PaymentPlanListDto> GetByIdAsync(EntityDto<Guid> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);

            return entity.MapTo<PaymentPlanListDto>();
        }

        /// <summary>
        /// 获取编辑 PaymentPlan
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<GetPaymentPlanForEditOutput> GetForEditAsync(NullableIdDto<Guid> input)
        {
            var output = new GetPaymentPlanForEditOutput();
            PaymentPlanEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                editDto = entity.MapTo<PaymentPlanEditDto>();

                //paymentPlanEditDto = ObjectMapper.Map<List<paymentPlanEditDto>>(entity);
            }
            else
            {
                editDto = new PaymentPlanEditDto();
            }

            output.PaymentPlan = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改PaymentPlan的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task CreateOrUpdateAsync(CreateOrUpdatePaymentPlanInput input)
        {

            if (input.PaymentPlan.Id.HasValue)
            {
                await UpdateAsync(input.PaymentPlan);
            }
            else
            {
                await CreateAsync(input.PaymentPlan);
            }
        }


        /// <summary>
        /// 新增PaymentPlan
        /// </summary>

        protected virtual async Task<PaymentPlanEditDto> CreateAsync(PaymentPlanEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <PaymentPlan>(input);
            var entity = input.MapTo<PaymentPlan>();
            entity = await _entityRepository.InsertAsync(entity);
            //TODO:新增后关联操作
            //已回款更新公司账户
            if (entity.Status == PaymentPlanStatusEnum.已回款)
            {
                Company company = await _companyRepository.GetAll().FirstOrDefaultAsync();
                Account account = new Account()
                {
                    CompanyId = company.Id,
                    Type = AccountType.入账,
                    Initial = company.Balance,
                    Amount = input.Amount,
                    Ending = company.Balance + input.Amount,
                    Desc = input.Desc,
                    RefId = entity.Id.ToString(),
                    CreationTime = DateTime.Now
                };
                //添加账户流水
                await _accountRepository.InsertAsync(account);

                company.Balance += input.Amount;
                //更新公司账户余额
                await _companyRepository.UpdateAsync(company);
            }

            return entity.MapTo<PaymentPlanEditDto>();
        }

        /// <summary>
        /// 编辑PaymentPlan
        /// </summary>

        protected virtual async Task UpdateAsync(PaymentPlanEditDto input)
        {

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            //TODO:更新前的逻辑判断，是否允许更新
            //已回款更新公司账户
            Account account = new Account();
            if (input.Status == PaymentPlanStatusEnum.已回款)
            {
                Company company = await _companyRepository.GetAll().FirstOrDefaultAsync();
                account = await _accountRepository.GetAll().FirstOrDefaultAsync(aa => aa.RefId == input.Id.Value.ToString());
                if (account != null)
                {
                    account.Initial = company.Balance;
                    account.Amount = input.Amount;
                    account.Ending = company.Balance + input.Amount;
                    account.Desc = input.Desc;
                    //更新账户流水
                    await _accountRepository.UpdateAsync(account);
                    input.Amount -= entity.Amount;
                }
                else
                { 
                account.CompanyId = company.Id;
                account.Type = AccountType.入账;
                account.Initial = company.Balance;
                account.Amount = input.Amount;
                account.Ending = company.Balance + input.Amount;
                account.Desc = input.Desc;
                account.RefId = input.Id.Value.ToString();
                account.CreationTime = DateTime.Now;
                //添加账户流水
                await _accountRepository.InsertAsync(account);
                }
                company.Balance += input.Amount;
                //更新公司账户余额
                await _companyRepository.UpdateAsync(company);
            }
            else
            {
                if (entity.Status != input.Status)
                {
                    account = await _accountRepository.GetAll().FirstOrDefaultAsync(aa => aa.RefId == input.Id.Value.ToString());
                    //删除公司账户流水
                    await _accountRepository.DeleteAsync(account.Id);
                    Company company = await _companyRepository.GetAll().FirstOrDefaultAsync();
                    company.Balance -= input.Amount;
                    //更新公司账户余额
                    await _companyRepository.UpdateAsync(company);
                }
            }
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _entityRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除PaymentPlan信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task DeleteAsync(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            var item = await _entityRepository.GetAsync(input.Id);
            if (item.Status == PaymentPlanStatusEnum.已回款)
            {
                Account account = await _accountRepository.GetAll().FirstOrDefaultAsync(aa => aa.RefId == input.Id.ToString());
                //删除公司账户流水
                await _accountRepository.DeleteAsync(account.Id);
                Company company = await _companyRepository.GetAll().FirstOrDefaultAsync();
                company.Balance -= item.Amount;
                //更新公司账户余额
                await _companyRepository.UpdateAsync(company);
            }

            await _entityRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除PaymentPlan的方法
        /// </summary>

        public async Task BatchDeleteAsync(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }


        /// <summary>
        /// 导出PaymentPlan为excel表,等待开发。
        /// </summary>
        /// <returns></returns>
        //public async Task<FileDto> GetToExcel()
        //{
        //	var users = await UserManager.Users.ToListAsync();
        //	var userListDtos = ObjectMapper.Map<List<UserListDto>>(users);
        //	await FillRoleNames(userListDtos);
        //	return _userListExcelExporter.ExportToFile(userListDtos);
        //}

    }
}


