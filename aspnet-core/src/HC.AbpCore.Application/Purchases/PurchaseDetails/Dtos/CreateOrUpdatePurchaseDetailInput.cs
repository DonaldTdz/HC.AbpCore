

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Purchases.PurchaseDetails;

namespace HC.AbpCore.Purchases.PurchaseDetails.Dtos
{
    public class CreateOrUpdatePurchaseDetailInput
    {
        [Required]
        public PurchaseDetailEditDto PurchaseDetail { get; set; }

    }
}