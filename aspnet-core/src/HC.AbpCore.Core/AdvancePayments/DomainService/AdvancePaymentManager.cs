

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
using HC.AbpCore.AdvancePayments;
using HC.AbpCore.Companys.Accounts;
using HC.AbpCore.Companys;

namespace HC.AbpCore.AdvancePayments.DomainService
{
    /// <summary>
    /// AdvancePayment领域层的业务管理
    ///</summary>
    public class AdvancePaymentManager :AbpCoreDomainServiceBase, IAdvancePaymentManager
    {
		
		private readonly IRepository<AdvancePayment,Guid> _repository;
        private readonly IRepository<Account, long> _accountRepository;
        private readonly IRepository<Company, int> _companyRepository;

        /// <summary>
        /// AdvancePayment的构造方法
        ///</summary>
        public AdvancePaymentManager(
			IRepository<AdvancePayment, Guid> repository
            , IRepository<Company, int> companyRepository
            , IRepository<Account, long> accountRepository
        )
		{
            _companyRepository = companyRepository;
            _accountRepository = accountRepository;
            _repository =  repository;
		}


        /// <summary>
        /// 初始化
        ///</summary>
        public void InitAdvancePayment()
		{
			throw new NotImplementedException();
		}

        /// <summary>
        /// 添加AdvancePayment并更新公司流水的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<AdvancePayment> CreateAndmodifyAccountAsync(AdvancePayment input)
        {
            var advancePayment = await _repository.InsertAsync(input);

            //已付款则更新公司流水
            if (advancePayment.Status == AdvancePaymentStatusEnum.已付款)
            {
                var company = await _companyRepository.GetAll().FirstOrDefaultAsync();
                Account account = new Account();
                account.CompanyId = company.Id;
                account.Type = AccountType.出账;
                account.Initial = company.Balance;
                account.Amount = advancePayment.Amount;
                account.Ending = company.Balance - advancePayment.Amount;
                account.RefId = advancePayment.Id.ToString();
                await _accountRepository.InsertAsync(account);
                //更新公司余额信息
                company.Balance = account.Ending;
                await _companyRepository.UpdateAsync(company);
            }
            return advancePayment;
        }

        /// <summary>
        /// 删除AdvancePayment并且删除账户流水信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeleteAndDeleteAccountAsync(Guid input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            //删除公司流水
            var company = await _companyRepository.GetAll().FirstOrDefaultAsync();
            var account = await _accountRepository.FirstOrDefaultAsync(aa => aa.Type == AccountType.出账 && aa.RefId == input.ToString());
            //更新公司账户
            company.Balance += account.Amount;
            await _companyRepository.UpdateAsync(company);
            await _accountRepository.DeleteAsync(account.Id);
            await _repository.DeleteAsync(input);
        }

        /// <summary>
        /// 修改AdvancePayment并更新公司流水的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<AdvancePayment> UpdateAndmodifyAccountAsync(AdvancePayment input)
        {
            var advancePayment = await _repository.UpdateAsync(input);

            //已付款则更新公司流水
            var company = await _companyRepository.GetAll().FirstOrDefaultAsync();
            var account = await _accountRepository.FirstOrDefaultAsync(aa => aa.Type == AccountType.出账 && aa.RefId == advancePayment.Id.ToString());
            if (advancePayment.Status == AdvancePaymentStatusEnum.已付款)
            {
                //账户流水不等于空
                if (account != null)
                {
                    //发生金额不等于付款金额更新账户流水同公司余额
                    if (account.Amount != advancePayment.Amount)
                    {
                        company.Balance = company.Balance + account.Amount;
                        account.Initial = company.Balance;
                        account.Amount = advancePayment.Amount;
                        account.Ending = company.Balance - advancePayment.Amount;
                        await _accountRepository.UpdateAsync(account);
                        //更新公司余额信息
                        company.Balance = account.Ending;
                        await _companyRepository.UpdateAsync(company);
                    }
                }
                else //账户流水等于空则新增账户流水并修改公司余额
                {
                    Account accountNew = new Account();
                    accountNew.CompanyId = company.Id;
                    accountNew.Type = AccountType.出账;
                    accountNew.Initial = company.Balance;
                    accountNew.Amount = advancePayment.Amount;
                    accountNew.Ending = company.Balance - advancePayment.Amount;
                    accountNew.RefId = advancePayment.Id.ToString();
                    await _accountRepository.InsertAsync(accountNew);
                    //更新公司余额信息
                    company.Balance = accountNew.Ending;
                    await _companyRepository.UpdateAsync(company);
                }
            }
            else
            {
                if (account != null)
                {
                    //更新公司余额信息
                    company.Balance += account.Amount;
                    await _companyRepository.UpdateAsync(company);
                    //删除账户流水
                    await _accountRepository.DeleteAsync(account.Id);
                }
            }
            return advancePayment;
        }

        // TODO:编写领域业务代码







    }
}
