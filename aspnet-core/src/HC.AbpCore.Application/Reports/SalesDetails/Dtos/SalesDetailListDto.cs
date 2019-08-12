

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Reports.SalesDetails;
using Abp.AutoMapper;

namespace HC.AbpCore.Reports.SalesDetails.Dtos
{
    [AutoMapFrom(typeof(SalesDetail))]
    public class SalesDetailListDto : EntityDto<Guid?>,IHasCreationTime 
    {

        
		/// <summary>
		/// Name
		/// </summary>
		public string projectName { get; set; }



		/// <summary>
		/// projectCode
		/// </summary>
		public string projectCode { get; set; }



		/// <summary>
		/// customerName
		/// </summary>
		public string customerName { get; set; }



		/// <summary>
		/// signatureTime
		/// </summary>
		public DateTime? signatureTime { get; set; }



		/// <summary>
		/// employeeName
		/// </summary>
		public string employeeName { get; set; }



		/// <summary>
		/// contractAmount
		/// </summary>
		public decimal? contractAmount { get; set; }



		/// <summary>
		/// paymentCondition
		/// </summary>
		public string paymentCondition { get; set; }



		/// <summary>
		/// acceptedAmount
		/// </summary>
		public decimal? acceptedAmount { get; set; }



		/// <summary>
		/// uncollectedAmount
		/// </summary>
		public decimal? uncollectedAmount { get; set; }



		/// <summary>
		/// paymentTime
		/// </summary>
		public DateTime? paymentTime { get; set; }



		/// <summary>
		/// invoiceCount
		/// </summary>
		public int? invoiceCount { get; set; }



		/// <summary>
		/// Profit
		/// </summary>
		public decimal? Profit { get; set; }



		/// <summary>
		/// CreationTime
		/// </summary>
		public DateTime CreationTime { get; set; }




    }
}