using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using CzuczenLand.Authorization.Users;
using CzuczenLand.MultiTenancy;
using Microsoft.AspNet.Identity;

namespace CzuczenLand;

public abstract class CzuczenLandAppServiceBase : ApplicationService
{
    public TenantManager TenantManager { get; set; }

    public UserManager UserManager { get; set; }

    protected CzuczenLandAppServiceBase()
    {
        LocalizationSourceName = CzuczenLandConsts.LocalizationSourceName;
    }

    protected virtual async Task<User> GetCurrentUserAsync()
    {
        var user = await UserManager.FindByIdAsync(AbpSession.GetUserId());
        if (user == null)
        {
            throw new ApplicationException("Brak aktualnego użytkownika!");
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
}