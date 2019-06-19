
using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using HC.AbpCore.Customers.CustomerContacts;

namespace  HC.AbpCore.Customers.CustomerContacts.Dtos
{
    public class CustomerContactEditDto : FullAuditedEntityDto<int?>
    {
        /// <summary>
        /// CustomerId
        /// </summary>
        [Required(ErrorMessage = "CustomerId不能为空")]
        public int? CustomerId { get; set; }



        /// <summary>
        /// Name
        /// </summary>
        [Required(ErrorMessage = "Name不能为空")]
        public string Name { get; set; }



        /// <summary>
        /// DeptName
        /// </summary>
        [Required(ErrorMessage = "DeptName不能为空")]
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