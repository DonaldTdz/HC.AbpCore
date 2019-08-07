

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Reports.ProfitStatistics;

namespace HC.AbpCore.Reports.ProfitStatistics.Dtos
{
    public class ProfitStatisticListDto : EntityDto<Guid>,IHasCreationTime 
    {


        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }



        /// <summary>
        /// 项目金额
        /// </summary>
        public decimal? ContractAmount { get; set; }



        /// <summary>
        /// 报销金额
        /// </summary>
        public decimal? ReimburseAmount { get; set; }



        /// <summary>
        /// 工时成本金额
        /// </summary>
        public decimal? TimesheetAmount { get; set; }



        /// <summary>
        /// 商品成本
        /// </summary>
        public decimal? CommodityCostAmount { get; set; }



        /// <summary>
        /// 销项税额
        /// </summary>
        public decimal? SaleTaxAmount { get; set; }



        /// <summary>
        /// 进项税额
        /// </summary>
        public decimal? IncomeTaxAmount { get; set; }




        /// <summary>
        /// 应交增值税
        /// </summary>
        public decimal? VATPayable { get; set; }




        /// <summary>
        /// 城建税+教育附加
        /// </summary>
        public decimal? CityEducationTax { get; set; }




        /// <summary>
        /// 企业所得税
        /// </summary>
        public decimal? CorporateIncomeTax { get; set; }




        /// <summary>
        /// 个人所得税
        /// </summary>
        public decimal? IndividualIncomeTax { get; set; }




        /// <summary>
        /// 利润
        /// </summary>
        public decimal? Profit { get; set; }




        /// <summary>
        /// 利润率
        /// </summary>%
        public decimal? ProfitMargin { get; set; }




        /// <summary>
        /// 成本总金额
        /// </summary>%
        public decimal? TotalCostAmount { get; set; }



        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreationTime { get; set; }




    }
}