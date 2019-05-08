

using System.Collections.Generic;
using Abp.Application.Services.Dto;
using HC.AbpCore.TimeSheets;

namespace HC.AbpCore.TimeSheets.Dtos
{
    public class GetTimeSheetForEditOutput
    {

        public TimeSheetEditDto TimeSheet { get; set; }

    }
}