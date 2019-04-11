

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Suppliers;

namespace HC.AbpCore.Suppliers.Dtos
{
    public class SupplierListDto : FullAuditedEntityDto 
    {

        
		/// <summary>
		/// Type
		/// </summary>
		public int? Type { get; set; }



		/// <summary>
		/// Name
		/// </summary>
		[Required(ErrorMessage="Name不能为空")]
		public string Name { get; set; }



		/// <summary>
		/// Address
		/// </summary>
		public string Address { get; set; }



		/// <summary>
		/// ZipCode
		/// </summary>
		public string ZipCode { get; set; }



		/// <summary>
		/// Tel
		/// </summary>
		public string Tel { get; set; }



		/// <summary>
		/// Contact
		/// </summary>
		public string Contact { get; set; }



		/// <summary>
		/// Position
		/// </summary>
		public string Position { get; set; }



		/// <summary>
		/// Phone
		/// </summary>
		public string Phone { get; set; }



		/// <summary>
		/// Desc
		/// </summary>
		public string Desc { get; set; }



		/// <summary>
		/// Remark
		/// </summary>
		public string Remark { get; set; }



		/// <summary>
		/// Attachments
		/// </summary>
		public string Attachments { get; set; }

        


    }
}