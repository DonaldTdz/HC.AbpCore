using HC.AbpCore.AdvancePayments.Dtos;
using HC.AbpCore.Purchases.PurchaseDetails.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HC.AbpCore.Purchases.Dtos
{
    public class OnekeyCreatePurchaseInput
    {
        [Required]
        public PurchaseEditDto Purchase { get; set; }

        [Required]
        public List<PurchaseDetailEditDtoNew> PurchaseDetailNews { get; set; }

        [Required]
        public List<AdvancePaymentEditDto> AdvancePayments { get; set; }
    }
}
