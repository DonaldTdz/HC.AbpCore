

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
using HC.AbpCore.Invoices.InvoiceDetails;
using Abp.AutoMapper;

namespace HC.AbpCore.Invoices.InvoiceDetails.DomainService
{
    /// <summary>
    /// InvoiceDetail领域层的业务管理
    ///</summary>
    public class InvoiceDetailManager : AbpCoreDomainServiceBase, IInvoiceDetailManager
    {

        private readonly IRepository<InvoiceDetail, Guid> _repository;
        private readonly IRepository<Invoice, Guid> _invoicerepository;

        /// <summary>
        /// InvoiceDetail的构造方法
        ///</summary>
        public InvoiceDetailManager(
            IRepository<InvoiceDetail, Guid> repository
            , IRepository<Invoice, Guid> invoicerepository
        )
        {
            _invoicerepository = invoicerepository;
            _repository = repository;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<InvoiceDetail> CreateAsync(InvoiceDetail input)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            //修改发票金额
            if (input.InvoiceId.HasValue)
            {
                var invoice = await _invoicerepository.GetAsync(input.InvoiceId.Value);
                decimal detailAmount=0;
                if (input.Num.HasValue && input.Price.HasValue)
                    detailAmount = input.Num.Value * input.Price.Value;
                invoice.Amount += detailAmount;
                await _invoicerepository.UpdateAsync(invoice);
            }
            input = await _repository.InsertAsync(input);
            return input;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Guid Id)
        {
            var entity = await _repository.GetAsync(Id);
            if (entity.InvoiceId.HasValue)
            {
                var contract = await _invoicerepository.GetAsync(entity.InvoiceId.Value);
                decimal detailAmount = 0;
                if (entity.Num.HasValue && entity.Price.HasValue)
                    detailAmount = entity.Num.Value * entity.Price.Value;
                contract.Amount -= detailAmount;
                await _invoicerepository.UpdateAsync(contract);
            }
            await _repository.DeleteAsync(Id);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateAsync(InvoiceDetail input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _repository.GetAsync(input.Id);
            //修改合同金额
            if (input.InvoiceId.HasValue)
            {
                var invoice = await _invoicerepository.GetAsync(input.InvoiceId.Value);
                decimal detailAmount = 0;
                if (input.Num.HasValue && input.Price.HasValue)
                    detailAmount = input.Num.Value * input.Price.Value;
                invoice.Amount += detailAmount-entity.Num.Value*entity.Price.Value;
                await _invoicerepository.UpdateAsync(invoice);
            }
            entity.Price = input.Price;
            entity.Num = input.Num;
            entity.Name = input.Name;
            //entity.RefId = input.RefId;
            entity.Specification = input.Specification;
            entity.TaxRate = input.TaxRate;
            //entity.Unit = input.Unit;
            //ObjectMapper.Map(input, entity);
            await _repository.UpdateAsync(entity);
        }


        /// <summary>
        /// 初始化
        ///</summary>
        public void InitInvoiceDetail()
        {
            throw new NotImplementedException();
        }

        // TODO:编写领域业务代码







    }
}
