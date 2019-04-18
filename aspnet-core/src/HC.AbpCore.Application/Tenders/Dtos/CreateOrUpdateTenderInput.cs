

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Tenders;

namespace HC.AbpCore.Tenders.Dtos
{
    public class CreateOrUpdateTenderInput
    {
        [Required]
        public TenderEditDto Tender { get; set; }

    }
}