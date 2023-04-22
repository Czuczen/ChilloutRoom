﻿using Abp.MultiTenancy;
using CzuczenLand.Authorization.Users;

namespace CzuczenLand.MultiTenancy;

public class Tenant : AbpTenant<User>
{
    public Tenant()
    {
            
    }

    public Tenant(string tenancyName, string name)
        : base(tenancyName, name)
    {
    }
}