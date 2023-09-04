using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.ConfigurationPanelManager.Dto;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.SelectListLoader;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.StructureTests;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.StructureTests.Dto;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.ModelsFactory;
using CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;
using CzuczenLand.ExtendingFunctionalities.Repositories.CustomRepository.Repository;
using CzuczenLand.ExtendingFunctionalities.Utils;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.ManyToManyRelations;
using Newtonsoft.Json;
using IObjectMapper = Abp.ObjectMapping.IObjectMapper;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.ConfigurationPanelManager;

/// <summary>
/// Klasa zarządzająca panelem konfiguracyjnym.
/// </summary>
public class ConfigurationPanelManager : IConfigurationPanelManager
{
    /// <summary>
    /// Repozytorium dla typów generowanych.
    /// </summary>
    private readonly IRepository<GeneratedType> _generatedTypeRepository;
    
    /// <summary>
    /// Repozytorium dla nagród.
    /// </summary>
    private readonly IRepository<Drop> _dropRepository;
    
    /// <summary>
    /// Repozytorium dla wymagań.
    /// </summary>
    private readonly IRepository<Requirement> _requirementRepository;
    
    /// <summary>
    /// Repozytorium dla tabeli łączącej nagrody z zadaniami.
    /// </summary>
    private readonly IRepository<DropQuest> _dropQuestRepository;
    
    /// <summary>
    /// Repozytorium dla postępu wymagań w zadaniach.
    /// </summary>
    private readonly IRepository<QuestRequirementsProgress> _questRequirementsProgressRepository;
    
    /// <summary>
    /// Interfejs do testów struktury.
    /// </summary>
    private readonly IStructureTests _structureTests;
    
    /// <summary>
    /// Interfejs do ładowania list wyboru.
    /// </summary>
    private readonly ISelectListLoader _selectListLoader;

    
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="generatedTypeRepository">Repozytorium dla typów generowanych.</param>
    /// <param name="dropRepository">Repozytorium dla nagród.</param>
    /// <param name="requirementRepository">Repozytorium dla wymagań.</param>
    /// <param name="dropQuestRepository">Repozytorium dla tabeli łączącej nagrody z zadaniami.</param>
    /// <param name="questRequirementsProgressRepository">Repozytorium dla postępu wymagań w zadaniach.</param>
    /// <param name="structureTests">Interfejs do testów struktury.</param>
    /// <param name="selectListLoader">Interfejs do ładowania list wyboru.</param>
    public ConfigurationPanelManager(
        IRepository<GeneratedType> generatedTypeRepository,
        IRepository<Drop> dropRepository,
        IRepository<Requirement> requirementRepository,
        IRepository<DropQuest> dropQuestRepository,
        IRepository<QuestRequirementsProgress> questRequirementsProgressRepository,
        IStructureTests structureTests,
        ISelectListLoader selectListLoader
    )
    {
        _generatedTypeRepository = generatedTypeRepository;
        _dropRepository = dropRepository;
        _requirementRepository = requirementRepository;
        _dropQuestRepository = dropQuestRepository;
        _questRequirementsProgressRepository = questRequirementsProgressRepository;
        _selectListLoader = selectListLoader;
        _structureTests = structureTests;
    }
        
    /// <summary>
    /// Formatuje logi zapisane w pliku do obiektu Logs.
    /// </summary>
    /// <returns>Obiekt Logs zawierający sformatowane logi.</returns>
    public Logs FormatLogs()
    {
        var ret = new Logs();

        var localRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory));
        var appDataPath = Path.Combine(localRoot, "App_Data");
        var logsPath = Path.Combine(appDataPath, "Logs");
        var filePath = Path.Combine(logsPath, "Logs.txt");

        using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(fs, Encoding.Default))
            {
                var buffer = new char[(int) sr.BaseStream.Length];
                sr.Read(buffer, 0, (int) sr.BaseStream.Length);

                var rawLogs = new string(buffer);
                var stringSeparators = new[] { "\r\n" };
                var lines = rawLogs.Split(stringSeparators, StringSplitOptions.None);

                var log = new List<string>();
                foreach (var line in lines)
                {
                    var isNewLog = false;
                    if (line.StartsWith("INFO"))
                    {
                        isNewLog = true;
                                
                        log = new List<string> {line};
                        ret.InfoLogs.Add(log);
                    }
                            
                    if (line.StartsWith("DEBUG"))
                    {
                        isNewLog = true;
                                
                        log = new List<string> {line};
                        ret.DebugLogs.Add(log);
                    }
                            
                    if (line.StartsWith("WARN"))
                    {
                        isNewLog = true;
                                
                        log = new List<string> {line};
                        ret.WarnLogs.Add(log);
                    }
                            
                    if (line.StartsWith("ERROR"))
                    {
                        isNewLog = true;

                        log = new List<string> {line};
                        ret.ErrorLogs.Add(log);
                    }

                    if (!isNewLog)
                        log.Add(line);
                }
            }

        ret.InfoLogs.Reverse();
        ret.DebugLogs.Reverse();
        ret.WarnLogs.Reverse();
        ret.ErrorLogs.Reverse();

        return ret;
    }
        
    /// <summary>
    /// Rozpoczyna testy struktury.
    /// </summary>
    /// <param name="isAdmin">Czy użytkownik jest administratorem.</param>
    /// <param name="userId">Identyfikator użytkownika.</param>
    /// <returns>Lista testów struktury.</returns>
    public async Task<List<StructureTest>> StartTests(bool isAdmin, long userId) => await _structureTests.BeginTests(isAdmin, userId);

    /// <summary>
    /// Ładuje dane potrzebne do wygenerowania formularza edycji lub tworzenia obiektu.
    /// </summary>
    /// <param name="entity">Nazwa encji.</param>
    /// <param name="objectMapper">Interfejs do mapowania obiektów.</param>
    /// <param name="isAdmin">Czy użytkownik jest administratorem.</param>
    /// <param name="objectId">Identyfikator obiektu (jeśli edycja).</param>
    /// <param name="editManyAction">Akcja edycji wielu obiektów (jeśli nie puste).</param>
    /// <returns>Obiekt EditOrCreate zawierający informacje o edycji/tworzeniu obiektu.</returns>
    public async Task<EditOrCreate> EditOrCreateObject(string entity, IObjectMapper objectMapper, bool isAdmin, int? objectId, string editManyAction)
    {
        var ret = new EditOrCreate();
        
        var isEdit = objectId != null;
        var obj = isEdit ? await CustomRepositoryFactory.GetRepository(entity).GetAsync(objectId) : null;
        var entityType = isEdit || !string.IsNullOrWhiteSpace(editManyAction)
            ? UpdateDtoModelFactory.GetUpdateDtoEntityTypeByEntityDbName(entity)
            : CreateDtoModelFactory.GetCreateDtoEntityTypeByEntityCreateDtoName(entity);
        var properties = entityType.GetProperties().ToList();
            
        ret.EditManyAction = editManyAction;
        ret.IsEdit = isEdit;
        ret.EntityName = entity;
        ret.EntityHrName = SelectListLoaderHelper.EntitiesNamesDbToHr.SingleOrDefault(currItem => currItem.Key == entity).Value;
        ret.Properties = properties;
            
        if (isEdit)
        {
            ret.ObjectId = objectId;
            var mappedObj = objectMapper.Map(obj, UpdateDtoModelFactory.GetUpdateDtoNewObjectByEntityUpdateDtoName(entityType.Name));
            foreach (var prop in properties)
                ret.EditingObjValues.Add(prop.GetValue(mappedObj));
        }

        ret.FieldsOptions = await _selectListLoader.GetSelectFieldsOptions(
            ret.Properties, ret.EditingObjValues, ret.EntityName, ret.IsEdit, isAdmin);
        ret.EditingObjValues = isEdit
            ? ParserFactory.GetParser(EnumUtils.ParseStrategies.Edit).Parse(ret.EditingObjValues, properties)
            : new List<object>();

        await LoadSelectors(ret, objectId, obj);

        return ret;
    }

    /// <summary>
    /// Ładuje selektory dla formularza encji, która ma możliwość ustawiania relacji Many-to-many.
    /// </summary>
    /// <param name="editOrCreate">Obiekt EditOrCreate zawierający informacje o edycji/tworzeniu obiektu.</param>
    /// <param name="objectId">Identyfikator obiektu (jeśli edycja).</param>
    /// <param name="obj">Obiekt do edycji.</param>
    private async Task LoadSelectors(EditOrCreate editOrCreate, int? objectId, object obj)
    {
        editOrCreate.ConnectionSelectors = ConfigurationPanelManagerHelper.GetEntityConnections(editOrCreate.EntityName);
        
        if (editOrCreate.IsEdit)
        {
            var drops = new List<Drop>();
            var requirements = new List<Requirement>();
            foreach (var connection in editOrCreate.ConnectionSelectors)
            {
                switch (connection.Key)
                {
                    case EntitiesDbNames.Drop:
                        var dropOptions = new List<object>();
                        var relations = await _dropQuestRepository.GetAllListAsync(item => item.QuestId == objectId);
                        var dropsIds = relations.Select(item => item.DropId);
                        foreach (var dropId in dropsIds)
                        {
                            var drop = await _dropRepository.GetAsync(dropId);
                            drops.Add(drop);
                            var option = new Select2Data{ Id = drop.Id, Text = drop.Name };
                            dropOptions.Add(option);
                        }
                            
                        editOrCreate.ExistingConnections[connection.Key] = dropOptions;
                        break;
                    case EntitiesDbNames.Requirement:
                        var requirementOptions = new List<object>();
                        var questProgress =
                            await _questRequirementsProgressRepository.SingleAsync(item =>
                                item.QuestId == ((Quest) obj).Id);
                        var requirementsProgress = JsonConvert.DeserializeObject<Dictionary<int, decimal>>(questProgress.RequirementsProgress);
                            
                        foreach (var reqProgress in requirementsProgress)
                        {
                            var req = await _requirementRepository.GetAsync(reqProgress.Key);
                            requirements.Add(req);
                            var option = new Select2Data{ Id = req.Id, Text = req.Name };
                            requirementOptions.Add(option);
                        }

                        editOrCreate.ExistingConnections[connection.Key] = requirementOptions; 
                        break;
                }
            }

            editOrCreate.ExistingConnectionsDistrictId = drops.FirstOrDefault()?.DistrictId ?? requirements.FirstOrDefault()?.DistrictId;
        }
    }

    /// <summary>
    /// Wyszukuje sugestie dla formularza encji, która ma możliwość ustawiania relacji Many-to-many.
    /// </summary>
    /// <param name="valueToSearch">Tekst do wyszukania.</param>
    /// <param name="entity">Nazwa encji.</param>
    /// <param name="generatedTypeId">Identyfikator typu generowanego.</param>
    /// <param name="districtId">Identyfikator dzielnicy.</param>
    /// <returns>Lista sugestii pasujących do wyszukiwanego tekstu.</returns>
    public async Task<List<object>> SearchSuggestions(string valueToSearch, string entity, int? generatedTypeId, int? districtId)
    {
        var ret = new List<object>();

        int currDistrictId;
        if (districtId != null)
            currDistrictId = (int) districtId;
        else if (generatedTypeId != null)
            currDistrictId = (await _generatedTypeRepository.GetAsync((int) generatedTypeId)).DistrictId;
        else
            throw new AbpException("Brak DistrictId i GeneratedTypeId. Jedno z nich musi istnieć");
            
        switch (entity)
        {
            case EntitiesDbNames.Drop:
                ret = (await _dropRepository.GetAllListAsync(item => item.DistrictId == currDistrictId)).Where(
                    item => string.IsNullOrWhiteSpace(valueToSearch) ||
                            item.Name.ToLower().Contains(valueToSearch.ToLower())).Cast<object>().ToList();
                break;
            case EntitiesDbNames.Requirement:
                ret = (await _requirementRepository.GetAllListAsync(item => item.DistrictId == currDistrictId))
                    .Where(item => string.IsNullOrWhiteSpace(valueToSearch) ||
                                   item.Name.ToLower().Contains(valueToSearch.ToLower())).Cast<object>().ToList();
                break;
        }
            
        return ret;
    }
}