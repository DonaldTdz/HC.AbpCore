using System;
using System.Collections.Generic;
using System.Text;

namespace HC.AbpCore.Reports.ProjectProfitReport.Dtos
{
    public class ProjectProfitListDto
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }



        /// <summary>
        /// 项目金额
        /// </summary>
        public decimal? ContractAmount { get; set; }



        /// <summary>
        /// 销售明细
        /// </summary>
        public List<SalesDetails> SalesDetails { get; set; }



        /// <summary>
        /// 成本
        /// </summary>
        public List<ProjectCost> ProjectCost { get; set; }



        /// <summary>
        /// 税费
        /// </summary>
        public List<Taxation> Taxation { get; set; }



        /// <summary>
        /// 费用
        /// </summary>
        //public List<Cost> Cost { get; set; }




        /// <summary>
        /// 利润
        /// </summary>
        public decimal? Profit { get; set; }




        /// <summary>
        /// 利润率
        /// </summary>%
        public decimal? ProfitMargin { get; set; }



        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreationTime { get; set; }
    }

    public class SalesDetails
    {
        /// <summary>
        /// 单价
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int? Num { get; set; }
    }

    public class ProjectCost
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int? Num { get; set; }
    }

    public class Taxation
    {

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
    }

    public class Cost
    {
        /// <summary>
        /// 个人所得税
        /// </summary>
        public decimal? IndividualIncomeTax { get; set; }
    }

    
}
