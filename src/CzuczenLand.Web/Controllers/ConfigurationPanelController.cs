using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Runtime.Session;
using Abp.Web.Mvc.Authorization;
using CzuczenLand.Authorization;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.ConfigurationPanelManager;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.SelectListLoader;
using CzuczenLand.ExtendingFunctionalities.Utils;
using CzuczenLand.Web.Helpers;
using CzuczenLand.Web.Models.ExtendingModels.ConfigurationPanel;
using CzuczenLand.Web.ViewModelsFactory;

namespace CzuczenLand.Web.Controllers;

[AbpMvcAuthorize(PermissionNames.Pages_ConfigurationPanel)]
public class ConfigurationPanelController : CzuczenLandControllerBase
{
    private readonly IConfigurationPanelManager _configurationPanelManager;

    
    public ConfigurationPanelController(IConfigurationPanelManager configurationPanelManager)
    {
        _configurationPanelManager = configurationPanelManager;
    }

    public ActionResult Index()
    {
        var ret = new EntityNamesViewModel();
        var entitiesList = new List<string>();
        
        if (IsDistrictWarden)
        {
            entitiesList = Enum.GetValues(typeof(EnumUtils.EntitiesPermissionWarden)).Cast<EnumUtils.EntitiesPermissionWarden>()
                .Select(currEnum => currEnum.ToString()).ToList();
        }
        else if (IsAdmin)
        {
            entitiesList = Enum.GetValues(typeof(EnumUtils.EntitiesPermissionAdmin)).Cast<EnumUtils.EntitiesPermissionAdmin>()
                .Select(currEnum => currEnum.ToString()).ToList();                        
        }
            
        ret.DbToHrNames = SelectListLoaderHelper.EntitiesNamesDbToHr;
        ret.DbNames = entitiesList;
            
        return View(ret);
    }

    [HttpGet]
    public ActionResult GetLogs()
    {
        var ret = new LogsViewModel();
  
        if (IsAdmin || IsDistrictWarden)
        {
            var logs = _configurationPanelManager.FormatLogs();
            ret = ViewModelFactory.CreateLogsViewModel(logs, ObjectMapper);
        }
        else
        {
            ret.PermissionInfo = "Brak uprawnień";
        }
            
        return PartialView("Logs/_logs", ret);
    }

    /// <summary>
    /// Problem z automatycznym wylogowaniem
    /// Jeśli testy są w trakcie zdaża się, że signalr traci połączenie i próbuje je odnowić
    /// ale odnowienie kończy się niepowodzeniem przez co użytkonik jest wylogowywany.
    /// Opis prawdopodobnego źródła problemu http://johnculviner.com/asp-net-concurrent-ajax-requests-and-session-state-blocking/
    /// Opis prawdopodobnego źródła problemu 2 https://github.com/SignalR/SignalR/issues/2631
    /// Nie znalazłem sposobu jak wpłynąć na sesję od Abp (AbpSession)
    /// [AbpAllowAnonymous] też niczego nie zmieniło.
    /// Próby z $.connection.hub._.keepAliveData.stopMonitoringKeepAlive($.connection.hub); też niczego nie zmieniły.
    /// Zmiany czasów dla $.connection.hub takich jak disconnectTimeout: 30000, transportConnectTimeout: 0, reconnectDelay: 2000, też niczego nie zmieniły.
    /// Jeśli akcja była synchroniczna lub akcja wymagała przeładowania strony, też niczego nie zmieniło.
    /// Problem nie rozwiązany!!!
    /// Ewentualne sposoby rozwiązania problemu to puszczenie akcji do backgroundJob która będzie w backendzie renderować widok i zapisywać go do pliku
    /// Stworzenie tej akcji jako akcję hub'a która będzie renderować widok w backendzie i zwracać to przez await Clients.User(AbpSession.GetUserId().ToString()).structureTests(content) (już sprawdzone w trochę inny sposób patrz klasa StructureTestsHub ale i tak ten sam problem)
    /// Stworzenie kontrolera anonymous który nie bedzie dziedziczył z kontrolera od abp. Stworzone ale jeszcze nie weryfikowane bo skończył mi się czas 2 miesiecy na smarter asp.
    /// PROBLEM PRZESTAŁ WYSTĘPOWAĆ
    /// Po postawieniu nowej bazy danych problem zniknął. Dziwne.
    /// Mimo utworzenia 23 dzielnic (districtCloner) dla których testy trwały około 3 minuty i robienie tych testów raz po raz przez godzine nie udało się odtworzyć problemu.
    /// Problem rozwiązany!!!
    /// Komentarze zostawiam w razie gdyby problem kiedyś powrócił
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult> StructureTests()
    {
        var ret = new StructureTestsViewModel();
        if (IsAdmin || IsDistrictWarden)
        {
            var structureTestsModel = await _configurationPanelManager.StartTests(IsAdmin, AbpSession.GetUserId());
            ret = ViewModelFactory.CreateStructureTestsViewModel(structureTestsModel);
        }
        else
        {
            ret.PermissionInfo = "Brak uprawnień";
        }

        return PartialView("StructureTests/_structureTests", ret);
    }

    [HttpPost]
    public async Task<ActionResult> EditOrCreateModal(string entity, int? objectId = null, string editManyAction = null)
    {
        var editOrCreateData = await _configurationPanelManager.EditOrCreateObject(entity, ObjectMapper, IsAdmin, objectId, editManyAction);
        var editOrCreateViewModel = ViewModelFactory.CreateEditOrCreateViewModel(editOrCreateData, ObjectMapper);
        
        return PartialView("EditOrCreate/_EditOrCreateModal", editOrCreateViewModel);
    }

    [HttpPost]
    public async Task<JsonResult> GetSuggestions(string valueToSearch, string entity, int? generatedTypeId, int? districtId)
    {
        var ret = await _configurationPanelManager.SearchSuggestions(valueToSearch, entity, generatedTypeId, districtId);
        
        return Json(ViewHelper.GetS2ObjectsList(ret));
    }
}