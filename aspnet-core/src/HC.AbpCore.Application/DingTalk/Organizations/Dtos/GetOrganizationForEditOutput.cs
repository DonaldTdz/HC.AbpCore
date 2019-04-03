

using System.Collections.Generic;
using Abp.Application.Services.Dto;
using HC.AbpCore.DingTalk.Organizations;

namespace HC.AbpCore.DingTalk.Organizations.Dtos
{
    public class GetOrganizationForEditOutput
    {

        public OrganizationEditDto Organization { get; set; }

    }
}