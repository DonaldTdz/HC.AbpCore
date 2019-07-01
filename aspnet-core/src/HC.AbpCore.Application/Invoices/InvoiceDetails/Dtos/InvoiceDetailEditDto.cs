
using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using HC.AbpCore.Invoices.InvoiceDetails;

namespace  HC.AbpCore.Invoices.InvoiceDetails.Dtos
{
    public class InvoiceDetailEditDto : FullAuditedEntityDto<Guid?>
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




    }
}