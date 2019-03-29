using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.AbpCore
{
    public abstract class AbpCoreDomainServiceBase : DomainService
    {
        protected AbpCoreDomainServiceBase()
        {
            LocalizationSourceName = AbpCoreConsts.LocalizationSourceName;
        }
    }
}
