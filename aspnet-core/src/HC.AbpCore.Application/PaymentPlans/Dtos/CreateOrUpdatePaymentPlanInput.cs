

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.PaymentPlans;

namespace HC.AbpCore.PaymentPlans.Dtos
{
    public class CreateOrUpdatePaymentPlanInput
    {
        [Required]
        public PaymentPlanEditDto PaymentPlan { get; set; }

    }
}