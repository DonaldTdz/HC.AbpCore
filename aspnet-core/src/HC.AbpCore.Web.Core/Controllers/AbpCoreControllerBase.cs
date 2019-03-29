using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace HC.AbpCore.Controllers
{
    public abstract class AbpCoreControllerBase: AbpController
    {
        protected AbpCoreControllerBase()
        {
            LocalizationSourceName = AbpCoreConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
