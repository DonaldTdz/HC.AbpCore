

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Products;

namespace HC.AbpCore.Products.Dtos
{
    public class ProductListDto : EntityDto<int>, IHasCreationTime
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
		/// Specification
		/// </summary>
		[Required(ErrorMessage="Specification不能为空")]
		public string Specification { get; set; }



        /// <summary>
        /// Num
        /// </summary>
        public int? Num { get; set; }



        /// <summary>
        /// TaxRate
        /// </summary>
        [Required(ErrorMessage = "税率不能为空")]
        public string TaxRate { get; set; }



        /// <summary>
        /// Price
        /// </summary>
        [Required(ErrorMessage = "单价不能为空")]
        public decimal Price { get; set; }



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