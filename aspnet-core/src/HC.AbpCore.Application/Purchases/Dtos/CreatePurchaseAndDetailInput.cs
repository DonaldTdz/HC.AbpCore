using HC.AbpCore.Purchases.PurchaseDetails.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HC.AbpCore.Purchases.Dtos
{
    public class CreatePurchaseAndDetailInput
    {
        /// <summary>
        /// 采购
        /// </summary>
        [Required]
        public PurchaseEditDto Purchase { get; set; }

        /// <summary>
        /// 采购明细
        /// </summary>
        public List<PurchaseDetailEditDto> PurchaseDetails { get; set; }
    }
}
