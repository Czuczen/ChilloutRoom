using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.Web.Mvc.Authorization;
using CzuczenLand.ExtendingFunctionalities;
using CzuczenLand.ExtendingFunctionalities.BackgroundWorkers;
using CzuczenLand.ExtendingFunctionalities.PlantationManager;
using CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.General.PlayerStorage.Base;
using CzuczenLand.ExtendingFunctionalities.Services.General.Quest.Base;
using CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Info;
using CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Quest;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;
using CzuczenLand.Web.Helpers;
using CzuczenLand.Web.Models.ExtendingModels.Plantation;
using CzuczenLand.Web.ViewModelsFactory;
using Microsoft.AspNet.SignalR;

namespace CzuczenLand.Web.Controllers;

[AbpMvcAuthorize]
public class PlantationController : CzuczenLandControllerBase
{
    private readonly IRepository<Quest> _questRepository;
    private readonly IRepository<Bonus> _bonusRepository;
    private readonly IRepository<PlantationStorage> _plantationStorageRepository;
    private readonly IRepository<QuestRequirementsProgress> _questRequirementsProgressRepository;
    private readonly IQuestService _questService;
    private readonly IPlantationManager _plantationManager;
    private readonly IPlayerStorageService _playerStorageService;
    private readonly IHubContext _questHub;
    private readonly IHubContext _infoHub;

    
    public PlantationController(
        IRepository<Quest> questRepository,
        IRepository<Bonus> bonusRepository,
        IRepository<PlantationStorage> plantationStorageRepository,
        IRepository<QuestRequirementsProgress> questRequirementsProgressRepository,
        IQuestService questService,
        IPlantationManager plantationManager,
        IPlayerStorageService playerStorageService
    )
    {
        _questRepository = questRepository;
        _bonusRepository = bonusRepository;
        _plantationStorageRepository = plantationStorageRepository;
        _questRequirementsProgressRepository = questRequirementsProgressRepository;
        _questService = questService;
        _plantationManager = plantationManager;
        _playerStorageService = playerStorageService;
        _infoHub = GlobalHost.ConnectionManager.GetHubContext<InfoHub>();
        _questHub = GlobalHost.ConnectionManager.GetHubContext<QuestHub>();
    }

    public async Task<ActionResult> Index()
    {
        await _playerStorageService.CreatePlayerStorageIfNotExist(AbpSession.GetUserId(), UserName);
        
        return View(await GetPlantationInfoViewModel());
    }

    [HttpGet]
    public async Task<ActionResult> SetDistrict(int id, bool heWantPayForHollow = false)
    {
        var userId = AbpSession.GetUserId();
        var lastSelectedDistrictId = (await _playerStorageService.GetPlayerStorage(userId)).LastSelectedDistrictId;
        var plantation = await GetPlantationInfoViewModel(id, heWantPayForHollow);

        // Wiele zakładek.
        // Jeśli strona została odświeżona nie wysyłamy. Jeśli odświeżona to są takie same.
        // Jeśli chce kupić dzielnicę odświeżamy wszędzie żeby mu się załadowała.
        if (lastSelectedDistrictId != id || heWantPayForHollow)
        {
            // jeśli w innej zakładce/przeglądarce użytkownik był by na innej dzielnicy. Odświerzamy stronę żeby załadowała mu się ostatnio wybrana.
            _infoHub.Clients.User(userId.ToString()).districtChanged(id);    
        }

        return View("Index", plantation);
    }

    private async Task<PlantationViewModel> GetPlantationInfoViewModel(int? districtId = null, bool heWantPayForHollow = false)
    {
        var userId = AbpSession.GetUserId();
        var plantation = await _plantationManager.GetPlantation(userId, districtId, heWantPayForHollow);
        var ret = ViewModelFactory.CreatePlantationViewModel(plantation, ObjectMapper, Logger);
            
        return ret;
    }
        
    [HttpGet]
    public async Task<JsonResult> GetBlackMarket()
    {
        return Json(await _plantationManager.CreatePlayerBlackMarket(AbpSession.GetUserId(), ObjectMapper), JsonRequestBehavior.AllowGet);
    }

    [HttpPost]
    public async Task<JsonResult> GetPlayerProducts(string entity, string valueToSearch)
    {
        var userId = AbpSession.GetUserId();
        var userProducts = await _plantationManager.GetAvailablePlayerProducts(userId, entity, valueToSearch);
        var ret = ViewHelper.GetS2ProductsList(userProducts);
        
        return Json(ret);
    }

    [HttpPost]
    public async Task<ActionResult> CreatePlant(PlantData plantData)
    {
        var userId = AbpSession.GetUserId();
        var createPlant = await _plantationManager.CreatePlayerPlant(userId, UserName, plantData);

        if (createPlant.RequirementsIsOk)
            return PartialView("Others/_plant",
                ViewModelFactory.CreatePlantViewModel(ObjectMapper, createPlant.Lamp, createPlant.Manure,
                    createPlant.Soil, createPlant.Water, createPlant.Seed, createPlant.Pot, createPlant.Plant));
            
        var checkMessageObj = new Dictionary<string, object> {["requirementMessage"] = createPlant.RequirementsIsNotOkMessage};
        
        return Json(checkMessageObj);
    }

    [HttpDelete]
    public async Task<JsonResult> RemovePlant(int id)
    {
        return Json(await _plantationManager.RemovePlayerPlant(id));
    }

    [HttpPost]
    public virtual async Task<JsonResult> CollectPlant(int id)
    {
        return Json(await _plantationManager.CollectPlayerPlant(id));
    }
    
    [HttpPost]
    public async Task<ActionResult> CompleteQuest(int id)
    {
        var userId = AbpSession.GetUserId();
        var completeQuest = await _plantationManager.ProcessCompletedQuest(userId, id, ObjectMapper);
        
        if (!completeQuest.InitStatus)
            return null;

        var questInfo = ViewModelFactory.CreateQuestInfoViewModel(completeQuest.Quest, ObjectMapper, completeQuest.QuestInfoCreation);
            
        return PartialView("Quests/_quest", questInfo);
    }
        
    [HttpGet]
    public async Task<ActionResult> GetQuest(int id)
    {
        var quest = await _questRepository.GetAsync(id);
        var questInfoCreation = await _plantationManager.CreateQuestInfoCreationModel(quest);
            
        return quest.IsAvailableInitially
            ? PartialView("Quests/_quest", ViewModelFactory.CreateQuestInfoViewModel(quest, ObjectMapper, questInfoCreation))
            : null;
    }

    [HttpPost]
    public async Task<ActionResult> AbandonQuest(int id)
    {
        var quest = await _questRepository.GetAsync(id);
        
        if (!quest.InProgress) return null;
            
        var questProgress = await _questRequirementsProgressRepository.SingleAsync(item => item.QuestId == quest.Id);
        WorkersHelper.ResetQuest(quest, questProgress);
        await _questService.SetStartValuesAsync(new List<Quest> {quest});
        var questInfoCreation = await _plantationManager.CreateQuestInfoCreationModel(quest);
            
        return PartialView("Quests/_quest", ViewModelFactory.CreateQuestInfoViewModel(quest, ObjectMapper, questInfoCreation));
    }

    [HttpPost]
    public async Task<ActionResult> TakeQuest(int id)
    {
        var quest = await _questRepository.GetAsync(id);
        
        if (quest.InProgress) return null;
            
        var plantationStorage = await _plantationStorageRepository.GetAsync((int) quest.PlantationStorageId);
        var questInfoCreation = await _plantationManager.CreateQuestInfoCreationModel(quest);
        await _questService.SetStartValuesAsync(new List<Quest> {quest});

        var limitIsOver = false;
        var heaveQuestTokens = TokensOperator.QuestTokenFee(_questHub, plantationStorage, quest);
        if (heaveQuestTokens)
            limitIsOver = PlantationManagerHelper.CheckQuestLimitsAndSetProgressStatus(quest, plantationStorage, _questHub);    

        return !limitIsOver && heaveQuestTokens
            ? PartialView("Quests/_quest", ViewModelFactory.CreateQuestInfoViewModel(quest, ObjectMapper, questInfoCreation))
            : null;
    }

    [HttpGet]
    public async Task<ActionResult> GetBonus(int id)
    {
        var bonus = await _bonusRepository.GetAsync(id);
        
        return PartialView("Others/_bonus", ViewModelFactory.CreateBonusesViewModel(new List<Bonus>{bonus}, ObjectMapper).First());
    }
}
