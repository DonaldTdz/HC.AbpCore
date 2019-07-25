using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HC.AbpCore.Purchases.PurchaseDetails.Dtos
{
     public class CreatePurchaseDetailAndUpdateproductInput
    {
        [Required]
        public PurchaseDetailEditDtoNew PurchaseDetail { get; set; }
    }
}
