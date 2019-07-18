﻿using System.Collections.Generic;

namespace HC.AbpCore.Sessions.Dto
{
    public class GetCurrentLoginInformationsOutput
    {
        public ApplicationInfoDto Application { get; set; }

        public UserLoginInfoDto User { get; set; }

        public TenantLoginInfoDto Tenant { get; set; }

        public IList<string> Roles { get; set; }
    }
}
