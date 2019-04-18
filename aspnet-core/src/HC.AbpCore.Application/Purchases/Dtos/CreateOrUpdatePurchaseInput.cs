

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Purchases;

namespace HC.AbpCore.Purchases.Dtos
{
    public class CreateOrUpdatePurchaseInput
    {
        [Required]
        public PurchaseEditDto Purchase { get; set; }

    }
}