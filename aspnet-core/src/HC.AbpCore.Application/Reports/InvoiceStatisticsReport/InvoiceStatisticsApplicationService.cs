
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


using HC.AbpCore.Reports.InvoiceStatisticsReport;
using HC.AbpCore.Reports.InvoiceStatisticsReport.Dtos;
using HC.AbpCore.Projects;
using HC.AbpCore.Invoices;
using HC.AbpCore.Invoices.InvoiceDetails;
using HC.AbpCore.Purchases.PurchaseDetails;
using HC.AbpCore.Suppliers;

namespace HC.AbpCore.Reports.InvoiceStatisticsReport
{
    /// <summary>
    /// InvoiceStatistics应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class InvoiceStatisticsAppService : AbpCoreAppServiceBase, IInvoiceStatisticsAppService
    {
        private readonly IRepository<Project, Guid> _projectRepository;
        private readonly IRepository<Invoice, Guid> _invoiceRepository;
        private readonly IRepository<InvoiceDetail, Guid> _invoiceDetailRepository;
        private readonly IRepository<PurchaseDetail, Guid> _purchaseDetailRepository;
        private readonly IRepository<Supplier, int> _supplierRepository;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public InvoiceStatisticsAppService(
          IRepository<Project, Guid> projectRepository,
          IRepository<Invoice, Guid> invoiceRepository,
          IRepository<InvoiceDetail, Guid> invoiceDetailRepository,
          IRepository<PurchaseDetail, Guid> purchaseDetailRepository,
          IRepository<Supplier, int> supplierRepository
        )
        {
            _projectRepository = projectRepository;
            _invoiceRepository = invoiceRepository;
            _invoiceDetailRepository = invoiceDetailRepository;
            _purchaseDetailRepository = purchaseDetailRepository;
            _supplierRepository = supplierRepository;
        }


        /// <summary>
        /// 获取发票分类统计
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
		 
        public async Task<PagedResultDto<InvoiceStatisticsListDto>> GetInvoiceStatisticsAsync(GetInvoiceStatisticssInput input)
		{

            var query = _invoiceDetailRepository.GetAll();
            var invoices = _invoiceRepository.GetAll().Where(aa => aa.SubmitDate >= input.StartSubmitDate && aa.SubmitDate < input.EndSubmitDate)
                .WhereIf(input.Type.HasValue, aa => aa.Type == input.Type);
            var projects = _projectRepository.GetAll();
            var purchaseDetails = _purchaseDetailRepository.GetAll();
            var suppliers = _supplierRepository.GetAll();
            // TODO:根据传入的参数添加过滤条件
            IQueryable<InvoiceStatisticsListDto> items = null;
            if (input.Type == InvoiceTypeEnum.销项)
            {
                items = from item in query
                            join invoice in invoices on item.InvoiceId equals invoice.Id
                            join project in projects on invoice.RefId equals project.Id
                            select (new InvoiceStatisticsListDto()
                            {
                                Id = item.Id,
                                Type = invoice.Type,
                                Code = invoice.Code,
                                SubmitDate = invoice.SubmitDate,
                                InvoiceUnit = project.Name,
                                Name = item.Name,
                                Specification = item.Specification,
                                Unit = item.Unit,
                                Num = item.Num,
                                Price = item.Price,
                                Amount = item.Amount,
                                TaxRate = item.TaxRate,
                                TaxAmount = item.TaxAmount,
                                TotalAmount = item.TotalAmount
                            });
            }
            else
            {
                items = from item in query
                        join invoice in invoices on item.InvoiceId equals invoice.Id
                        join purchaseDetail in purchaseDetails on item.RefId equals purchaseDetail.Id
                        join supplier in suppliers on purchaseDetail.SupplierId equals supplier.Id
                        select (new InvoiceStatisticsListDto()
                        {
                            Id = item.Id,
                            Type = invoice.Type,
                            Code = invoice.Code,
                            SubmitDate = invoice.SubmitDate,
                            InvoiceUnit = supplier.Name,
                            Name = item.Name,
                            Specification = item.Specification,
                            Unit = item.Unit,
                            Num = item.Num,
                            Price = item.Price,
                            Amount = item.Amount,
                            TaxRate = item.TaxRate,
                            TaxAmount = item.TaxAmount,
                            TotalAmount = item.TotalAmount
                        });
            }
            var count = await items.CountAsync();
			var entityList = await items
                    .OrderBy(input.Sorting).AsNoTracking()
					.PageBy(input)
					.ToListAsync();

			// var entityListDtos = ObjectMapper.Map<List<InvoiceStatisticsListDto>>(entityList);
			//var entityListDtos =entityList.MapTo<List<InvoiceStatisticsListDto>>();

			return new PagedResultDto<InvoiceStatisticsListDto>(count, entityList);
		}


    }
}


