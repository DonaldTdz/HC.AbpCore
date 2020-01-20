using System;
using System.Collections.Generic;
using System.Text;

namespace HC.AbpCore.Reimburses.Dtos
{
    public class ModifyGrantStatusEditDto
    {
        public Guid Id { get; set; }

        public bool GrantStatus { get; set; }
    }
}
