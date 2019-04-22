
using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using HC.AbpCore.Purchases.PurchaseDetails;

namespace HC.AbpCore.Purchases.PurchaseDetails.Dtos
{
    public class PurchaseDetailEditDto : FullAuditedEntityDto<Guid?>
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
        /// Price
        /// </summary>
        public decimal? Price { get; set; }



        /// <summary>
        /// ProjectDetailId
        /// </summary>
        public Guid? ProjectDetailId { get; set; }




    }
}