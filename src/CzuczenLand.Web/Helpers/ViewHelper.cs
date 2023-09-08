using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Abp.Extensions;
using Abp.UI;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.StructureTests;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.StructureTests.Dto;
using CzuczenLand.ExtendingFunctionalities.Consts;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.SharedEntitiesFieldsNames.Db;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.SharedEntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.PlantationManager;
using CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;
using CzuczenLand.ExtendingFunctionalities.Utils;
using CzuczenLand.ExtendingModels.Interfaces;
using CzuczenLand.Web.Models.Account;
using CzuczenLand.Web.Models.ExtendingModels.ConfigurationPanel;

namespace CzuczenLand.Web.Helpers;

public static class ViewHelper
{
    private static readonly List<string> NotRenderedProductInfoFields = new()
    {
        "", // puste to pole id
        ProductFieldsHrNames.Name, // nazwa jest renderowana jako pierwsza dlatego nie ma być później już renderowana
        PlantFieldsHrNames.GrowingSpeedDivider,
        PlantFieldsHrNames.GrowingLevel,
        PlantFieldsHrNames.WiltLevel,
        PlantFieldsHrNames.TimeOfInsensitivity,
        PlantFieldsHrNames.TimeRemaining,
        PlantFieldsHrNames.ImageUrl,
    };

    public static MvcHtmlString CreateProductInfo(object product)
    {
        var ret = "";
        
        if (product == null) 
            return MvcHtmlString.Create(ret);
    
        var generatedType = product.GetType();
        var nameProp = generatedType.GetProperty(EntitiesFieldsDbNames.Name);
        var productProperties = generatedType.GetProperties();
        var propsValues = generatedType.GetProperties().Select(currProp => currProp.GetValue(product)).ToList();
        var propsNames = PlantationManagerHelper.GetHrPropList(generatedType);
        var displayParser = ParserFactory.GetParser(EnumUtils.ParseStrategies.Display);
        var propName = propsNames.SingleOrDefault(item => item == ProductFieldsHrNames.Name);

        if (propName == ProductFieldsHrNames.Name)
        {
            var nameIndex = propsNames.FindIndex(item => item == ProductFieldsHrNames.Name);
            var validNameFieldValue =  displayParser.Parse(nameProp, propsValues[nameIndex]);
            ret += $"<span>{propName}: {validNameFieldValue}</span>";
        }

        for (var i = 0; i < propsNames.Count; i++)
        {
            if (NotRenderedProductInfoFields.Any(currName => currName == propsNames[i])) continue;
                
            var validFieldValue = displayParser.Parse(productProperties[i], propsValues[i]);
            ret += $"<span>{propsNames[i]}: {validFieldValue}</span>";
        }

        return MvcHtmlString.Create(ret);
    }

    public static string GetInputMaxValueForProduct(string itemEntityName)
    {
        string ret;
        switch (itemEntityName)
        {
            case EntitiesDbNames.DriedFruit:
                ret = "100";
                break;
            case EntitiesDbNames.Lamp:
                ret = "100";
                break;
            case EntitiesDbNames.Manure:
                ret = "1000";
                break;
            case EntitiesDbNames.Pot:
                ret = "100";
                break;
            case EntitiesDbNames.Seed:
                ret = "100";
                break;
            case EntitiesDbNames.Soil:
                ret = "100";
                break;
            case EntitiesDbNames.Water:
                ret = "1000";
                break;
            case EntitiesDbNames.Bonus:
                ret = "100";
                break;
            default:
                throw new ArgumentOutOfRangeException(itemEntityName);
        }

        return ret;
    }
    
    public static string GetInputMaxValueForCurrencyExchange(string currencyName)
    {
        var ret = "100";

        switch (currencyName)
        {
            case PlantationStorageObservedFields.Gold:
                ret = "10000";
                break;
            case PlantationStorageObservedFields.QuestToken:
                ret = "100";
                break;
            case PlantationStorageObservedFields.DealerToken:
                ret = "100";
                break;
            case PlantationStorageObservedFields.BlackMarketToken:
                ret = "100";
                break;
            case PlantationStorageObservedFields.DonToken:
                ret = "100";
                break;
            case PlantationStorageObservedFields.UnlockToken:
                ret = "100";
                break;
            case PlantationStorageObservedFields.Prestige:
                ret = "100";
                break;
        }

        return ret;
    }
    
    public static string GetInputStepForCurrencyExchange(string currencyName)
    {
        var ret = "0.01";

        switch (currencyName)
        {
            case PlantationStorageObservedFields.Gold:
                ret = "0.01";
                break;
            case PlantationStorageObservedFields.QuestToken:
                ret = "1";
                break;
            case PlantationStorageObservedFields.DealerToken:
                ret = "1";
                break;
            case PlantationStorageObservedFields.BlackMarketToken:
                ret = "1";
                break;
            case PlantationStorageObservedFields.DonToken:
                ret = "1";
                break;
            case PlantationStorageObservedFields.UnlockToken:
                ret = "1";
                break;
            case PlantationStorageObservedFields.Prestige:
                ret = "1";
                break;
        }

        return ret;
    }

    public static string GetUserNameFromIdentity(string userName)
    {
        var ret = userName?.Replace("\"", "");
            
        return ret;
    }

    public static string GetTypeAttr(PropertyInfo prop)
    {
        var ret = "";
            
        var propType = ParserHelper.GetPropType(prop);
        switch (propType)
        {
            case EnumUtils.PropTypes.Decimal:
                ret = "type=number min=0 step=0,01 data-rule-multipleOf01=true";
                break;
            case EnumUtils.PropTypes.Int:
            case EnumUtils.PropTypes.Long:
                ret = "type=number min=0 step=1";
                break;
            case EnumUtils.PropTypes.DateTime:
                ret = "type=datetime-local";
                break;
            case EnumUtils.PropTypes.String:
                ret = "type=text";
                break;
            case EnumUtils.PropTypes.Boolean:
                ret = "type=checkbox";
                break;
        }

        return ret;
    }

    /// <summary>
    /// Tworzenie adresu e-mail jest hardkodowane, żeby nie pobierać danych wrażliwych od użytkownika
    /// Adres e-mail jest tworzony na podstawie Nick
    /// Dlatego potrzebna jest korekta wiadomości
    /// </summary>
    /// <param name="ex"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public static string GetCorrectMessage(UserFriendlyException ex, RegisterViewModel model)
    {
        string ret;

        if (ex.Message.Contains("jest już zajęt"))
            ret = "Nick " + model.UserName + " jest już zajęty.";
        else if (ex.Message.Contains("jest nieprawidłowa, może zawierać tylko litery i cyfry."))
            ret = "Nazwa użytkownika " + model.UserName + " jest nieprawidłowa, może zawierać tylko litery, cyfry i podkreślenia.";
        else
            ret = ex.Message;

        return ret;
    }

    public static bool? CheckInputRenderConditions(string propName, EditOrCreateViewModel editOrCreateViewModel)
    {
        bool? ret;
            
        if (propName == EntitiesFieldsDbNames.Name) return null;

        if (!editOrCreateViewModel.IsEdit && !editOrCreateViewModel.IsManyEdit)
            ret = true;
        else if (editOrCreateViewModel.IsManyEdit && propName == EntitiesFieldsDbNames.Id)
            ret = null;
        else if (editOrCreateViewModel.IsEdit)
            ret = propName != EntitiesFieldsDbNames.Id;
        else
            ret = true;

        return ret;
    }

    public static bool CheckNameInputRenderCondition(EditOrCreateViewModel model)
    {
        var ret = false;
            
        var nameProp = model.Properties?.SingleOrDefault(item => item.Name == EntitiesFieldsDbNames.Name);
        if (nameProp != null)
        {
            var indexOfNameField = model.Properties.FindIndex(item => item.Name == EntitiesFieldsDbNames.Name);
            model.CurrProp = nameProp;
            
            if (model.EditingObjValues?.Count > 0)
                model.CurrValue = (string) model.EditingObjValues?[indexOfNameField];

            ret = true;
        }

        return ret;
    }

    public static MvcHtmlString GenerateStructureTestsTable(StructureTest test)
    {
        var ret = "";
            
        ret += "<table class='table-bordered table-hover table-responsive table-striped d-inline-block'>";
        ret += "<tr>";
        ret += "<th>Status</th>";
        ret += "<th>Opis</th>";
        ret += "</tr>";

        foreach (var subTest in test.SubTests)
        {
            var status = subTest.Status;

            var statusColor = StructureTestsHelper.GetStatusColor(status);
            var statusText = StructureTestsHelper.GetStatusText(status);

            ret += "<tr>";
            ret += $"<td class='font-bold' style='background-color: {statusColor}'>{statusText}</td>";
            ret += $"<td class='align-left'>{subTest.Description}</td>";
            ret += "</tr>";
        }
            
        ret += "<tr>";
        ret += "<th>Dodatkowe informacje</th>";
        ret += $"<th>Ilość testów: {test.SubTests.Count}</br>{test.AdditionalInfos}</th>";
        ret += "</tr>";
        ret += "</table>";

        return MvcHtmlString.Create(ret);
    }
        
    public static MvcHtmlString GenerateDistrictsCloneTable(List<object> objects)
    {
        var ret = "";
        
        if (objects == null || !objects.Any())
            return MvcHtmlString.Create(ret);

        var type = objects.First().GetType();
        var properties = type.GetProperties();

        ret += "<table>";
            
        foreach (var obj in objects)
        {
            var nameOrIdProp = properties.SingleOrDefault(item => item.Name == EntitiesFieldsDbNames.Name) ??
                               properties.SingleOrDefault(item => item.Name == EntitiesFieldsDbNames.Id);
            var nameOrId = nameOrIdProp?.GetValue(obj);
                
            ret += "<tr style='border: 6px double #1C6EA4;'>";
            ret += "<th>Encja</th>";
            ret += "<th>Nazwa</th>";
            ret += "</tr>";
                
            ret += "<tr>";
            ret += $"<td>{type.Name}</td>";
            ret += $"<td>{nameOrId}</td>";
            ret += "</tr>";   
                
            ret += "<tr style='border: 2px dashed rgba(66,164,64,0.35);'>";
            ret += "<th>Nazwa pola</th>";
            ret += "<th>Wartość</th>";
            ret += "</tr>";

            foreach (var prop in properties)
            {
                ret += "<tr style='background-color: whitesmoke;'>";
                ret += $"<td>{prop.Name}</td>";
                ret += $"<td style='overflow-wrap: anywhere;'>{prop.GetValue(obj)}</td>";
                ret += "</tr>";   
            }
        }

        ret += "</table>";

        return MvcHtmlString.Create(ret);
    }

    public static List<Select2Data> GetS2ProductsList(List<object> objects)
    {
        var ret = new List<Select2Data>();
        var tempObjects = objects?.Cast<IS2Product>().ToList();
        
        if (tempObjects == null || tempObjects.Count <= 0) return ret;

        var measureUnit = PlantationManagerHelper.GetMeasureUnitByType(objects[0]?.GetType());
        ret.AddRange(tempObjects.Select(item => new Select2Data {Id = item.Id, Text = item.Name + "   Ilość: " + item.OwnedAmount + measureUnit}));

        return ret;
    }
    
    public static List<Select2Data> GetS2ObjectsList(List<object> objects)
    {
        var ret = new List<Select2Data>();
        
        var tempObjects = objects?.Cast<INamedEntity>().ToList();
        
        if (tempObjects == null || tempObjects.Count <= 0) 
            return ret;
            
        ret.AddRange(tempObjects.Select(item => new Select2Data {Id = item.Id, Text = item.Name}));

        return ret;
    }
    
    // TODO: SLT - Separator liczb tysięcznych. Zrobić obsługę przerw dla liczb tysięcznych w formularzach dla edycji jak w DisplayStrategy -> ParseIntAndLongToFormattedSeparator i ParseDecimalToFormattedSeparator. Będzie trzeba uzupełnić parsowania dla tego podejścia. Sprawdzić czy można by wykorzystać - https://webdesign.tutsplus.com/tutorials/auto-formatting-input-value--cms-26745
    // public static string GetInputFormat(PropertyInfo prop, object value)
    // {
    //     string ret;
    //     
    //     var propType = EntitiesUtils.GetPropType(prop);
    //     var nfi = (NumberFormatInfo) CultureInfo.InvariantCulture.NumberFormat.Clone();
    //     nfi.NumberGroupSeparator = " ";
    //     
    //     switch (propType)
    //     {
    //         case EnumUtils.PropTypes.Decimal:
    //             ret = StringUtils.ParseDecimalToFormattedSeparator(value).Replace(".", ",");
    //             break;
    //         case EnumUtils.PropTypes.Int:
    //             ret = StringUtils.ParseIntAndLongToFormattedSeparator(value);
    //             break;
    //         case EnumUtils.PropTypes.Long:
    //             ret = StringUtils.ParseIntAndLongToFormattedSeparator(value);
    //             break;
    //         case EnumUtils.PropTypes.DateTime:
    //             ret =  DateTime.Parse(value.ToString()).ToLocalTime().ToString("G");
    //             break;
    //         default:
    //             ret = string.Empty;
    //             break;
    //     }
    //     
    //     return ret;
    // }
}
