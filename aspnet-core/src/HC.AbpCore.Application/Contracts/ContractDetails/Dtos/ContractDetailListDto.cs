

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Contracts.ContractDetails;
using Abp.AutoMapper;

namespace HC.AbpCore.Contracts.ContractDetails.Dtos
{
    [AutoMapFrom(typeof(ContractDetail))]
    public class ContractDetailListDto : FullAuditedEntityDto<Guid> 
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



        /// <summary>
		/// RefDetailName
		/// </summary>
		public string RefDetailName { get; set; }
    }
}