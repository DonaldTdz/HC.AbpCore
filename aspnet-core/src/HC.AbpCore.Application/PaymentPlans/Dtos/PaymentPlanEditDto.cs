
using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using HC.AbpCore.PaymentPlans;

namespace  HC.AbpCore.PaymentPlans.Dtos
{
    public class PaymentPlanEditDto : EntityDto<Guid?>, IHasCreationTime
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
		/// Ratio
		/// </summary>
		[Required(ErrorMessage = "回款比率不能为空")]
        public int Ratio { get; set; }



        /// <summary>
        /// PaymentCondition
        /// </summary>
        [Required(ErrorMessage = "回款条件不能为空")]
        public string PaymentCondition { get; set; }



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