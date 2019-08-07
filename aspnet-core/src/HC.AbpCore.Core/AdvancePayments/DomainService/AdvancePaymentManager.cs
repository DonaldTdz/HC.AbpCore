

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
using HC.AbpCore.DingTalk;
using HC.AbpCore.Purchases;
using HC.AbpCore.DingTalk.Employees;
using HC.AbpCore.Messages;
using HC.AbpCore.Common;
using Senparc.CO2NET.Helpers;
using System.Text;
using Senparc.CO2NET.HttpUtility;

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
        private readonly IRepository<Purchase, Guid> _purchaseRepository;
        private readonly IRepository<Employee, string> _employeeRepository;
        private readonly IRepository<Message, Guid> _messageRepository;

        /// <summary>
        /// AdvancePayment的构造方法
        ///</summary>
        public AdvancePaymentManager(
			IRepository<AdvancePayment, Guid> repository
            , IRepository<Company, int> companyRepository
            , IRepository<Account, long> accountRepository
            , IRepository<Purchase, Guid> purchaseRepository
            , IRepository<Employee, string> employeeRepository
            , IRepository<Message, Guid> messageRepository
        )
		{
            _messageRepository = messageRepository;
            _employeeRepository = employeeRepository;
            _purchaseRepository = purchaseRepository;
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
            if (account != null)
            {
                company.Balance += account.Amount;
                await _companyRepository.UpdateAsync(company);
                await _accountRepository.DeleteAsync(account.Id);
            }
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

        /// <summary>
        /// 付款提醒
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="dingDingAppConfig"></param>
        /// <returns></returns>
        public async Task PaymentRemindAsync(string accessToken, DingDingAppConfig dingDingAppConfig)
        {
            var purchases = _purchaseRepository.GetAll();
            var query = _repository.GetAll()
                .Where(aa => aa.Status == AdvancePaymentStatusEnum.未付款)
                .Where(aa => aa.PaymentTime <= DateTime.Now.AddDays(7) && aa.PaymentTime >= DateTime.Now);
            var advancePayment = from payment in query
                               join purchase in purchases on payment.PurchaseId equals purchase.Id
                               select new
                               {
                                   PurchaseCode = purchase.Code,
                                   purchase.EmployeeId,
                                   payment.PlanTime
                               };
            var items = await advancePayment.AsNoTracking().ToListAsync();
            var employeeIdList = await _employeeRepository.GetAll().Where(aa => aa.IsLeaderInDepts == "key:73354253value:True").Select(aa => aa.Id)
                .Distinct().AsNoTracking().ToListAsync();
            var url = string.Format("https://oapi.dingtalk.com/topapi/message/corpconversation/asyncsend_v2?access_token={0}", accessToken);
            foreach (var item in items)
            {
                employeeIdList.Add(item.EmployeeId);
                foreach (var employeeId in employeeIdList)
                {
                    Message message = new Message();
                    message.Content = string.Format("您好! 采购:{0}计划付款时间即将达到，计划付款时间为:{1}", item.PurchaseCode, item.PlanTime.ToString("yyyy-MM-dd"));
                    message.SendTime = DateTime.Now;
                    message.Type = MessageTypeEnum.付款提醒;
                    message.IsRead = false;
                    message.EmployeeId = employeeId;
                    //新增到消息中心
                    var messageId = await _messageRepository.InsertAndGetIdAsync(message);

                    DingMsgs dingMsgs = new DingMsgs();
                    dingMsgs.userid_list = employeeId;
                    dingMsgs.to_all_user = false;
                    dingMsgs.agent_id = dingDingAppConfig.AgentID;
                    dingMsgs.msg.msgtype = "link";
                    dingMsgs.msg.link.title = "付款提醒";
                    dingMsgs.msg.link.text = string.Format("所属采购:{0}，点击查看详情", item.PurchaseCode, item.PlanTime.ToString("yyyy-MM-dd"));
                    dingMsgs.msg.link.picUrl = "@lALPDeC2uQ_7MOHMgMyA";
                    dingMsgs.msg.link.messageUrl = "eapp://page/messages/detail-messages/detail-messages?id=" + messageId;
                    var jsonString = SerializerHelper.GetJsonString(dingMsgs, null);
                    MessageResponseResult response = new MessageResponseResult();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        var bytes = Encoding.UTF8.GetBytes(jsonString);
                        ms.Write(bytes, 0, bytes.Length);
                        ms.Seek(0, SeekOrigin.Begin);
                        response = Post.PostGetJson<MessageResponseResult>(url, null, ms);
                    };
                    //发送失败则自动删除消息中心对应数据
                    if (response.errcode != 0)
                    {
                        await _messageRepository.DeleteAsync(messageId);
                    }
                }
            }
        }

        // TODO:编写领域业务代码







    }
}
