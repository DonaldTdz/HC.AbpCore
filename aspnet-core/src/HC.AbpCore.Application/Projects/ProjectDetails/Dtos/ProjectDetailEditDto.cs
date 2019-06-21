
using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using HC.AbpCore.Projects.ProjectDetails;

namespace  HC.AbpCore.Projects.ProjectDetails.Dtos
{
    public class ProjectDetailEditDto : FullAuditedEntityDto<Guid?>
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
		/// Num
		/// </summary>
		public decimal? Num { get; set; }



		/// <summary>
		/// Price
		/// </summary>
		public decimal? Price { get; set; }




    }
}