
using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using HC.AbpCore.Purchases;

namespace  HC.AbpCore.Purchases.Dtos
{
    public class PurchaseEditDto : FullAuditedEntityDto<Guid?>
    {

		/// <summary>
		/// Code
		/// </summary>
		public string Code { get; set; }



		/// <summary>
		/// EmployeeId
		/// </summary>
		public string EmployeeId { get; set; }



		/// <summary>
		/// PurchaseDate
		/// </summary>
		public DateTime? PurchaseDate { get; set; }



		/// <summary>
		/// Desc
		/// </summary>
		public string Desc { get; set; }



		/// <summary>
		/// ArrivalDate
		/// </summary>
		public DateTime? ArrivalDate { get; set; }



		/// <summary>
		/// InvoiceIssuance
		/// </summary>
		public bool? InvoiceIssuance { get; set; }



		/// <summary>
		/// Attachments
		/// </summary>
		public string Attachments { get; set; }




    }
}