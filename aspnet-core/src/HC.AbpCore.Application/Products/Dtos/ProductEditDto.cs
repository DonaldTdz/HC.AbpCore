
using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;
using HC.AbpCore.Products;

namespace  HC.AbpCore.Products.Dtos
{
    public class ProductEditDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public int? Id { get; set; }         


        
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
		/// Specification
		/// </summary>
		[Required(ErrorMessage="Specification不能为空")]
		public string Specification { get; set; }



		/// <summary>
		/// Unit
		/// </summary>
		public string Unit { get; set; }



		/// <summary>
		/// IsEnabled
		/// </summary>
		public bool? IsEnabled { get; set; }



		/// <summary>
		/// CreationTime
		/// </summary>
		[Required(ErrorMessage="CreationTime不能为空")]
		public DateTime CreationTime { get; set; }




    }
}