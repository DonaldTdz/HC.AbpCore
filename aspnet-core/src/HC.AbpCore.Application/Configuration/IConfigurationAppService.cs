using System.Threading.Tasks;
using HC.AbpCore.Configuration.Dto;

namespace HC.AbpCore.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
