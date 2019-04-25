
using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;
using HC.AbpCore.Invoices;

namespace  HC.AbpCore.Invoices.Dtos
{
    public class InvoiceEditDto: FullAuditedEntity<Guid?>
    {   
        
		/// <summary>
		/// Title
		/// </summary>
		[Required(ErrorMessage="Title不能为空")]
		public string Title { get; set; }



		/// <summary>
		/// Type
		/// </summary>
		[Required(ErrorMessage="Type不能为空")]
		public InvoiceTypeEnum Type { get; set; }



		/// <summary>
		/// Code
		/// </summary>
		[Required(ErrorMessage="Code不能为空")]
		public string Code { get; set; }



		/// <summary>
		/// Amount
		/// </summary>
		public decimal? Amount { get; set; }



		/// <summary>
		/// SubmitDate
		/// </summary>
		public DateTime? SubmitDate { get; set; }



		/// <summary>
		/// Attachments
		/// </summary>
		public string Attachments { get; set; }



		/// <summary>
		/// RefId
		/// </summary>
		public Guid? RefId { get; set; }




    }
}