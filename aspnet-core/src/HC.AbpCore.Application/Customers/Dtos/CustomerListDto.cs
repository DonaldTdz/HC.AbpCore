

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Customers;
using static HC.AbpCore.Customers.CustomerBase;

namespace HC.AbpCore.Customers.Dtos
{
    public class CustomerListDto : FullAuditedEntityDto
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

        /// <summary>
        /// 类型
        /// </summary>
        public string TypeName
        {
            get
            {
                return Type.ToString();
            }
        }


    }
}