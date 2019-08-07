

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
        /// ��Ŀ����
        /// </summary>
        public string Name { get; set; }



        /// <summary>
        /// ��Ŀ���
        /// </summary>
        public decimal? ContractAmount { get; set; }



        /// <summary>
        /// �������
        /// </summary>
        public decimal? ReimburseAmount { get; set; }



        /// <summary>
        /// ��ʱ�ɱ����
        /// </summary>
        public decimal? TimesheetAmount { get; set; }



        /// <summary>
        /// ��Ʒ�ɱ�
        /// </summary>
        public decimal? CommodityCostAmount { get; set; }



        /// <summary>
        /// ����˰��
        /// </summary>
        public decimal? SaleTaxAmount { get; set; }



        /// <summary>
        /// ����˰��
        /// </summary>
        public decimal? IncomeTaxAmount { get; set; }




        /// <summary>
        /// Ӧ����ֵ˰
        /// </summary>
        public decimal? VATPayable { get; set; }




        /// <summary>
        /// �ǽ�˰+��������
        /// </summary>
        public decimal? CityEducationTax { get; set; }




        /// <summary>
        /// ��ҵ����˰
        /// </summary>
        public decimal? CorporateIncomeTax { get; set; }




        /// <summary>
        /// ��������˰
        /// </summary>
        public decimal? IndividualIncomeTax { get; set; }




        /// <summary>
        /// ����
        /// </summary>
        public decimal? Profit { get; set; }




        /// <summary>
        /// ������
        /// </summary>%
        public decimal? ProfitMargin { get; set; }




        /// <summary>
        /// �ɱ��ܽ��
        /// </summary>%
        public decimal? TotalCostAmount { get; set; }



        /// <summary>
        /// ��������
        /// </summary>
        public DateTime CreationTime { get; set; }




    }
}