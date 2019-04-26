

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using HC.AbpCore.Invoices.InvoiceDetails;


namespace HC.AbpCore.Invoices.InvoiceDetails.DomainService
{
    public interface IInvoiceDetailManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitInvoiceDetail();


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<InvoiceDetail> CreateAsync(InvoiceDetail input);


        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateAsync(InvoiceDetail input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid Id);




    }
}
