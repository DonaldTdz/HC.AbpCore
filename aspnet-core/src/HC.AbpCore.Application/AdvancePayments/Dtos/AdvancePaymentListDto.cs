

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.AdvancePayments;

namespace HC.AbpCore.AdvancePayments.Dtos
{
    public class AdvancePaymentListDto : EntityDto<Guid>,IHasCreationTime 
    {

        
		/// <summary>
		/// PurchaseId
		/// </summary>
		[Required(ErrorMessage="PurchaseId不能为空")]
		public Guid PurchaseId { get; set; }



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
		/// Ratio
		/// </summary>
		public int? Ratio { get; set; }



		/// <summary>
		/// Amount
		/// </summary>
		public decimal? Amount { get; set; }



		/// <summary>
		/// Status
		/// </summary>
		public AdvancePaymentStatusEnum? Status { get; set; }



		/// <summary>
		/// PaymentTime
		/// </summary>
		public DateTime? PaymentTime { get; set; }



		/// <summary>
		/// CreationTime
		/// </summary>
		public DateTime CreationTime { get; set; }



        /// <summary>
        /// 付款状态
        /// </summary>
        public string StatusName
        {
            get { return Status.ToString(); }
        }
    }
}