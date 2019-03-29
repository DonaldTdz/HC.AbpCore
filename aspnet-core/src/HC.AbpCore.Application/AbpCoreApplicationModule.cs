using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using HC.AbpCore.Authorization;

namespace HC.AbpCore
{
    [DependsOn(
        typeof(AbpCoreCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class AbpCoreApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<AbpCoreAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(AbpCoreApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddProfiles(thisAssembly)
            );
        }
    }
}
