using System;
using System.Collections.Generic;
using System.Text;
using HC.AbpCore.Reimburses.Dtos;

namespace HC.AbpCore.Dtos
{
    public class PagedResultNewDto<T>
    {

        public PagedResultNewDto(int totalCount, List<T> items,decimal totalAmount)
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
