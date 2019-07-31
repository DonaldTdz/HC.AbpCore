
using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;
using HC.AbpCore.Contracts.ContractDetails;

namespace  HC.AbpCore.Contracts.ContractDetails.Dtos
{
    public class ContractDetailEditDto:FullAuditedEntity<Guid?>
    {   


        
		/// <summary>
		/// ContractId
		/// </summary>
		public Guid? ContractId { get; set; }



        /// <summary>
        /// ²úÆ·Id
        /// </summary>
        public int? ProductId { get; set; }



        /// <summary>
        /// Name
        /// </summary>
        [StringLength(100)]
        public string Name { get; set; }



        /// <summary>
        /// Model
        /// </summary>
        [StringLength(100)]
        public string Model { get; set; }



        /// <summary>
        /// Num
        /// </summary>
        public int? Num { get; set; }



        /// <summary>
        /// Price
        /// </summary>
        public decimal? Price { get; set; }




    }
}