

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.PaymentPlans;

namespace HC.AbpCore.PaymentPlans.Dtos
{
    public class PaymentPlanListDto : EntityDto<Guid>,IHasCreationTime 
    {

        
		/// <summary>
		/// ProjectId
		/// </summary>
		[Required(ErrorMessage="ProjectId不能为空")]
		public Guid ProjectId { get; set; }



		/// <summary>
		/// PlanTime
		/// </summary>
		[Required(ErrorMessage="PlanTime不能为空")]
		public DateTime PlanTime { get; set; }



		/// <summary>
		/// Desc
		/// </summary>
		public string Desc { get; set; }



		/// <summary>
		/// Amount
		/// </summary>
		public decimal? Amount { get; set; }



		/// <summary>
		/// Status
		/// </summary>
		public PaymentPlanStatusEnum? Status { get; set; }



		/// <summary>
		/// PaymentTime
		/// </summary>
		public DateTime? PaymentTime { get; set; }



        /// <summary>
        /// CreationTime
        /// </summary>
        [Required(ErrorMessage = "CreationTime不能为空")]
        public DateTime CreationTime { get; set; }




    }
}