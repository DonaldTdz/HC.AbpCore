

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.InventoryFlows;

namespace HC.AbpCore.InventoryFlows.Dtos
{
    public class CreateOrUpdateInventoryFlowInput
    {
        [Required]
        public InventoryFlowEditDto InventoryFlow { get; set; }

    }
}