using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using HC.AbpCore.Authorization.Users;
using HC.AbpCore.MultiTenancy;
using System.Collections.Generic;

namespace HC.AbpCore
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class AbpCoreAppServiceBase : ApplicationService
    {
        public TenantManager TenantManager { get; set; }

        public UserManager UserManager { get; set; }

        protected AbpCoreAppServiceBase()
        {
            LocalizationSourceName = AbpCoreConsts.LocalizationSourceName;
        }

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        protected virtual Task<User> GetCurrentUserAsync()
        {
            var user = UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            return user;
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        /// <summary>
        /// 当前用户是否为超级管理员
        /// </summary>
        /// <returns></returns>
        protected async Task<bool> CheckAdminAsync()
        {
            var currentUser = await GetCurrentUserAsync();
            var roles = await UserManager.GetRolesAsync(currentUser);
            return roles.Contains(RoleCodes.Admin);
        }

        /// <summary>
        /// 获取当前用户所属角色
        /// </summary>
        /// <returns></returns>
        protected async Task<IList<string>> GetUserRolesAsync()
        {
            var currentUser = await GetCurrentUserAsync();
            var roles = await UserManager.GetRolesAsync(currentUser);
            return roles;
        }
    }
}
