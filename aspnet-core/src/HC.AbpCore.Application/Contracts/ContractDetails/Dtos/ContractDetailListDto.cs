

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