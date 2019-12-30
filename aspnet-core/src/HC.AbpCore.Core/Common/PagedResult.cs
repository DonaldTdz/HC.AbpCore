using System;
using System.Collections.Generic;
using System.Text;

namespace HC.AbpCore.Dtos
{
    public class PagedResult<T>
    {

        public PagedResult(int totalCount, List<T> items,decimal totalAmount)
        {
            this.TotalCount = totalCount;
            this.Items = items;
            this.TotalAmount = totalAmount;
        }

        public decimal TotalAmount { get; set; }

        public int TotalCount { get; set; }

        public IReadOnlyList<T> Items { get; set; }
    }
}
