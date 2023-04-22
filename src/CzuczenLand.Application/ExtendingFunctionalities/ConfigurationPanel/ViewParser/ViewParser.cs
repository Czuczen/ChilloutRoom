using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Castle.Core.Logging;
using CzuczenLand.Authorization.Users;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.SelectListLoader;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.Consts.ViewParser;
using CzuczenLand.ExtendingFunctionalities.Repositories.CustomRepository.Loader;
using CzuczenLand.ExtendingFunctionalities.Utils;
using Newtonsoft.Json;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.ViewParser;

public class ViewParser : IViewParser
{
    private readonly IRepository<User, long> _userRepository;
    private readonly ICustomRepositoryLoader _customRepositoryLoader;

    public ILogger Logger { get; set; }
        
    
    public ViewParser(
        IRepository<User, long> userRepository,
        ICustomRepositoryLoader customRepositoryLoader
    )
    {
        Logger = NullLogger.Instance;
        _userRepository = userRepository;
        _customRepositoryLoader = customRepositoryLoader;
    }
        
    public async Task<List<object>> ParseObjectsValues(List<object> dtoObjList, List<PropertyInfo> properties)
    {
        var ret = new List<Dictionary<string, object>>();
        
        if (dtoObjList == null || dtoObjList.Count == 0 || properties == null || properties.Count == 0) 
            return ret.Cast<object>().ToList();

        var records = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(JsonConvert.SerializeObject(dtoObjList));
        var displayParser = ParserFactory.GetParser(EnumUtils.ParseStrategies.Display);
        var relationFieldsNames = ViewParserHelper.GetRelationFields(properties).Select(item => item.Name).ToList();
        
        foreach (var obj in records)
        {
            var newObj = new Dictionary<string, object>();
            foreach (var objElem in obj)
            {
                var prop = properties.Single(item => item.Name == objElem.Key);
                if (relationFieldsNames.Any(item => item == objElem.Key))
                    newObj[objElem.Key] = await ParseRelationFieldToName(prop, objElem);
                else
                    newObj[objElem.Key] = displayParser.Parse(prop, objElem.Value);
            }
            
            ret.Add(newObj);
        }

        return ret.Cast<object>().ToList();
    }

    private async Task<string> ParseRelationFieldToName(PropertyInfo prop, KeyValuePair<string, object> objElem)
    {
        var ret = "";
        
        if (objElem.Value == null) return "Brak";
        if (objElem.Value.ToString() == "") return ret;

        switch (prop.Name)
        {
            case RelationFieldsNames.EntityName:
                return SelectListLoaderHelper.EntitiesNamesDbToHr[objElem.Value.ToString()];
            case RelationFieldsNames.Condition:
                return SelectListLoaderHelper.RequirementConditionsNamesDbToHr[objElem.Value.ToString()];
            case RelationFieldsNames.QuestType:
                return SelectListLoaderHelper.QuestTypesNamesDbToHr[objElem.Value.ToString()];
            case RelationFieldsNames.Comparer:
                return SelectListLoaderHelper.RequirementComparersNamesDbToHr[objElem.Value.ToString()];
            case RelationFieldsNames.CustomEntityName:
                return SelectListLoaderHelper.CustomEntityNamesDbToHr[objElem.Value.ToString()];
        }
            
        var entityName = ViewParserHelper.GetRelationFieldEntityName(prop);
        var isUserEntity = entityName == EntitiesDbNames.User;
        var value = isUserEntity
            ? (object) long.Parse(objElem.Value.ToString())
            : (object) int.Parse(objElem.Value.ToString());

        try
        {
            ret = isUserEntity
                ? (await _userRepository.GetAsync((long) value)).Name
                : await _customRepositoryLoader.GetObjectName(entityName, (int) value);
        }
        catch (Exception ex)
        {
            if (ex is InvalidOperationException && ex.Message.Contains("Sequence contains no matching element"))
            {
                ret = "Nie istnieje";
                Logger.Warn("Nie istnieje. Encja - " + entityName + ". Id: " + value, ex);
            }
            else if (ex is EntityNotFoundException && ex.Message.Contains("There is no such an entity"))
            {
                ret = "Nie istnieje";
                Logger.Warn("Nie istnieje. Encja - " + entityName + ". Id: " + value, ex);
            }
            else
                Logger.Error("Blad =======//========", ex);
        }
            
        return ret;
    }
}