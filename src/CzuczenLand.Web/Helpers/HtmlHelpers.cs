using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.SelectListLoader;
using CzuczenLand.Web.Models.ExtendingModels.ConfigurationPanel;
using CzuczenLand.Web.Models.ExtendingModels.Plantation;
using CzuczenLand.Web.Models.ExtendingModels.Plantation.GameStorage;
using CzuczenLand.Web.Models.ExtendingModels.Plantation.QuestInfo;
using Newtonsoft.Json;

namespace CzuczenLand.Web.Helpers;

public static class HtmlHelpers
{
    private class ScriptBlock : IDisposable
    {
        private const string ScriptsKey = "PartialViewScripts";
        public static List<string> PartialViewScripts
        {
            get
            {
                if (HttpContext.Current.Items[ScriptsKey] == null)
                    HttpContext.Current.Items[ScriptsKey] = new List<string>();
                return (List<string>)HttpContext.Current.Items[ScriptsKey];
            }
        }

        readonly WebViewPage _webPageBase;

        public ScriptBlock(WebViewPage webPageBase)
        {
            _webPageBase = webPageBase;
            _webPageBase.OutputStack.Push(new StringWriter());
        }

        public void Dispose()
        {
            PartialViewScripts.Add(((StringWriter)_webPageBase.OutputStack.Pop()).ToString());
        }
    }

    public static IDisposable BeginScripts(this HtmlHelper helper)
    {
        return new ScriptBlock((WebViewPage)helper.ViewDataContainer);
    }

    public static MvcHtmlString PartialViewScripts(this HtmlHelper helper)
    {
        return MvcHtmlString.Create(string.Join(Environment.NewLine, ScriptBlock.PartialViewScripts.Select(s => s.ToString())));
    }
        
    // ==============================================================================================================
    // ========================================== MOJE ==============================================================
    // ==============================================================================================================

    public static MvcHtmlString GenerateGameDataInput(this HtmlHelper helper, PlantationViewModel model)
    {
        var additionalAttr = new RouteValueDictionary
        {
            { "data-user_id", JsonConvert.SerializeObject(model.UserId) },
            { "data-don_data", JsonConvert.SerializeObject(model.DonData) },
            { "data-selected_district", JsonConvert.SerializeObject(model.SelectedDistrictId) },
            { "data-districts", JsonConvert.SerializeObject(model.S2DistrictsList) },
            { "data-is_new_player", JsonConvert.SerializeObject(model.IsNewPlayer) },
            { "data-must_buy_hollow", JsonConvert.SerializeObject(model.MustBuyHollow) },
            { "data-district_loading_message", JsonConvert.SerializeObject(model.DistrictLoadingMessage) },
            { "data-has_plantation", JsonConvert.SerializeObject(model.HasPlantation) },
            { "data-too_low_level", JsonConvert.SerializeObject(model.TooLowLevel) },
            { "type", "hidden" },
        };

        var ret = helper.TextBox("gameData", null, additionalAttr);

        return ret;
    }

    public static MvcHtmlString GenerateDropDownField(this HtmlHelper helper, PropertyInfo prop, EditOrCreateViewModel model,
        string value = null, string fieldAttr = null)
    {
        var ret = new MvcHtmlString("");

        if (prop == null) return ret;
        
        var fieldName = prop.Name;
        var additionalAttr = new RouteValueDictionary();
        additionalAttr.Add("class", "validate form-control");

        var required = prop.CustomAttributes?.SingleOrDefault(currAttr =>
            currAttr?.AttributeType?.FullName?.Contains("FieldIsRequiredAttribute") ?? false) != null;
            
        if (required)
            additionalAttr.Add("required", "required");
        
        var isMany = model?.IsManyEdit != null && model.IsManyEdit;
        if (isMany)
            additionalAttr.Add("disabled", "true");

        if (fieldAttr == "type=checkbox")
        {
            var list = SelectListLoaderHelper.GetCheckBoxAsSelectOptions(value);
            ret = helper.DropDownList(fieldName, list, additionalAttr);
        }
        else
        {
            ret = helper.DropDownList(fieldName, (List<SelectListItem>) model?.FieldsOptions[fieldName], "", additionalAttr);
        }

        return ret;
    }

    public static MvcHtmlString RenderStorageProducts(this HtmlHelper helper, object storageProducts)
    {
        var ret = "";
        var objectsList = ((IEnumerable) storageProducts)?.Cast<object>().ToList();

        if (objectsList != null && objectsList.Count > 0)
            ret = objectsList.Aggregate(ret,
                (current, obj) => current + helper.Partial("~/Views/Plantation/Storage/_storageProduct.cshtml", obj));
        else
            ret = "<h4 class='text-c-gray'>Brak</h4>";

        return MvcHtmlString.Create(ret);
    }

    public static MvcHtmlString RenderCurrencyExchanges(this HtmlHelper helper, List<CurrencyExchangeViewModel> currencyExchange)
    {
        var ret = "";

        if (currencyExchange != null && currencyExchange.Count > 0)
            ret = currencyExchange.Aggregate(ret,
                (current, obj) => current + helper.Partial("~/Views/Plantation/Storage/_currencyExchange.cshtml", obj));
        else
            ret = "<h4 class='text-c-gray'>Brak</h4>";

        return MvcHtmlString.Create(ret);
    }
        
    public static MvcHtmlString RenderQuestsInfo(this HtmlHelper helper, List<QuestInfoViewModel> quests)
    {
        var ret = "";

        if (quests != null && quests.Count > 0)
            ret = quests.GroupBy(item => item.QuestType).Aggregate(ret,
                (current, obj) => current + helper.Partial("~/Views/Plantation/Storage/_questsInfo.cshtml", obj));
        else
            ret = "<h4 class='text-c-gray'>Brak</h4>";

        return MvcHtmlString.Create(ret);
    }
        
    public static MvcHtmlString RenderPlantationStoragePlants(this HtmlHelper helper, List<PlantViewModel> plants)
    {
        var ret = "";
        if (plants != null && plants.Count > 0)
            ret = plants.Aggregate(ret,
                (current, plant) => current + helper.Partial("~/Views/Plantation/Others/_plant.cshtml", plant));

        return MvcHtmlString.Create(ret);
    }

    public static MvcHtmlString RenderFormInputs(this HtmlHelper helper, EditOrCreateViewModel model)
    {
        var ret = "";
            
        if (ViewHelper.CheckNameInputRenderCondition(model))
            ret += helper.Partial("~/Views/ConfigurationPanel/EditOrCreate/_FormInput.cshtml", model);

        for (var i = 0; i < model.Properties?.Count; i++)
        {
            var propName = model.Properties[i]?.Name;
            var prop = model.Properties[i];
            var propValue = model.EditingObjValues?.Count > 0 ? (string) model.EditingObjValues?[i] : "";
            
            model.CurrProp = prop;
            model.CurrValue = propValue;

            var condition = ViewHelper.CheckInputRenderConditions(propName, model);
            
            if (condition != null && (bool) condition)
                ret += helper.Partial("~/Views/ConfigurationPanel/EditOrCreate/_FormInput.cshtml", model);
            else if (condition != null)
                ret += $"<input id='{propName}' type='hidden' name='{propName}' value='{propValue}' class='validate form-control' required>";
        }

        return MvcHtmlString.Create(ret);
    }

    public static MvcHtmlString RenderFormSelectors(this HtmlHelper helper, EditOrCreateViewModel model)
    {
        var ret = "";
        
        if (model.ConnectionSelectors.Any() && !model.IsManyEdit)
            ret += helper.Partial("~/Views/ConfigurationPanel/EditOrCreate/_FormSelectors.cshtml", model);

        return MvcHtmlString.Create(ret);
    }

    public static MvcHtmlString RenderStructureTests(this HtmlHelper helper, StructureTestsViewModel model)
    {
        var ret = "";

        if (model.StructureTests != null && model.StructureTests.Count > 0)
            ret = model.StructureTests.Aggregate(ret,
                (current, test) => current + helper.Partial("~/Views/ConfigurationPanel/StructureTests/_structureTest.cshtml", test));
        else
            ret = "<h4 class='text-c-gray'>Brak testów</h4>";

        return MvcHtmlString.Create(ret);
    }
    
    public static MvcHtmlString RenderDistrictsInfo(this HtmlHelper helper, List<DistrictViewModel> districts)
    {
        var ret = "";

        if (districts != null && districts.Count > 0)
            ret = districts.Aggregate(ret,
                (current, obj) => current + helper.Partial("~/Views/Plantation/Storage/_districtInfo.cshtml", obj));
        else
            ret = "<h4 class='text-c-gray'>Brak</h4>";

        return MvcHtmlString.Create(ret);
    }
}
