

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.AdvancePayments;

namespace HC.AbpCore.AdvancePayments.Dtos
{
    public class CreateOrUpdateAdvancePaymentInput
    {
        [Required]
        public AdvancePaymentEditDto AdvancePayment { get; set; }

    }
}