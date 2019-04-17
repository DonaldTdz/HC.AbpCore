

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Projects;

namespace HC.AbpCore.Projects.Dtos
{
    public class CreateOrUpdateProjectInput
    {
        [Required]
        public ProjectEditDto Project { get; set; }

    }
}