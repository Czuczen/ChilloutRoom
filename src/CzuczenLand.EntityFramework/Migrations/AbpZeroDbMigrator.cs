﻿using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Abp.Zero.EntityFramework;
using CzuczenLand.EntityFramework;

namespace CzuczenLand.Migrations;

public class AbpZeroDbMigrator : AbpZeroDbMigrator<CzuczenLandDbContext, Configuration>
{
    public AbpZeroDbMigrator(
        IUnitOfWorkManager unitOfWorkManager,
        IDbPerTenantConnectionStringResolver connectionStringResolver,
        IIocResolver iocResolver)
        : base(
            unitOfWorkManager,
            connectionStringResolver,
            iocResolver)
    {
    }
}