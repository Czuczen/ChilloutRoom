using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Runtime.Session;
using Abp.Web.Mvc.Authorization;
using CzuczenLand.ExtendingFunctionalities.Services.Others.TicTacToe.Base;
using CzuczenLand.Web.ViewModelsFactory;

namespace CzuczenLand.Web.Controllers;

[AbpMvcAuthorize]
public class TicTacToeController : CzuczenLandControllerBase
{
    private readonly ITicTacToeStorageService _ticTacToeStorageService;

    
    public TicTacToeController(ITicTacToeStorageService ticTacToeStorageService)
    {
        _ticTacToeStorageService = ticTacToeStorageService;
    }

    public async Task<ActionResult> Index()
    {
        var userId = AbpSession.GetUserId();
        var playerStorage = await _ticTacToeStorageService.GetOrInitStorage(userId, UserName);
        var ret = ViewModelFactory.CreateTicTacToeViewModel(playerStorage, ObjectMapper);

        return View(ret);
    }
}