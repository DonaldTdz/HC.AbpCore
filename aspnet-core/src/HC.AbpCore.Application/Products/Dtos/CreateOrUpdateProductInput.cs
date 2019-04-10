

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Products;

namespace HC.AbpCore.Products.Dtos
{
    public class CreateOrUpdateProductInput
    {
        [Required]
        public ProductEditDto Product { get; set; }

    }
}