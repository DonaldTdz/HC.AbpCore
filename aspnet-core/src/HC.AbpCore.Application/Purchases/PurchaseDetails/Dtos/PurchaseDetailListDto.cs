

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Purchases.PurchaseDetails;
using Abp.AutoMapper;

namespace HC.AbpCore.Purchases.PurchaseDetails.Dtos
{
    [AutoMapFrom(typeof(PurchaseDetail))]
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
        /// Num
        /// </summary>
        public int? Num { get; set; }

        /// <summary>
        /// Price
        /// </summary>
        public decimal? Price { get; set; }



		/// <summary>
		/// ProjectDetailId
		/// </summary>
		public Guid? ProjectDetailId { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 项目明细名称
        /// </summary>
        public string ProjectDetailName { get; set; }

    }
}