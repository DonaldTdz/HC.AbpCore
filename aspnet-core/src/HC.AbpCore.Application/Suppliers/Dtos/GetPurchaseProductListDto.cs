using System;
using System.Collections.Generic;
using System.Text;

namespace HC.AbpCore.Suppliers.Dtos
{
    public class GetPurchaseProductListDto
    {
        /// <summary>
        /// 采购ID
        /// </summary>
        public Guid PurchaseId { get; set; }

        /// <summary>
        /// 采购编号
        /// </summary>
        public string PurchaseCode { get; set; }

        /// <summary>
        /// 采购产品名称
        /// </summary>
        public string PurchaseName { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specification { get; set; }

        /// <summary>
        /// 采购日期
        /// </summary>
        public DateTime? PurchaseDate { get; set; }

    }
}
