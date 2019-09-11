using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using HC.AbpCore.Contracts;
using HC.AbpCore.Contracts.ContractDetails;
using HC.AbpCore.Invoices;
using HC.AbpCore.Invoices.InvoiceDetails;
using HC.AbpCore.Products;
using HC.AbpCore.Projects;
using HC.AbpCore.Reimburses;
using HC.AbpCore.Reports.ProjectProfitReport.Dtos;
using HC.AbpCore.TimeSheets;
using Microsoft.EntityFrameworkCore;
using static HC.AbpCore.Contracts.ContractEnum;

namespace HC.AbpCore.Reports.ProjectProfitReport
{
    public class ProjectProfitReportAppService : IProjectProfitReportApplicationService
    {
        private readonly IRepository<Project, Guid> _projectyRepository;
        private readonly IRepository<Contract, Guid> _contractRepository;
        private readonly IRepository<ContractDetail, Guid> _contractDetailRepository;
        private readonly IRepository<Product, int> _productRepository;
        private readonly IRepository<Invoice, Guid> _invoiceRepository;
        private readonly IRepository<InvoiceDetail, Guid> _invoiceDetailRepository;
        private readonly IRepository<Reimburse, Guid> _reimburseRepository;
        private readonly IRepository<TimeSheet, Guid> _timeSheetRepository;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public ProjectProfitReportAppService(
        IRepository<Project, Guid> projectyRepository
         , IRepository<Contract, Guid> contractRepository
         , IRepository<ContractDetail, Guid> contractDetailRepository
         , IRepository<Product, int> productRepository
         , IRepository<Invoice, Guid> invoiceRepository
         , IRepository<InvoiceDetail, Guid> invoiceDetailRepository
            , IRepository<Reimburse, Guid> reimburseRepository
            , IRepository<TimeSheet, Guid> timeSheetRepository
        )
        {
            _invoiceRepository = invoiceRepository;
            _projectyRepository = projectyRepository;
            _contractRepository = contractRepository;
            _contractDetailRepository = contractDetailRepository;
            _productRepository = productRepository;
            _invoiceDetailRepository = invoiceDetailRepository;
            _reimburseRepository = reimburseRepository;
            _timeSheetRepository = timeSheetRepository;
        }

        public async Task<List<ProjectProfitListDto>> GetProjectProfitByIdAsync(GetProjectProfitInput input)
        {
            List<ProjectProfitListDto> projectProfitListDtos = new List<ProjectProfitListDto>();
            var project = await _projectyRepository.GetAsync(input.ProjectId);
            var contract = await _contractRepository.FirstOrDefaultAsync(aa => aa.Type == ContractTypeEnum.销项 && aa.RefId == input.ProjectId);

            var products = _productRepository.GetAll().Select(aa => new { aa.Id, aa.Price, aa.TaxRate, aa.Name });
            var invoices = _invoiceRepository.GetAll().Where(aa => aa.Type == InvoiceTypeEnum.销项 && aa.RefId == input.ProjectId);
            var invoiceDetails = _invoiceDetailRepository.GetAll().Select(aa => new { aa.RefId, aa.TaxAmount, aa.InvoiceId });
            var reimburseAmount = await _reimburseRepository.GetAll().Where(aa => aa.ProjectId == project.Id && aa.Type == ReimburseTypeEnum.项目型报销 && aa.Status == ReimburseStatusEnum.审批通过).SumAsync(aa => aa.Amount);
            var timeSheetHour = await _timeSheetRepository.GetAll().Where(aa => aa.ProjectId == project.Id && aa.Status == TimeSheetStatusEnum.审批通过)
                .SumAsync(aa => aa.Hour ?? 0);
            List<ProjectCost> projectCosts = new List<ProjectCost>();
            List<Taxation> taxations = new List<Taxation>();
            List<SalesDetail> salesDetails = new List<SalesDetail>();
            if (contract != null)
            {
                var contractDetails = _contractDetailRepository.GetAll().Where(aa => aa.ContractId == contract.Id).Select(aa => new { aa.Id, aa.Price, aa.Num, aa.ProductId });
                invoiceDetails = from invoice in invoices
                                 join invoiceDetail in invoiceDetails on invoice.Id equals invoiceDetail.InvoiceId into aa
                                 from cc in aa.DefaultIfEmpty()
                                 select new
                                 {
                                     cc.RefId,
                                     cc.TaxAmount,
                                     cc.InvoiceId
                                 };


                taxations = await (from contractDetail in contractDetails
                                   join product in products on contractDetail.ProductId equals product.Id
                                   join invoiceDetail in invoiceDetails on contractDetail.Id equals invoiceDetail.RefId into c
                                   from d in c.DefaultIfEmpty()
                                   group new { contractDetail, product, d } by contractDetail.Id into ff
                                   select new Taxation
                                   {
                                       SaleTaxAmount = ff.Sum(aa => aa.d == null ? 0 : aa.d.TaxAmount),
                                       IncomeTaxAmount = ff.Max(aa => aa.contractDetail.Num) * ff.Max(aa => aa.product.Price) * Convert.ToDecimal(ff.Select(aa => aa.product.TaxRate).FirstOrDefault()
                                       .Substring(0, ff.Select(aa => aa.product.TaxRate).FirstOrDefault().IndexOf("%")))/100,
                                       VATPayable = ff.Sum(aa => aa.d == null ? 0 : aa.d.TaxAmount) - Math.Round(ff.Select(aa => aa.contractDetail.Num).FirstOrDefault() * ff.Select(aa => aa.product.Price).FirstOrDefault()
                                       * Convert.ToDecimal(ff.Select(aa => aa.product.TaxRate).FirstOrDefault().Substring(0, ff.Select(aa => aa.product.TaxRate).FirstOrDefault().IndexOf("%")))/100 ?? 0, 2),
                                       CityEducationTax = Math.Round((ff.Sum(aa => aa.d == null ? 0 : aa.d.TaxAmount) - Math.Round(ff.Select(aa => aa.contractDetail.Num).First()
                                       * ff.Select(aa => aa.product.Price).First() * Convert.ToDecimal(ff.Select(aa => aa.product.TaxRate).FirstOrDefault().Substring(0,
                                       ff.Select(aa => aa.product.TaxRate).FirstOrDefault().IndexOf("%")))/100 ?? 0, 2))
                                       * Convert.ToDecimal(0.07 + 0.02 + 0.03) + contract.Amount * Convert.ToDecimal(0.001) ?? 0, 2),
                                       CorporateIncomeTax = Math.Round((contract.Amount - ff.Select(aa => aa.contractDetail.Num).First() * ff.Select(aa => aa.product.Price).First() -
                                       (ff.Sum(aa => aa.d == null ? 0 : aa.d.TaxAmount) - Math.Round(ff.Select(aa => aa.contractDetail.Num).First() * ff.Select(aa => aa.product.Price).First()
                                       * Convert.ToDecimal(ff.Select(aa => aa.product.TaxRate).FirstOrDefault().Substring(0,
                                       ff.Select(aa => aa.product.TaxRate).FirstOrDefault().IndexOf("%")))/100 ?? 0, 2)) -
                                       Math.Round((ff.Sum(aa => aa.d == null ? 0 : aa.d.TaxAmount) - Math.Round(ff.Select(aa => aa.contractDetail.Num).First() * ff.Select(aa => aa.product.Price).First()
                                       * Convert.ToDecimal(ff.Select(aa => aa.product.TaxRate).FirstOrDefault().Substring(0,
                                       ff.Select(aa => aa.product.TaxRate).FirstOrDefault().IndexOf("%")))/100 ?? 0, 2))
                                       * Convert.ToDecimal(0.07 + 0.02 + 0.03) + contract.Amount * Convert.ToDecimal(0.001) ?? 0, 2)) * Convert.ToDecimal(0.25) ?? 0, 2),
                                       IndividualIncomeTax = Math.Round((contract.Amount - ff.Select(aa => aa.contractDetail.Num).First() * ff.Select(aa => aa.product.Price).First() -
                                       (ff.Sum(aa => aa.d == null ? 0 : aa.d.TaxAmount) - Math.Round(ff.Select(aa => aa.contractDetail.Num).First() * ff.Select(aa => aa.product.Price).First()
                                       * Convert.ToDecimal(ff.Select(aa => aa.product.TaxRate).FirstOrDefault().Substring(0,
                                       ff.Select(aa => aa.product.TaxRate).FirstOrDefault().IndexOf("%"))) / 100 ?? 0, 2)) -
                                       Math.Round((ff.Sum(aa => aa.d == null ? 0 : aa.d.TaxAmount) - Math.Round(ff.Select(aa => aa.contractDetail.Num).First() * ff.Select(aa => aa.product.Price).First()
                                       * Convert.ToDecimal(ff.Select(aa => aa.product.TaxRate).FirstOrDefault().Substring(0,
                                       ff.Select(aa => aa.product.TaxRate).FirstOrDefault().IndexOf("%"))) / 100 ?? 0, 2))
                                       * Convert.ToDecimal(0.07 + 0.02 + 0.03) + contract.Amount * Convert.ToDecimal(0.001) ?? 0, 2) - Math.Round((contract.Amount - ff.Select(aa => aa.contractDetail.Num).First() * ff.Select(aa => aa.product.Price).First() -
                                       (ff.Sum(aa => aa.d == null ? 0 : aa.d.TaxAmount) - Math.Round(ff.Select(aa => aa.contractDetail.Num).First() * ff.Select(aa => aa.product.Price).First()
                                       * Convert.ToDecimal(ff.Select(aa => aa.product.TaxRate).FirstOrDefault().Substring(0,
                                       ff.Select(aa => aa.product.TaxRate).FirstOrDefault().IndexOf("%"))) / 100 ?? 0, 2)) -
                                       Math.Round((ff.Sum(aa => aa.d == null ? 0 : aa.d.TaxAmount) - Math.Round(ff.Select(aa => aa.contractDetail.Num).First() * ff.Select(aa => aa.product.Price).First()
                                       * Convert.ToDecimal(ff.Select(aa => aa.product.TaxRate).FirstOrDefault().Substring(0,
                                       ff.Select(aa => aa.product.TaxRate).FirstOrDefault().IndexOf("%"))) / 100 ?? 0, 2))
                                       * Convert.ToDecimal(0.07 + 0.02 + 0.03) + contract.Amount * Convert.ToDecimal(0.001) ?? 0, 2)) * Convert.ToDecimal(0.25) ?? 0, 2)) * Convert.ToDecimal(0.2) ?? 0, 2)
                                   }).AsNoTracking().ToListAsync();
                projectCosts = await (from contractDetail in contractDetails
                                      join product in products on contractDetail.ProductId equals product.Id into a
                                      from b in a.DefaultIfEmpty()
                                      select new ProjectCost()
                                      {
                                          DeviceName = b.Name,
                                          Price = b.Price,
                                          Num = contractDetail.Num
                                      }).AsNoTracking().ToListAsync();


                salesDetails = await _contractDetailRepository.GetAll().Where(aa => aa.ContractId == contract.Id)
                    .Select(aa => new SalesDetail() { Price = aa.Price, Num = aa.Num })
                    .AsNoTracking().ToListAsync();

            }
            projectCosts.Add(new ProjectCost() { DeviceName = "工时成本", Num = 1, Price = Math.Round(Convert.ToDecimal(timeSheetHour * (30000.00 / 21 / 8)), 2) });
            projectCosts.Add(new ProjectCost() { DeviceName = "报销金额", Num = 1, Price = reimburseAmount });

            int index = 0;
            foreach (var model in projectCosts)
            {
                ProjectProfitListDto item = new ProjectProfitListDto();
                item.CostPrice = model.Price;
                item.CostDeviceName = model.DeviceName;
                item.CostNum = model.Num;
                item.CostAmount = model.Num * model.Price;
                if (index == 0)
                {
                    item.Id = project.Id;
                    item.Name = project.Name + "(" + project.ProjectCode + ")";
                    item.CreationTime = project.CreationTime;
                }
                if (contract != null)
                {
                    if (index == 0)
                    {
                        item.ContractAmount = contract.Amount ?? 0;
                        item.Profit = salesDetails.Sum(aa => aa.Num * aa.Price) - projectCosts.Sum(aa => aa.Num * aa.Price) - taxations.Sum(aa => aa.VATPayable) -
                            taxations.Sum(aa => aa.CityEducationTax) - taxations.Sum(aa => aa.CorporateIncomeTax) - taxations.Sum(aa => aa.IndividualIncomeTax);
                        item.ProfitMargin = Math.Round(item.Profit.Value / (item.ContractAmount.Value == 0 ? 1 : item.ContractAmount.Value) / 100, 2);
                    }
                    if (index <= salesDetails.Count - 1)
                    {
                        item.SalesNum = salesDetails[index].Num;
                        item.SalesPrice = salesDetails[index].Price;
                        item.SalesAmount = salesDetails[index].Num * salesDetails[index].Price;
                        item.SaleTaxAmount = taxations[index].SaleTaxAmount;
                        item.IncomeTaxAmount = taxations[index].IncomeTaxAmount;
                        item.VATPayable = taxations[index].VATPayable;
                        item.CityEducationTax = taxations[index].CityEducationTax;
                        item.CorporateIncomeTax = taxations[index].CorporateIncomeTax;
                        item.IndividualIncomeTax = taxations[index].IndividualIncomeTax;
                    }
                }
                projectProfitListDtos.Add(item);
                index += 1;
            }
            //item.Id = project.Id;
            //item.Name = project.Name + "(" + project.ProjectCode + ")";
            //item.ContractAmount = contract.Amount;
            //item.CreationTime = project.CreationTime;
            //item.SalesDetails = await _contractDetailRepository.GetAll().Where(aa => aa.ContractId == contract.Id)
            //    .Select(aa => new Dtos.SalesDetails() { Price = aa.Price, Num = aa.Num })
            //    .AsNoTracking().ToListAsync();
            //item.ProjectCost = await projectCosts.AsNoTracking().ToListAsync();
            //item.Taxation = await taxations.AsNoTracking().ToListAsync();
            ////item.Taxation = taxations;
            //item.Profit = item.SalesDetails.Sum(aa => aa.Num * aa.Price) - item.ProjectCost.Sum(aa => aa.Num * aa.Price) - item.Taxation.Sum(aa => aa.VATPayable) -
            //    item.Taxation.Sum(aa => aa.CityEducationTax) - item.Taxation.Sum(aa => aa.CorporateIncomeTax) - item.Taxation.Sum(aa => aa.IndividualIncomeTax);
            //item.ProfitMargin = Math.Round(item.Profit / item.ContractAmount ?? 0, 2);
            projectProfitListDtos.Add(new ProjectProfitListDto()
            {
                Name = "合计",
                ContractAmount = contract==null?0:contract.Amount,
                SalesAmount = projectProfitListDtos.Sum(aa => aa.SalesAmount),
                CostAmount = projectProfitListDtos.Sum(aa => aa.CostAmount)
            ,
                SaleTaxAmount = projectProfitListDtos.Sum(aa => aa.SaleTaxAmount),
                IncomeTaxAmount = projectProfitListDtos.Sum(aa => aa.IncomeTaxAmount)
            ,
                VATPayable = projectProfitListDtos.Sum(aa => aa.VATPayable),
                CityEducationTax = projectProfitListDtos.Sum(aa => aa.CityEducationTax)
            ,
                CorporateIncomeTax = projectProfitListDtos.Sum(aa => aa.CorporateIncomeTax)
            ,
                IndividualIncomeTax = projectProfitListDtos.Sum(aa => aa.IndividualIncomeTax)
            });
            return projectProfitListDtos;
        }
    }
}
