using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HC.AbpCore.MultiTenancy.Dto;

namespace HC.AbpCore.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

