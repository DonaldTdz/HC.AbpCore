

using System.Collections.Generic;
using Abp.Application.Services.Dto;
using HC.AbpCore.Tasks;

namespace HC.AbpCore.Tasks.Dtos
{
    public class GetCompletedTaskForEditOutput
    {

        public CompletedTaskEditDto CompletedTask { get; set; }

    }
}