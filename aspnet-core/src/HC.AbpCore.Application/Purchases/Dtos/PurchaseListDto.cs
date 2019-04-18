

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Purchases;

namespace HC.AbpCore.Purchases.Dtos
{
    public class PurchaseListDto : FullAuditedEntityDto<Guid> 
    {

        
		/// <summary>
		/// Code
		/// </summary>
		public string Code { get; set; }



		/// <summary>
		/// ProjectId
		/// </summary>
		public Guid? ProjectId { get; set; }



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




    }
}