

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
		/// Type
		/// </summary>
		public string Type { get; set; }



		/// <summary>
		/// Name
		/// </summary>
		public string Name { get; set; }



		/// <summary>
		/// Specification
		/// </summary>
		public string Specification { get; set; }



		/// <summary>
		/// Unit
		/// </summary>
		public string Unit { get; set; }



		/// <summary>
		/// Num
		/// </summary>
		public decimal? Num { get; set; }



		/// <summary>
		/// Price
		/// </summary>
		public decimal? Price { get; set; }



		/// <summary>
		/// ProductId
		/// </summary>
		public int? ProductId { get; set; }



        /// <summary>
        /// ×Ü½ð¶î
        /// </summary>
        public decimal? TotalSum { get; set; }
    }
}