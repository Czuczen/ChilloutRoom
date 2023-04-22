using System.Web.Mvc;
using Abp.Application.Navigation;
using Abp.Runtime.Session;
using Abp.Threading;
using CzuczenLand.Web.Models.Layout;

namespace CzuczenLand.Web.Controllers;

public class LayoutController : CzuczenLandControllerBase
{
    private readonly IUserNavigationManager _userNavigationManager;


    public LayoutController(IUserNavigationManager userNavigationManager)
    {
        _userNavigationManager = userNavigationManager;
    }

    [ChildActionOnly]
    public PartialViewResult SideBarNav(string activeMenu = "")
    {
        var model = new SideBarNavViewModel
        {
            MainMenu = AsyncHelper.RunSync(() => _userNavigationManager.GetMenuAsync("MainMenu", AbpSession.ToUserIdentifier())),
            ActiveMenuItemName = activeMenu
        };

        return PartialView("_SideBarNav", model);
    }
}