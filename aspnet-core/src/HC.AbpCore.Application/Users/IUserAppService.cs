using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HC.AbpCore.Dtos;
using HC.AbpCore.Roles.Dto;
using HC.AbpCore.Users.Dto;

namespace HC.AbpCore.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
    {
        Task<ListResultDto<RoleDto>> GetRoles();

        Task ChangeLanguage(ChangeUserLanguageDto input);

        /// <summary>
        /// ͬ�������û�
        /// </summary>
        /// <returns></returns>
        Task<APIResultDto> SynchroDingUserAsync();
    }
}
