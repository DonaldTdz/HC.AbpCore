

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HC.AbpCore.DingTalk.Employees;

namespace HC.AbpCore.DingTalk.Employees.Dtos
{
    public class CreateOrUpdateEmployeeInput
    {
        [Required]
        public EmployeeEditDto Employee { get; set; }

    }
}