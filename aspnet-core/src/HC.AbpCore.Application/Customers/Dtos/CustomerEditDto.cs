
using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using HC.AbpCore.Customers;
using static HC.AbpCore.Customers.CustomerBase;

namespace  HC.AbpCore.Customers.Dtos
{
    public class CustomerEditDto : FullAuditedEntityDto<int?>
    {
		/// <summary>
		/// Name
		/// </summary>
		[Required(ErrorMessage="Name不能为空")]
		public string Name { get; set; }



        /// <summary>
        /// Type
        /// </summary>
        public CustomerEnum Type { get; set; }



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
		/// Remark
		/// </summary>
		public string Remark { get; set; }
        
    }
}