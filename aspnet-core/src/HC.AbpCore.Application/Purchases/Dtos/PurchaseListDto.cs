

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Purchases;
using Abp.AutoMapper;

namespace HC.AbpCore.Purchases.Dtos
{
    [AutoMapFrom(typeof(Purchase))]
    public class PurchaseListDto : FullAuditedEntityDto<Guid> 
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



        /// <summary>
        /// 负责人名称
        /// </summary>
        public string EmployeeName { get; set; }
    }
}