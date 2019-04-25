

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Invoices.InvoiceDetails;

namespace HC.AbpCore.Invoices.InvoiceDetails.Dtos
{
    public class InvoiceDetailListDto : FullAuditedEntityDto<Guid> 
    {

        
		/// <summary>
		/// InvoiceId
		/// </summary>
		public Guid? InvoiceId { get; set; }



		/// <summary>
		/// RefId
		/// </summary>
		public Guid? RefId { get; set; }



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
		/// Unit
		/// </summary>
		public string Unit { get; set; }



		/// <summary>
		/// Num
		/// </summary>
		public decimal? Num { get; set; }



		/// <summary>
		/// Price
		/// </summary>
		public decimal? Price { get; set; }



		/// <summary>
		/// TaxRate
		/// </summary>
		public string TaxRate { get; set; }




    }
}