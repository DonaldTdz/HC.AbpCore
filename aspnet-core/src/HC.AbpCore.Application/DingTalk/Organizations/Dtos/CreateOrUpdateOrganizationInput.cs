

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.DingTalk.Organizations;

namespace HC.AbpCore.DingTalk.Organizations.Dtos
{
    public class CreateOrUpdateOrganizationInput
    {
        [Required]
        public OrganizationEditDto Organization { get; set; }

    }
}