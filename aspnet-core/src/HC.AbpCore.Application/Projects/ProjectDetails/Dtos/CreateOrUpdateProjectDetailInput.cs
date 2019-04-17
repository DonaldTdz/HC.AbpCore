

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.Projects.ProjectDetails;

namespace HC.AbpCore.Projects.ProjectDetails.Dtos
{
    public class CreateOrUpdateProjectDetailInput
    {
        [Required]
        public ProjectDetailEditDto ProjectDetail { get; set; }

    }
}