using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HC.AbpCore.Projects.ProjectDetails.Dtos
{
    public class BatchCreateProjectDetailInput
    {
        [Required]
        public List<ProjectDetailEditDto> ProjectDetails { get; set; }
    }
}
