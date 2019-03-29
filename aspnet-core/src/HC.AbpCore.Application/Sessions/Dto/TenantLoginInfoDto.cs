using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using HC.AbpCore.MultiTenancy;

namespace HC.AbpCore.Sessions.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantLoginInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}
