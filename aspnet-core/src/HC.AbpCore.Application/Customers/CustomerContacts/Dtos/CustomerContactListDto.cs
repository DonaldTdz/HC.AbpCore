

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Customers.CustomerContacts;

namespace HC.AbpCore.Customers.CustomerContacts.Dtos
{
    public class CustomerContactListDto : FullAuditedEntityDto 
    {

        
		/// <summary>
		/// CustomerId
		/// </summary>
		public int? CustomerId { get; set; }



		/// <summary>
		/// Name
		/// </summary>
		public string Name { get; set; }



		/// <summary>
		/// DeptName
		/// </summary>
		public string DeptName { get; set; }



		/// <summary>
		/// Position
		/// </summary>
		public string Position { get; set; }



		/// <summary>
		/// Phone
		/// </summary>
		public string Phone { get; set; }




    }
}