

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Projects.ProjectDetails;
using Abp.AutoMapper;

namespace HC.AbpCore.Projects.ProjectDetails.Dtos
{
    [AutoMapFrom(typeof(ProjectDetail))]
    public class ProjectDetailListDto : FullAuditedEntityDto<Guid> 
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



        /// <summary>
        /// ×Ü½ð¶î
        /// </summary>
        public decimal? TotalSum { get; set; }
    }
}