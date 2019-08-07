

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.AdvancePayments.AdvancePaymentDetails;

namespace HC.AbpCore.AdvancePayments.AdvancePaymentDetails.Dtos
{
    public class CreateOrUpdateAdvancePaymentDetailInput
    {
        [Required]
        public AdvancePaymentDetailEditDto AdvancePaymentDetail { get; set; }

    }
}