using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using CzuczenLand.Authorization;
using CzuczenLand.Authorization.Users;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.ViewParser;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.Consts.ViewParser;
using CzuczenLand.ExtendingFunctionalities.Repositories.CustomRepository.Loader;
using CzuczenLand.ExtendingFunctionalities.Utils;
using CzuczenLand.ExtendingModels.Interfaces;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.SelectListLoader;

/// <summary>
/// Klasa odpowiedzialna za ładowanie danych do pola listy wyboru formularza (SelectList) w panelu konfiguracji.
/// </summary>
public class SelectListLoader : ISelectListLoader
{
    /// <summary>
    /// Repozytorium suszu.
    /// </summary>
    private readonly IRepository<DriedFruit> _driedFruitRepository;
    
    /// <summary>
    /// Repozytorium dzielnic.
    /// </summary>
    private readonly IRepository<District> _districtRepository;
    
    /// <summary>
    /// Repozytorium typów generowanych.
    /// </summary>
    private readonly IRepository<GeneratedType> _generatedTypeRepository;
    
    /// <summary>
    /// Repozytorium użytkowników.
    /// </summary>
    private readonly IRepository<User, long> _userRepository;
    
    /// <summary>
    /// Kontroluje tworzenie niestandardowych repozytoriów.
    /// </summary>
    private readonly ICustomRepositoryLoader _customRepositoryLoader;
    
    /// <summary>
    /// Obiekt do sprawdzania uprawnień.
    /// </summary>
    private readonly IPermissionChecker _permissionChecker;
    
    

    /// <summary>
    /// Właściwość pozwalająca na uzyskanie dostępu do sesji Abp, która przechowuje informacje dotyczące aktualnie zalogowanego użytkownika.
    /// Właściwość musi być public oraz mieć getter i setter dla poprawnego działania wstrzykiwania właściwości.
    /// </summary>
    public IAbpSession AbpSession { get; set; }
        
    /// <summary>
    /// Interfejs ILogger służy do rejestrowania komunikatów z aplikacji.
    /// Właściwość musi być public oraz mieć getter i setter dla poprawnego działania wstrzykiwania właściwości.
    /// </summary>
    public ILogger Logger { get; set; }
        
    
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="driedFruitRepository">Repozytorium suszu.</param>
    /// <param name="districtRepository">Repozytorium dzielnic.</param>
    /// <param name="generatedTypeRepository">Repozytorium typów generowanych.</param>
    /// <param name="userRepository">Repozytorium użytkowników.</param>
    /// <param name="customRepositoryLoader">Kontroluje tworzenie niestandardowych repozytoriów.</param>
    /// <param name="permissionChecker">Obiekt do sprawdzania uprawnień.</param>
    public SelectListLoader(
        IRepository<DriedFruit> driedFruitRepository,
        IRepository<District> districtRepository,
        IRepository<GeneratedType> generatedTypeRepository,
        IRepository<User, long> userRepository,
        ICustomRepositoryLoader customRepositoryLoader,
        IPermissionChecker permissionChecker
    )
    {
        Logger = NullLogger.Instance;
        AbpSession = NullAbpSession.Instance;
        _driedFruitRepository = driedFruitRepository;
        _districtRepository = districtRepository;
        _generatedTypeRepository = generatedTypeRepository;
        _userRepository = userRepository;
        _customRepositoryLoader = customRepositoryLoader;
        _permissionChecker = permissionChecker;
    }

    /// <summary>
    /// Pobiera opcje dla pól typu SelectList.
    /// </summary>
    /// <param name="properties">Lista właściwości.</param>
    /// <param name="values">Lista wartości obiektu.</param>
    /// <param name="entity">Nazwa encji.</param>
    /// <param name="isEdit">Czy edycja.</param>
    /// <param name="isAdmin">Czy administrator.</param>
    /// <returns>Słownik opcji pól typu SelectList.</returns>
    public async Task<Dictionary<string, object>> GetSelectFieldsOptions(List<PropertyInfo> properties, List<object> values, string entity, bool isEdit, bool isAdmin)
    {
        var ret = new Dictionary<string, object>();
        var relationFields = ViewParserHelper.GetRelationFields(properties);
        
        foreach (var prop in relationFields)
        {
            ret[prop.Name] = new List<SelectListItem>();
            try
            {
                switch (prop.Name)
                {
                    case RelationFieldsNames.GeneratedTypeId:
                    {
                        var fieldIndex = properties.FindIndex(item => item.Name == RelationFieldsNames.GeneratedTypeId);
                        var fieldValue = values?.Count > 0 ? (int?) values[fieldIndex] : null;
                        var availableGeneratedTypes = await GetAvailableGeneratedTypes(entity, fieldValue);
                        ret[prop.Name] = SelectListLoaderHelper.GetGeneratedTypesAsSelectOptions(availableGeneratedTypes, fieldValue);
                        break;
                    }
                    case RelationFieldsNames.EntityName:
                        ret[prop.Name] = SelectListLoaderHelper.GetGeneratedEntitiesAsSelectOptions(properties, values);
                        break;
                    case RelationFieldsNames.QuestType:
                        ret[prop.Name] = SelectListLoaderHelper.GetQuestTypesAsSelectOptions(properties, values);
                        break;
                    case RelationFieldsNames.Condition:
                        ret[prop.Name] = SelectListLoaderHelper.GetRequirementConditionsAsSelectOptions(properties, values);
                        break;
                    case RelationFieldsNames.Comparer:
                        ret[prop.Name] = SelectListLoaderHelper.GetRequirementComparersAsSelectOptions(properties, values);
                        break;
                    case RelationFieldsNames.CustomEntityName:
                        ret[prop.Name] = SelectListLoaderHelper.GetCustomEntitiesAsSelectOptions(properties, values);
                        break;
                    case RelationFieldsNames.UserId:
                    {
                        var users = await GetAvailableUsersForSelectOptions(entity, isEdit, properties, values, isAdmin);
                        ret[prop.Name] = SelectListLoaderHelper.GetUsersAsSelectOptions(users, properties, values);
                        break;
                    }
                    case RelationFieldsNames.DistrictId when !isAdmin:
                    {
                        var userId = AbpSession.GetUserId();
                        var district = (await _districtRepository.GetAllListAsync(currDistrict => currDistrict.UserId == userId)).SingleOrDefault();
                        if (district == null) break;
                            
                        ret[prop.Name] = new List<SelectListItem>
                        {
                            new() {Text = district.Name, Value = district.Id.ToString(), Selected = true}
                        };
                        break;
                    }
                    default:
                    {
                        var entityName = ViewParserHelper.GetRelationFieldEntityName(prop);
                        var allItems = (await _customRepositoryLoader.GetRepository(entityName).GetAllListAsync()).Cast<INamedEntity>();
                        var fieldIndex = properties.FindIndex(item => item.Name == prop.Name);
                        var options = allItems.Select(currObj => new SelectListItem
                        {
                            Text = currObj.Name, 
                            Value = currObj.Id.ToString(),
                            Selected = values?.Count > 0 && currObj.Id == (int?) values[fieldIndex]
                        }).ToList();

                        ret[prop.Name] = options;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Blad=====/=====", ex);
            }
        }
            
        return ret;
    }

    /// <summary>
    /// Pobiera dostępne typy generowane dla danej encji.
    /// </summary>
    /// <param name="entity">Nazwa encji obiektu.</param>
    /// <param name="updatingObjGeneratedTypeid">Id typu generowanego aktualizowanego obiektu.</param>
    /// <returns>Lista dostępnych typów generowanych.</returns>
    private async Task<List<GeneratedType>> GetAvailableGeneratedTypes(string entity, int? updatingObjGeneratedTypeid)
    {
        var ret = new List<GeneratedType>();
        var entityEnum = SelectListLoaderHelper.GetEntityEnum(entity);
        var filteredGeneratedTypes = await GetFilteredGeneratedTypes(entity);
        
        if (entity == EntitiesDbNames.Drop || entity == EntitiesDbNames.Requirement) return filteredGeneratedTypes;
            
        foreach (var generatedType in filteredGeneratedTypes)
        {
            var typeEntityEnum = SelectListLoaderHelper.GetEntityEnum(generatedType.EntityName);
            if (entityEnum == EnumUtils.Entities.DriedFruit && typeEntityEnum == EnumUtils.Entities.Seed)
            {
                var driedFruitUseThisType = (await _driedFruitRepository.GetAllListAsync(item =>
                    item.GeneratedTypeId == generatedType.Id && item.PlantationStorageId == null)).SingleOrDefault();
                    
                if (driedFruitUseThisType == null) 
                    ret.Add(generatedType);
            }
            else
            {
                var typeIsUsed = false;
                var allItems = (await _customRepositoryLoader.GetRepository(generatedType.EntityName).GetAllListAsync()).Cast<IGeneratedEntity>().ToList();
                foreach (var item in allItems)
                    if (generatedType.Id == item.GeneratedTypeId && generatedType.Id != updatingObjGeneratedTypeid) 
                        typeIsUsed = true;
                
                if (!typeIsUsed) 
                    ret.Add(generatedType);
            }
        }
            
        return ret;
    }

    /// <summary>
    /// Pobiera dostępnych użytkowników dla pola typu SelectList.
    /// </summary>
    /// <param name="entity">Nazwa encji.</param>
    /// <param name="isEdit">Czy edycja.</param>
    /// <param name="properties">Lista właściwości.</param>
    /// <param name="values">Lista wartości obiektu.</param>
    /// <param name="currUserIsAdmin">Czy bieżący użytkownik jest administratorem.</param>
    /// <returns>Lista dostępnych użytkowników.</returns>
    private async Task<List<User>> GetAvailableUsersForSelectOptions(string entity, bool isEdit, List<PropertyInfo> properties, List<object> values, bool currUserIsAdmin)
    {
        var ret = new List<User>();
     
        if (isEdit)
        {
            // Aby użytkownik, który jest już na edytowanym obiekcie też był sugerowany
            var userIdPropIndex = properties.FindIndex(item => item.Name == RelationFieldsNames.UserId);
            var userId = (long) values[userIdPropIndex];
            try
            {
                var userFromObject = await _userRepository.GetAsync(userId);
                ret.Add(userFromObject);
            }
            catch (Exception ex)
            {
                if (ex is EntityNotFoundException && ex.Message.Contains("There is no such an entity"))
                    Logger.Warn("Nie istnieje. Encja - " + entity + ". Id: " + userId, ex);
                else
                    Logger.Error("Blad =======//========", ex);
            }
        }
            
        if (currUserIsAdmin && entity == EntitiesDbNames.District)
        {
            var allUsers = await _userRepository.GetAllListAsync();
            var allDistricts = await _districtRepository.GetAllListAsync();
            
            // Wszyscy opiekunowie, którzy nie są przypisani do dzielnicy
            foreach (var user in allUsers)
            {
                var isAdmin = await _permissionChecker.IsGrantedAsync(user.ToUserIdentifier(), PermissionNames.Crud_Admin);
                var isDistrictWarden = await _permissionChecker.IsGrantedAsync(user.ToUserIdentifier(), PermissionNames.Crud_DistrictWarden);
                var userDistrict = allDistricts.SingleOrDefault(item => item.UserId == user.Id);
                if (userDistrict == null && isDistrictWarden && !isAdmin)
                    ret.Add(user);
            }
        }
   
        return ret;
    }

    /// <summary>
    /// Pobiera przefiltrowane typy generowane dla danej encji.
    /// </summary>
    /// <param name="entity">Nazwa encji.</param>
    /// <returns>Lista przefiltrowanych typów generowanych.</returns>
    private async Task<List<GeneratedType>> GetFilteredGeneratedTypes(string entity)
    {
        var ret = new List<GeneratedType>();

        var userId = AbpSession.GetUserId();
        var wardenDistrict = (await _districtRepository.GetAllListAsync(item => item.UserId == userId)).SingleOrDefault();
        if (wardenDistrict == null) return ret;
            
        if (entity == EntitiesDbNames.Drop || entity == EntitiesDbNames.Requirement)
        {
            ret = await _generatedTypeRepository.GetAllListAsync(item => item.DistrictId == wardenDistrict.Id);
        }
        else
        {
            ret = await _generatedTypeRepository.GetAllListAsync(item =>
                (item.EntityName == entity && item.DistrictId == wardenDistrict.Id) 
                || 
                item.EntityName == EntitiesDbNames.Seed && entity == EntitiesDbNames.DriedFruit 
                                                        && item.DistrictId == wardenDistrict.Id);
        }
            
        return ret;
    }
}