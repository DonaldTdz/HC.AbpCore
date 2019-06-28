

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Invoices.InvoiceDetails;
using Abp.AutoMapper;

namespace HC.AbpCore.Invoices.InvoiceDetails.Dtos
{
    [AutoMapFrom(typeof(InvoiceDetail))]
    public class InvoiceDetailListDto : FullAuditedEntityDto<Guid> 
    {

        
		/// <summary>
		/// InvoiceId
		/// </summary>
		public Guid? InvoiceId { get; set; }



		/// <summary>
		/// Name
		/// </summary>
		[Required(ErrorMessage="Name不能为空")]
		public string Name { get; set; }



		/// <summary>
		/// Specification
		/// </summary>
		public string Specification { get; set; }



		/// <summary>
		/// Num
		/// </summary>
		public decimal? Num { get; set; }



		/// <summary>
		/// Price
		/// </summary>
		public decimal? Price { get; set; }



        /// <summary>
        /// Amount
        /// </summary>
        public decimal? Amount { get; set; }



        /// <summary>
        /// TaxRate
        /// </summary>
        public string TaxRate { get; set; }



        /// <summary>
        /// TaxAmount
        /// </summary>
        public decimal? TaxAmount { get; set; }



        /// <summary>
        /// TotalAmount
        /// </summary>
        public decimal? TotalAmount { get; set; }



        /// <summary>
        /// RefName
        /// </summary>
        //public string RefName { get; set; }

    }
}