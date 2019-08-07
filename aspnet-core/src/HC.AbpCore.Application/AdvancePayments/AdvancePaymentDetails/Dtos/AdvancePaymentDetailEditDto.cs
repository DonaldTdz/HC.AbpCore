
using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;
using HC.AbpCore.AdvancePayments.AdvancePaymentDetails;

namespace  HC.AbpCore.AdvancePayments.AdvancePaymentDetails.Dtos
{
    public class AdvancePaymentDetailEditDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }         


        
		/// <summary>
		/// AdvancePaymentId
		/// </summary>
		[Required(ErrorMessage="AdvancePaymentId不能为空")]
		public Guid AdvancePaymentId { get; set; }



		/// <summary>
		/// PurchaseDetailId
		/// </summary>
		public Guid? PurchaseDetailId { get; set; }



		/// <summary>
		/// Ratio
		/// </summary>
		public int? Ratio { get; set; }



		/// <summary>
		/// Amount
		/// </summary>
		public decimal? Amount { get; set; }



		/// <summary>
		/// CreationTime
		/// </summary>
		[Required(ErrorMessage="CreationTime不能为空")]
		public DateTime CreationTime { get; set; }




    }
}