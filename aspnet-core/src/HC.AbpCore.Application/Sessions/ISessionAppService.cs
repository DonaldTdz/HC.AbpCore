using System.Threading.Tasks;
using Abp.Application.Services;
using HC.AbpCore.Sessions.Dto;

namespace HC.AbpCore.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
