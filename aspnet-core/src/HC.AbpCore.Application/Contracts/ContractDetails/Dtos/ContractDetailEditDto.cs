
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
		/// RefDetailId
		/// </summary>
		public Guid? RefDetailId { get; set; }



		/// <summary>
		/// DeliveryDate
		/// </summary>
		public DateTime? DeliveryDate { get; set; }




    }
}