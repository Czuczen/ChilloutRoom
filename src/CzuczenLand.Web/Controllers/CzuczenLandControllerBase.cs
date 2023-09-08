using System.Web.Mvc.Filters;
using Abp.IdentityFramework;
using Abp.UI;
using Abp.Web.Mvc.Controllers;
using CzuczenLand.Authorization;
using CzuczenLand.ExtendingFunctionalities.Consts;
using CzuczenLand.Web.Helpers;
using Microsoft.AspNet.Identity;

namespace CzuczenLand.Web.Controllers;

/// <summary>
/// Wszystkie kontrolery powinny być dziedziczone z tej klasy.
/// </summary>
public abstract class CzuczenLandControllerBase : AbpController
{
    private bool? _isAdmin;
    private bool? _isDistrictWarden;
    
    protected string UserName { get; private set; }

    protected bool IsAdmin => _isAdmin ?? (_isAdmin = IsGranted(PermissionNames.Crud_Admin)) ?? false;
        
    protected bool IsDistrictWarden
    {
        get
        {
            _isDistrictWarden ??= IsGranted(PermissionNames.Crud_DistrictWarden);

            if (IsAdmin)
                _isDistrictWarden = false;
                
            return (bool) _isDistrictWarden;
        }
    }
    
    
    protected CzuczenLandControllerBase()
    {
        LocalizationSourceName = CzuczenLandConsts.LocalizationSourceName;
        ViewBag.SiteEmail = OtherConsts.ChillOutRoomEmail;
    }

    protected override void OnAuthentication(AuthenticationContext filterContext)
    {
        var userName = ViewHelper.GetUserNameFromIdentity(User?.Identity?.GetUserName());
        ViewBag.CurrUserName = userName;
        UserName = userName;
            
        base.OnAuthentication(filterContext);
    }
    
    protected virtual void CheckModelState()
    {
        if (!ModelState.IsValid)
        {
            throw new UserFriendlyException("Formularz jest nieprawidłowy. Sprawdź i napraw błędy.");
        }
    }

    protected void CheckErrors(IdentityResult identityResult)
    {
        identityResult.CheckErrors(LocalizationManager);
    }
}