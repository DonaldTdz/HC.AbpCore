

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Companys;

namespace HC.AbpCore.Companys.Dtos
{
    public class CreateOrUpdateCompanyInput
    {
        [Required]
        public CompanyEditDto Company { get; set; }

    }
}