
using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using HC.AbpCore.Projects;
using static HC.AbpCore.Projects.ProjectBase;

namespace  HC.AbpCore.Projects.Dtos
{
    public class ProjectEditDto: FullAuditedEntityDto<Guid?>
    {      
        
		/// <summary>
		/// Mode
		/// </summary>
		[Required(ErrorMessage="Mode不能为空")]
		public ProjectMode? Mode { get; set; }



		/// <summary>
		/// ProfitRatio
		/// </summary>
		public decimal? ProfitRatio { get; set; }



		/// <summary>
		/// BillCost
		/// </summary>
		public decimal? BillCost { get; set; }



		/// <summary>
		/// Type
		/// </summary>
		public string Type { get; set; }



		/// <summary>
		/// ProjectCode
		/// </summary>
		public string ProjectCode { get; set; }



		/// <summary>
		/// Name
		/// </summary>
		public string Name { get; set; }



		/// <summary>
		/// CustomerId
		/// </summary>
		public int? CustomerId { get; set; }



		/// <summary>
		/// EmployeeId
		/// </summary>
		public string EmployeeId { get; set; }



		/// <summary>
		/// StartDate
		/// </summary>
		public DateTime? StartDate { get; set; }



		/// <summary>
		/// EndDate
		/// </summary>
		public DateTime? EndDate { get; set; }



		/// <summary>
		/// Year
		/// </summary>
		public int? Year { get; set; }



		/// <summary>
		/// Budget
		/// </summary>
		public decimal? Budget { get; set; }



		/// <summary>
		/// Status
		/// </summary>
		public ProjectStatus? Status { get; set; }



		/// <summary>
		/// Desc
		/// </summary>
		public string Desc { get; set; }




    }
}