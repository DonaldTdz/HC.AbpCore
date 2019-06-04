using HC.AbpCore.Projects.ProjectDetails.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HC.AbpCore.Projects.Dtos
{
    public class CreateProjectAndDetailInput
    {
        /// <summary>
        /// 项目
        /// </summary>
        [Required]
        public ProjectEditDto Project { get; set; }

        /// <summary>
        /// 项目明细
        /// </summary>
        public List<ProjectDetailEditDto> ProjectDetails { get; set; }
    }
}
