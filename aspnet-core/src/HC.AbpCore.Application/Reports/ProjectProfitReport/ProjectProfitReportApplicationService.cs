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
using HC.AbpCore.Reports.ProjectProfitReport.Dtos;
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
        )
        {
            _invoiceRepository = invoiceRepository;
            _projectyRepository = projectyRepository;
            _contractRepository = contractRepository;
            _contractDetailRepository = contractDetailRepository;
            _productRepository = productRepository;
            _invoiceDetailRepository = invoiceDetailRepository;
        }

        public async Task<List<ProjectProfitListDto>> GetProjectProfitByIdAsync(GetProjectProfitInput input)
        {
            List<ProjectProfitListDto> projectProfitListDtos = new List<ProjectProfitListDto>();
            var project = await _projectyRepository.GetAsync(input.ProjectId);
            var contract = await _contractRepository.FirstOrDefaultAsync(aa => aa.Type == ContractTypeEnum.销项 && aa.RefId == input.ProjectId);
            var contractDetails = _contractDetailRepository.GetAll().Where(aa => aa.ContractId == contract.Id).Select(aa => new { aa.Id, aa.Price, aa.Num, aa.ProductId });
            var products = _productRepository.GetAll().Select(aa => new { aa.Id, aa.Price, aa.TaxRate, aa.Name });
            var invoice = await _invoiceRepository.FirstOrDefaultAsync(aa => aa.Type == InvoiceTypeEnum.销项 && aa.RefId == input.ProjectId);
            var invoiceDetails = _invoiceDetailRepository.GetAll().Where(aa => aa.InvoiceId == invoice.Id).Select(aa => new { aa.RefId, aa.TaxAmount });

            var taxations = (from contractDetail in contractDetails
                             join product in products on contractDetail.ProductId equals product.Id
                             join invoiceDetail in invoiceDetails on contractDetail.Id equals invoiceDetail.RefId into c
                             from d in c.DefaultIfEmpty()
                             group new {contractDetail, product,d} by contractDetail.Id into ff
                             select new
                             {
                                 ff.Key,
                                 ffff=ff.Select(aa=>aa.contractDetail.Num)
                                 //SaleTaxAmount = ff.Sum(aa=>aa.d==null?0:aa.d.TaxAmount),
                                 //IncomeTaxAmount = ff.Max(aa=>aa.contractDetail.Num),
                                 //VATPayable= d.TaxAmount- Math.Round(contractDetail.Num * b.Price * Convert.ToDecimal(b.TaxRate.Substring(0, b.TaxRate.IndexOf("%"))) ?? 0, 2),
                                 //CityEducationTax= Math.Round((d.TaxAmount- Math.Round(contractDetail.Num * b.Price * Convert.ToDecimal(b.TaxRate.Substring(0, b.TaxRate.IndexOf("%"))) ?? 0, 2))
                                 //* Convert.ToDecimal(0.07 + 0.02 + 0.03)+contract.Amount* Convert.ToDecimal(0.01)??0,2),
                                 //CorporateIncomeTax= Math.Round((contract.Amount-contractDetail.Num*b.Price-
                                 //(d.TaxAmount - Math.Round(contractDetail.Num * b.Price * Convert.ToDecimal(b.TaxRate.Substring(0, b.TaxRate.IndexOf("%"))) ?? 0, 2))-
                                 //Math.Round((d.TaxAmount - Math.Round(contractDetail.Num * b.Price * Convert.ToDecimal(b.TaxRate.Substring(0, b.TaxRate.IndexOf("%"))) ?? 0, 2))
                                 //* Convert.ToDecimal(0.07 + 0.02 + 0.03) + contract.Amount * Convert.ToDecimal(0.01) ?? 0, 2))*Convert.ToDecimal(0.25)??0,2),
                                 //IndividualIncomeTax= Math.Round((contractDetail.Num * contractDetail.Price- contractDetail.Num * b.Price-
                                 //(d.TaxAmount - Math.Round(contractDetail.Num * b.Price * Convert.ToDecimal(b.TaxRate.Substring(0, b.TaxRate.IndexOf("%"))) ?? 0, 2))-
                                 //Math.Round((d.TaxAmount - Math.Round(contractDetail.Num * b.Price * Convert.ToDecimal(b.TaxRate.Substring(0, b.TaxRate.IndexOf("%"))) ?? 0, 2))
                                 //* Convert.ToDecimal(0.07 + 0.02 + 0.03) + contract.Amount * Convert.ToDecimal(0.01) ?? 0, 2)- Math.Round((contract.Amount - contractDetail.Num * b.Price -
                                 //(d.TaxAmount - Math.Round(contractDetail.Num * b.Price * Convert.ToDecimal(b.TaxRate.Substring(0, b.TaxRate.IndexOf("%"))) ?? 0, 2)) -
                                 //Math.Round((d.TaxAmount - Math.Round(contractDetail.Num * b.Price * Convert.ToDecimal(b.TaxRate.Substring(0, b.TaxRate.IndexOf("%"))) ?? 0, 2))
                                 //* Convert.ToDecimal(0.07 + 0.02 + 0.03) + contract.Amount * Convert.ToDecimal(0.01) ?? 0, 2)) * Convert.ToDecimal(0.25) ?? 0, 2))*Convert.ToDecimal(0.2)??0,2)
                             }).ToList();
            var projectCosts = from contractDetail in contractDetails
                               join product in products on contractDetail.ProductId equals product.Id into a
                               from b in a.DefaultIfEmpty()
                               select new ProjectCost()
                               {
                                   DeviceName = b.Name,
                                   Price = b.Price,
                                   Num = contractDetail.Num
                               };



            ProjectProfitListDto item = new ProjectProfitListDto();
            item.Id = project.Id;
            item.Name = project.Name + "(" + project.ProjectCode + ")";
            item.ContractAmount = contract.Amount;
            item.CreationTime = project.CreationTime;
            item.SalesDetails = await _contractDetailRepository.GetAll().Where(aa => aa.ContractId == contract.Id)
                .Select(aa => new Dtos.SalesDetails() { Price = aa.Price, Num = aa.Num })
                .AsNoTracking().ToListAsync();
            item.ProjectCost = await projectCosts.AsNoTracking().ToListAsync();
            //item.Taxation = await taxations.AsNoTracking().ToListAsync();
            //item.Taxation = taxations;
            item.Profit = item.SalesDetails.Sum(aa => aa.Num * aa.Price) - item.ProjectCost.Sum(aa => aa.Num * aa.Price) - item.Taxation.Sum(aa => aa.VATPayable) -
                item.Taxation.Sum(aa => aa.CityEducationTax) - item.Taxation.Sum(aa => aa.CorporateIncomeTax) - item.Taxation.Sum(aa => aa.IndividualIncomeTax);
            item.ProfitMargin = Math.Round(item.Profit / item.ContractAmount ?? 0, 2);
            projectProfitListDtos.Add(item);
            throw new NotImplementedException();
        }
    }
}
