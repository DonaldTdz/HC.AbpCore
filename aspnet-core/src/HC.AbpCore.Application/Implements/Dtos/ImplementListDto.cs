

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Implements;

namespace HC.AbpCore.Implements.Dtos
{
    public class ImplementListDto : FullAuditedEntityDto<Guid> 
    {

        
		/// <summary>
		/// ProjectId
		/// </summary>
		public Guid? ProjectId { get; set; }



		/// <summary>
		/// Name
		/// </summary>
		public string Name { get; set; }



		/// <summary>
		/// IsImplement
		/// </summary>
		public bool? IsImplement { get; set; }



		/// <summary>
		/// Attachments
		/// </summary>
		public string Attachments { get; set; }




    }
}