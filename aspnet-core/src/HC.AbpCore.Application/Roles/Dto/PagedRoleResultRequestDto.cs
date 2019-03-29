using Abp.Application.Services.Dto;

namespace HC.AbpCore.Roles.Dto
{
    public class PagedRoleResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}

