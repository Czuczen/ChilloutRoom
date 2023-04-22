using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Web.Mvc.Authorization;
using CzuczenLand.Authorization;
using CzuczenLand.Users;
using CzuczenLand.Web.Models.Users;

namespace CzuczenLand.Web.Controllers;

[AbpMvcAuthorize(PermissionNames.Pages_Users)]
public class UsersController : CzuczenLandControllerBase
{
    private readonly IUserAppService _userAppService;

    public UsersController(IUserAppService userAppService)
    {
        _userAppService = userAppService;
    }

    public async Task<ActionResult> Index()
    {
        var users = (await _userAppService.GetAllAsync(new PagedResultRequestDto { MaxResultCount = int.MaxValue })).Items; //Paging not implemented yet
        var roles = (await _userAppService.GetRoles()).Items;
        var model = new UserListViewModel
        {
            Users = users,
            Roles = roles
        };

        return View(model);
    }

    public async Task<ActionResult> EditUserModal(long userId)
    {
        var user = await _userAppService.GetAsync(new EntityDto<long>(userId));
        var roles = (await _userAppService.GetRoles()).Items;
        var model = new EditUserModalViewModel
        {
            User = user,
            Roles = roles
        };
        return View("_EditUserModal", model);
    }
}