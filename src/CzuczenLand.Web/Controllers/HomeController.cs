using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Domain.Repositories;
using Abp.Timing;
using Abp.Web.Mvc.Authorization;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Others;
using CzuczenLand.Web.Models.ExtendingModels.Home;
using CzuczenLand.Web.ViewModelsFactory;

namespace CzuczenLand.Web.Controllers;

[AbpMvcAuthorize]
public class HomeController : CzuczenLandControllerBase
{
    private readonly IRepository<PlayerStorage> _playerStorageRepository;
    private readonly IRepository<News> _newsRepository;
    private readonly IRepository<District> _districtRepository;
    private readonly IRepository<TicTacToeStorage> _ticTacToeStorageRepository;
        
    
    public HomeController(
        IRepository<PlayerStorage> playerStorageRepository,
        IRepository<News> newsRepository,
        IRepository<District> districtRepository,
        IRepository<TicTacToeStorage> ticTacToeStorageRepository
    )
    {
        _playerStorageRepository = playerStorageRepository;
        _newsRepository = newsRepository;
        _districtRepository = districtRepository;
        _ticTacToeStorageRepository = ticTacToeStorageRepository;
    }

    public async Task<ActionResult> Index()
    {
        var ret = new List<NewsViewModel>();
    
        var allNews = await _newsRepository.GetAllListAsync();
        var allDistricts = await _districtRepository.GetAllListAsync();
        var allPlayerStorages = await _playerStorageRepository.GetAllListAsync();
        var entityNews = ViewModelFactory.CreateNewsViewModel(allNews, ObjectMapper);
        ret.AddRange(entityNews);

        var nowDateTime = Clock.Now;
        var oneWeekBeforeStart = nowDateTime - TimeSpan.FromDays(7); // komunikaty o dzielnicach będą widoczne tydzień przed ich startem, do dnia ich startu
        var districts = allDistricts.Where(item =>
            (item.StartTime >= oneWeekBeforeStart && item.StartTime <= nowDateTime && item.IsDefined && item.EndTime == null) ||
            (item.StartTime >= oneWeekBeforeStart && item.StartTime <= nowDateTime && item.IsDefined &&
             item.EndTime != null && item.EndTime > nowDateTime)).ToList();
        
        var districtsNews = ViewModelFactory.CreateDistrictNewsViewModel(districts);
        ret.AddRange(districtsNews);

        var districtWardens = allDistricts
            .Select(item => allPlayerStorages.FirstOrDefault(storage => storage.UserId == item.UserId))
            .Where(item => item != null);
        var playerStorages = allPlayerStorages.Where(item => districtWardens.All(di => di.UserId != item.UserId) && item.GainedExperience > 100);
        var topPlantationPlayers = playerStorages.OrderByDescending(item => item.GainedExperience).Take(10).ToList();
        var topPlantationPlayersNews = ViewModelFactory.CreateTopPlantationPlayersNewsViewModel(topPlantationPlayers);
        ret.Add(topPlantationPlayersNews);

        var ticTacToePlayerStorages = await _ticTacToeStorageRepository.GetAllListAsync(item => item.GamesPlayed > 20);
        var topTicTacToePlayers = ticTacToePlayerStorages.OrderByDescending(item => 
            item.GamesWon / item.GamesPlayed * 100).Take(10).ToList();
        var topTicTacToePlayersNews = ViewModelFactory.CreateTopTicTacToePlayersNewsViewModel(topTicTacToePlayers);
        ret.Add(topTicTacToePlayersNews);

        return View(ret);
    }
}