
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


using HC.AbpCore.Reports.ProfitStatistics;
using HC.AbpCore.Reports.ProfitStatistics.Dtos;
using HC.AbpCore.Reports.ProfitStatistics.DomainService;



namespace HC.AbpCore.Reports.ProfitStatistics
{
    /// <summary>
    /// ProfitStatistic应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class ProfitStatisticAppService : AbpCoreAppServiceBase, IProfitStatisticAppService
    {
        private readonly IRepository<ProfitStatistic, Guid> _entityRepository;

        private readonly IProfitStatisticManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public ProfitStatisticAppService(
        IRepository<ProfitStatistic, Guid> entityRepository
        , IProfitStatisticManager entityManager
        )
        {
            _entityRepository = entityRepository;
            _entityManager = entityManager;
        }


        /// <summary>
        /// 获取ProfitStatistic的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<List<ProfitStatisticListDto>> GetProfitStatisticAsync(GetProfitStatisticsInput input)
        {

            var query = _entityRepository.GetAll()
                .Where(aa => aa.CreationTime >= input.StartCreationTime && aa.CreationTime < input.EndCreationTime)
                .WhereIf(input.ProjectId.HasValue, aa => aa.Id == input.ProjectId);
            // TODO:根据传入的参数添加过滤条件

            //var count = await query.CountAsync();

            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    //.PageBy(input)
                    .ToListAsync();
            ProfitStatisticListDto entity = new ProfitStatisticListDto();
            List<ProfitStatisticListDto> profitStatisticListDtos = new List<ProfitStatisticListDto>();
            foreach (var item in entityList)
            {
                ProfitStatisticListDto profitStatistic = new ProfitStatisticListDto();
                profitStatistic.Id = item.Id;
                profitStatistic.Name = item.Name;
                profitStatistic.ContractAmount = item.ContractAmount ?? 0;
                profitStatistic.ReimburseAmount = item.ReimburseAmount ?? 0;
                profitStatistic.TimesheetAmount = item.TimesheetAmount ?? 0;
                profitStatistic.CommodityCostAmount = item.CommodityCostAmount ?? 0;
                profitStatistic.SaleTaxAmount = item.SaleTaxAmount ?? 0;
                profitStatistic.IncomeTaxAmount = item.IncomeTaxAmount ?? 0;
                profitStatistic.TotalCostAmount = profitStatistic.CommodityCostAmount + profitStatistic.TimesheetAmount + profitStatistic.ReimburseAmount;
                profitStatistic.CreationTime = item.CreationTime;
                profitStatistic.VATPayable = profitStatistic.SaleTaxAmount - profitStatistic.IncomeTaxAmount;
                profitStatistic.CityEducationTax = Math.Round(((profitStatistic.VATPayable * Convert.ToDecimal(0.07 + 0.02 + 0.03)) + (profitStatistic.ContractAmount* item.CostCount * Convert.ToDecimal(0.001)))??0,2);
                profitStatistic.CorporateIncomeTax = Math.Round(((profitStatistic.ContractAmount*item.CostCount - profitStatistic.CommodityCostAmount - profitStatistic.CityEducationTax
                    - profitStatistic.VATPayable) * Convert.ToDecimal(0.25)).Value,2);
                profitStatistic.IndividualIncomeTax = Math.Round(((profitStatistic.ContractAmount * item.CostCount - profitStatistic.CommodityCostAmount - profitStatistic.CityEducationTax
                    - profitStatistic.VATPayable - profitStatistic.CorporateIncomeTax) * Convert.ToDecimal(0.2)).Value,2);
                profitStatistic.Profit = profitStatistic.ContractAmount - profitStatistic.TotalCostAmount - profitStatistic.VATPayable - profitStatistic.CityEducationTax
                    - profitStatistic.CorporateIncomeTax - profitStatistic.IndividualIncomeTax;
                profitStatistic.ProfitMargin = Math.Round((profitStatistic.Profit / (profitStatistic.ContractAmount==0?1: profitStatistic.ContractAmount) / Convert.ToDecimal(100)).Value,2);
                profitStatisticListDtos.Add(profitStatistic);
            }
            if (profitStatisticListDtos?.Count > 0)
            {
                entity.Name = "合计";
                entity.ContractAmount = profitStatisticListDtos.Sum(aa => aa.ContractAmount);
                entity.TotalCostAmount = profitStatisticListDtos.Sum(aa => aa.TotalCostAmount);
                entity.IncomeTaxAmount = profitStatisticListDtos.Sum(aa => aa.IncomeTaxAmount);
                entity.SaleTaxAmount = profitStatisticListDtos.Sum(aa => aa.SaleTaxAmount);
                entity.VATPayable = profitStatisticListDtos.Sum(aa => aa.VATPayable);
                entity.CityEducationTax = profitStatisticListDtos.Sum(aa => aa.CityEducationTax);
                entity.IndividualIncomeTax = profitStatisticListDtos.Sum(aa => aa.IndividualIncomeTax);
                entity.Profit = profitStatisticListDtos.Sum(aa => aa.Profit);
                entity.ProfitMargin = Math.Round((entity.Profit / (entity.ContractAmount == 0 ? 1 : entity.ContractAmount) / Convert.ToDecimal(100)).Value, 2);
                entity.CorporateIncomeTax = profitStatisticListDtos.Sum(aa => aa.CorporateIncomeTax);
                profitStatisticListDtos.Add(entity);
            }

            // var entityListDtos = ObjectMapper.Map<List<ProfitStatisticListDto>>(entityList);
            //var entityListDtos = entityList.MapTo<List<ProfitStatisticListDto>>();

            return profitStatisticListDtos;
        }


        /// <summary>
        /// 通过指定id获取ProfitStatisticListDto信息
        /// </summary>

        public async Task<ProfitStatisticListDto> GetByIdAsync(EntityDto<Guid> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);

            return entity.MapTo<ProfitStatisticListDto>();
        }


        /// <summary>
        /// 导出ProfitStatistic为excel表,等待开发。
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


