using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using CzuczenLand.Authorization.Users;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.Consts.Quest;
using CzuczenLand.ExtendingFunctionalities.Consts.Requirement.Comparers;
using CzuczenLand.ExtendingFunctionalities.Consts.Requirement.Conditions;
using CzuczenLand.ExtendingFunctionalities.Consts.Requirement.CustomEntity;
using CzuczenLand.ExtendingFunctionalities.Consts.ViewParser;
using CzuczenLand.ExtendingFunctionalities.Utils;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.SelectListLoader;

/// <summary>
/// Klasa pomocnicza zawierająca mapowania nazw opcji na ich czytelne nazwy oraz metody generujące opcje pól typu SelectList w różnych kontekstach.
/// </summary>
public static class SelectListLoaderHelper
{
    /// <summary>
    /// Mapowanie nazw bazodanowych generowanych encji na ich odpowiedniki czytelne dla użytkownika.
    /// </summary>
    private static readonly Dictionary<string, string> GeneratedEntitiesNamesDbToHr = new()
    {
        {EntitiesDbNames.Seed, EntitiesHrNames.Seed},
        {EntitiesDbNames.Lamp, EntitiesHrNames.Lamp},
        {EntitiesDbNames.Manure, EntitiesHrNames.Manure},
        {EntitiesDbNames.Pot, EntitiesHrNames.Pot},
        {EntitiesDbNames.Soil, EntitiesHrNames.Soil},
        {EntitiesDbNames.Water, EntitiesHrNames.Water},
        {EntitiesDbNames.Bonus, EntitiesHrNames.Bonus},
        {EntitiesDbNames.Quest, EntitiesHrNames.Quest},
    };

    /// <summary>
    /// Mapowanie nazw bazodanowych porównań wymagań na ich odpowiedniki czytelne dla użytkownika.
    /// </summary>
    public static readonly Dictionary<string, string> RequirementComparersNamesDbToHr = new()
    {
        {DbComparers.OwnedAmount, HrComparers.OwnedAmount},
        {DbComparers.Level, HrComparers.Level},
        {DbComparers.GainedExperience, HrComparers.GainedExperience},
        {DbComparers.Gold, HrComparers.Gold},
        {DbComparers.Prestige, HrComparers.Prestige},
        {DbComparers.QuestToken, HrComparers.QuestToken},
        {DbComparers.DealerToken, HrComparers.DealerToken},
        {DbComparers.BlackMarketToken, HrComparers.BlackMarketToken},
        {DbComparers.DonToken, HrComparers.DonToken},
        {DbComparers.UnlockToken, HrComparers.UnlockToken},
        {DbComparers.Honor, HrComparers.Honor},
        {DbComparers.CompletedAmount, HrComparers.CompletedAmount},
        {DbComparers.Usages, HrComparers.Usages},
        {DbComparers.CollectPlant, HrComparers.CollectPlant},
        {DbComparers.RemovePlant, HrComparers.RemovePlant},
        {DbComparers.CreatePlant, HrComparers.CreatePlant},
        {DbComparers.SellOnBlackMarket, HrComparers.SellOnBlackMarket},
        {DbComparers.BuyOnBlackMarket, HrComparers.BuyOnBlackMarket},
    };
        
    /// <summary>
    /// Mapowanie nazw bazodanowych encji niestandardowych na ich odpowiedniki czytelne dla użytkownika.
    /// </summary>
    public static readonly Dictionary<string, string> CustomEntityNamesDbToHr = new()
    {
        {DbCustomEntityNames.PlantationStorage, HrCustomEntityNames.PlantationStorage},
        {DbCustomEntityNames.PlayerStorage, HrCustomEntityNames.PlayerStorage},
        {DbCustomEntityNames.Plant, HrCustomEntityNames.Plant},
        {DbCustomEntityNames.DriedFruit, HrCustomEntityNames.DriedFruit},
        {DbCustomEntityNames.Seed, HrCustomEntityNames.Seed},
    };

    /// <summary>
    /// Mapowanie nazw bazodanowych typów zadań na ich odpowiedniki czytelne dla użytkownika.
    /// </summary>
    public static readonly Dictionary<string, string> QuestTypesNamesDbToHr = new()
    {
        {DbQuestTypesNames.Daily, HrQuestTypesNames.Daily},
        {DbQuestTypesNames.Weekly, HrQuestTypesNames.Weekly},
        {DbQuestTypesNames.Event, HrQuestTypesNames.Event},
        {DbQuestTypesNames.Achievement, HrQuestTypesNames.Achievement},
        {DbQuestTypesNames.Others, HrQuestTypesNames.Others}
    };

    /// <summary>
    /// Mapowanie nazw bazodanowych warunków wymagań na ich odpowiedniki czytelne dla użytkownika.
    /// </summary>
    public static readonly Dictionary<string, string> RequirementConditionsNamesDbToHr = new()
    {
        {DbRequirementConditionsNames.Increase, HrRequirementConditionsNames.Increase},
        {DbRequirementConditionsNames.Decrease, HrRequirementConditionsNames.Decrease},
        {DbRequirementConditionsNames.Possession, HrRequirementConditionsNames.Possession},
        {DbRequirementConditionsNames.Deliver, HrRequirementConditionsNames.Deliver},
    };
        
    /// <summary>
    /// Mapowanie nazw bazodanowych encji na ich odpowiedniki czytelne dla użytkownika.
    /// </summary>
    public static readonly Dictionary<string, string> EntitiesNamesDbToHr = new()
    {
        {EntitiesDbNames.DriedFruit, EntitiesHrNames.DriedFruit},
        {EntitiesDbNames.Lamp, EntitiesHrNames.Lamp},
        {EntitiesDbNames.Manure, EntitiesHrNames.Manure},
        {EntitiesDbNames.Pot, EntitiesHrNames.Pot},
        {EntitiesDbNames.Seed, EntitiesHrNames.Seed},
        {EntitiesDbNames.Soil, EntitiesHrNames.Soil},
        {EntitiesDbNames.Water, EntitiesHrNames.Water},
        {EntitiesDbNames.Bonus, EntitiesHrNames.Bonus},
        {EntitiesDbNames.News, EntitiesHrNames.News},
        {EntitiesDbNames.Drop, EntitiesHrNames.Drop},
        {EntitiesDbNames.PlantationStorage, EntitiesHrNames.PlantationStorage},
        {EntitiesDbNames.Plant, EntitiesHrNames.Plant},
        {EntitiesDbNames.PlayerStorage, EntitiesHrNames.PlayerStorage},
        {EntitiesDbNames.Quest, EntitiesHrNames.Quest},
        {EntitiesDbNames.Requirement, EntitiesHrNames.Requirement},
        {EntitiesDbNames.District, EntitiesHrNames.District},
        {EntitiesDbNames.GeneratedType, EntitiesHrNames.GeneratedType},
    };
    
    /// <summary>
    /// Pobiera enum EnumUtils.Entities dla danej nazwy encji.
    /// </summary>
    /// <param name="entity">Nazwa encji.</param>
    /// <returns>Wartość EnumUtils.Entities odpowiadająca nazwie encji.</returns>
    public static EnumUtils.Entities GetEntityEnum(string entity)
    {
        switch (entity)
        {
            case EntitiesDbNames.DriedFruit:
                return EnumUtils.Entities.DriedFruit;
            case EntitiesDbNames.Lamp:
                return EnumUtils.Entities.Lamp;
            case EntitiesDbNames.Manure:
                return EnumUtils.Entities.Manure;
            case EntitiesDbNames.Pot:
                return EnumUtils.Entities.Pot;
            case EntitiesDbNames.Seed:
                return EnumUtils.Entities.Seed;
            case EntitiesDbNames.Soil:
                return EnumUtils.Entities.Soil;
            case EntitiesDbNames.Water:
                return EnumUtils.Entities.Water;
            case EntitiesDbNames.Bonus:
                return EnumUtils.Entities.Bonus;
            case EntitiesDbNames.News:
                return EnumUtils.Entities.News;
            case EntitiesDbNames.User:
                return EnumUtils.Entities.User;
            case EntitiesDbNames.Drop:
                return EnumUtils.Entities.Drop;
            case EntitiesDbNames.PlantationStorage:
                return EnumUtils.Entities.PlantationStorage;
            case EntitiesDbNames.Plant:
                return EnumUtils.Entities.Plant;
            case EntitiesDbNames.PlayerStorage:
                return EnumUtils.Entities.PlayerStorage;
            case EntitiesDbNames.Quest:
                return EnumUtils.Entities.Quest;
            case EntitiesDbNames.Requirement:
                return EnumUtils.Entities.Requirement;
            case EntitiesDbNames.District:
                return EnumUtils.Entities.District;
            case EntitiesDbNames.GeneratedType:
                return EnumUtils.Entities.GeneratedType;
            case EntitiesDbNames.DropQuest:
                return EnumUtils.Entities.DropQuest;
            default:
                throw new ArgumentOutOfRangeException(entity);
        }
    }
    
    /// <summary>
    /// Generuje listę opcji wyboru w postaci pól wyboru (checkbox).
    /// </summary>
    /// <param name="value">Wartość pola wyboru.</param>
    /// <returns>Lista pól wyboru jako SelectListItem.</returns>
    public static List<SelectListItem> GetCheckBoxAsSelectOptions(string value)
    {
        var ret = new List<SelectListItem>();

        var parsingSuccessful = bool.TryParse(value, out var parsedValue);

        var item = new SelectListItem
        {
            Text = null,
            Value = null,
            Selected = !parsingSuccessful
        };
        
        ret.Add(item);

        var item1 = new SelectListItem
        {
            Text = "Tak",
            Value = "true",
            Selected = parsedValue && parsingSuccessful
        };
                       
        ret.Add(item1);

        var item2 = new SelectListItem
        {
            Text = "Nie",
            Value = "false",
            Selected = !parsedValue && parsingSuccessful
        };

        ret.Add(item2);

        return ret;
    }
        
    /// <summary>
    /// Generuje opcje wyboru użytkowników.
    /// </summary>
    /// <param name="users">Lista użytkowników.</param>
    /// <param name="properties">Lista właściwości.</param>
    /// <param name="values">Lista wartości.</param>
    /// <returns>Lista pól typu SelectList dla użytkowników.</returns>
    public static List<SelectListItem> GetUsersAsSelectOptions(List<User> users, List<PropertyInfo> properties, List<object> values)
    {
        var fieldIndex = properties.FindIndex(item => item.Name == RelationFieldsNames.UserId);
        return users.Select(user => new SelectListItem
            {
                Text = user.Name,
                Value = user.Id.ToString(),
                Selected = values?.Count > 0 && user.Id == (long?) values[fieldIndex],
            })
            .ToList();
    }

    /// <summary>
    /// Generuje opcje wyboru typów generowanych.
    /// </summary>
    /// <param name="availableGeneratedTypes">Lista dostępnych typów generowanych.</param>
    /// <param name="fieldValue">Id typu generowanego.</param>
    /// <returns>Lista pól typu SelectList dla typów wygenerowanych.</returns>
    public static List<SelectListItem> GetGeneratedTypesAsSelectOptions(List<GeneratedType> availableGeneratedTypes, int? fieldValue)
    {
        return availableGeneratedTypes.Select(generatedType => new SelectListItem
        {
            Text = generatedType.Name, 
            Value = generatedType.Id.ToString(),
            Selected = generatedType.Id == fieldValue
        }).ToList();
    }

    /// <summary>
    /// Generuje opcje wyboru nazw encji generowanych.
    /// </summary>
    /// <param name="properties">Lista właściwości.</param>
    /// <param name="values">Lista wartości.</param>
    /// <returns>Lista pól typu SelectList dla nazw encji generowanych.</returns>
    public static List<SelectListItem> GetGeneratedEntitiesAsSelectOptions(List <PropertyInfo> properties, List<object> values)
    {
        var fieldIndex = properties.FindIndex(item => item.Name == RelationFieldsNames.EntityName);
        return GeneratedEntitiesNamesDbToHr.Select(entityName => new SelectListItem
        {
            Text = entityName.Value, 
            Value = entityName.Key,
            Selected = values?.Count > 0 && entityName.Key == values[fieldIndex]?.ToString()
        }).ToList();
    }
     
    /// <summary>
    /// Generuje opcje wyboru typów zadań.
    /// </summary>
    /// <param name="properties">Lista właściwości.</param>
    /// <param name="values">Lista wartości.</param>
    /// <returns>Lista pól typu SelectList dla typów zadań.</returns>
    public static List<SelectListItem> GetQuestTypesAsSelectOptions(List <PropertyInfo> properties, List<object> values)
    {
        var fieldIndex = properties.FindIndex(item => item.Name == RelationFieldsNames.QuestType);
        return QuestTypesNamesDbToHr.Select(entityName => new SelectListItem
        {
            Text = entityName.Value, 
            Value = entityName.Key,
            Selected = values?.Count > 0 && entityName.Key == values[fieldIndex]?.ToString()
        }).ToList();
    }
        
    /// <summary>
    /// Generuje opcje wyboru warunków wymagań.
    /// </summary>
    /// <param name="properties">Lista właściwości.</param>
    /// <param name="values">Lista wartości.</param>
    /// <returns>Lista pól typu SelectList dla warunków wymagań.</returns>
    public static List<SelectListItem> GetRequirementConditionsAsSelectOptions(List <PropertyInfo> properties, List<object> values)
    {
        var fieldIndex = properties.FindIndex(item => item.Name == RelationFieldsNames.Condition);
        return RequirementConditionsNamesDbToHr.Select(entityName => new SelectListItem
        {
            Text = entityName.Value, 
            Value = entityName.Key,
            Selected = values?.Count > 0 && entityName.Key == values[fieldIndex]?.ToString()
        }).ToList();
    }
        
    /// <summary>
    /// Generuje opcje wyboru porównań wymagań.
    /// </summary>
    /// <param name="properties">Lista właściwości.</param>
    /// <param name="values">Lista wartości.</param>
    /// <returns>Lista pól typu SelectList dla porównań wymagań.</returns>
    public static List<SelectListItem> GetRequirementComparersAsSelectOptions(List <PropertyInfo> properties, List<object> values)
    {
        var fieldIndex = properties.FindIndex(item => item.Name == RelationFieldsNames.Comparer);
        return RequirementComparersNamesDbToHr.Select(entityName => new SelectListItem
        {
            Text = entityName.Value, 
            Value = entityName.Key,
            Selected = values?.Count > 0 && entityName.Key == values[fieldIndex]?.ToString()
        }).ToList();
    }
        
    /// <summary>
    /// Generuje opcje wyboru niestandardowych encji.
    /// </summary>
    /// <param name="properties">Lista właściwości.</param>
    /// <param name="values">Lista wartości.</param>
    /// <returns>Lista pól typu SelectList dla niestandardowych encji.</returns>
    public static List<SelectListItem> GetCustomEntitiesAsSelectOptions(List <PropertyInfo> properties, List<object> values)
    {
        var fieldIndex = properties.FindIndex(item => item.Name == RelationFieldsNames.CustomEntityName);
        return CustomEntityNamesDbToHr.Select(entityName => new SelectListItem
        {
            Text = entityName.Value, 
            Value = entityName.Key,
            Selected = values?.Count > 0 && entityName.Key == values[fieldIndex]?.ToString()
        }).ToList();
    }
}