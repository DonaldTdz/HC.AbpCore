

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Purchases.PurchaseDetails;

namespace HC.AbpCore.Purchases.PurchaseDetails.Dtos
{
    public class PurchaseDetailListDto : FullAuditedEntityDto<Guid> 
    {

        
		/// <summary>
		/// PurchaseId
		/// </summary>
		public Guid? PurchaseId { get; set; }



		/// <summary>
		/// SupplierId
		/// </summary>
		public int? SupplierId { get; set; }



		/// <summary>
		/// ProductId
		/// </summary>
		public int? ProductId { get; set; }



		/// <summary>
		/// Num
		/// </summary>
		public int? Num { get; set; }




    }
    public class PurchaseDetailListDtoNew
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }



        /// <summary>
        /// PurchaseId
        /// </summary>
        public Guid? PurchaseId { get; set; }



        /// <summary>
        /// SupplierId
        /// </summary>
        public int? SupplierId { get; set; }



        /// <summary>
        /// ProductId
        /// </summary>
        public int? ProductId { get; set; }



        /// <summary>
        /// Num
        /// </summary>
        public int? Num { get; set; }



        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }



        /// <summary>
        /// Specification
        /// </summary>
        public string Specification { get; set; }



        /// <summary>
        /// TaxRate
        /// </summary>
        public string TaxRate { get; set; }



        /// <summary>
        /// Price
        /// </summary>
        public decimal Price { get; set; }


        /// <summary>
        /// SupplierName
        /// </summary>
        public string SupplierName { get; set; }
    }
}