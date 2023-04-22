using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.ConfigurationPanelManager.Dto;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.SharedEntitiesFieldsNames.Db;
using CzuczenLand.ExtendingFunctionalities.Consts.ViewParser;
using CzuczenLand.ExtendingFunctionalities.PlantationManager;
using CzuczenLand.Web.Helpers;

namespace CzuczenLand.Web.Models.ExtendingModels.ConfigurationPanel;

[AutoMapFrom(typeof(EditOrCreate))]
public class EditOrCreateViewModel
{
    public int? ObjectId { get; set; }
        
    public bool IsNotDropDownField => !CurrProp.Name.Contains(EntitiesFieldsDbNames.Id) &&
                                      CurrProp.Name != RelationFieldsNames.EntityName &&
                                      CurrProp.Name != RelationFieldsNames.QuestType &&
                                      CurrProp.Name != RelationFieldsNames.Condition &&
                                      CurrProp.Name != RelationFieldsNames.Comparer &&
                                      CurrProp.Name != RelationFieldsNames.CustomEntityName &&
                                      FieldTypeAttr != "type=checkbox";
        
    public string FieldTypeAttr => ViewHelper.GetTypeAttr(CurrProp);
        
    public string CurrPropHrName => PlantationManagerHelper.GetHrPropName(CurrProp);
        
    public string FieldIsRequired => CurrProp.CustomAttributes?.SingleOrDefault(currAttr =>
        currAttr?.AttributeType?.FullName?.Contains("FieldIsRequiredAttribute") ?? false) != null ? "required" : "";

    public string EditManyHeaderInfo => IsManyEdit ? "Wybierz pola do edycji" : "";

    public string Header => IsEdit || IsManyEdit ? "Edytuj" : "Utwórz";

    public string CrudAction => IsEdit ? "Update" : IsManyEdit ? "UpdateMany" : "Create";

    public bool IsManyEdit => EditManyAction != null;

    public string SetDisabledColorOnEditMany => IsManyEdit ? "bg-c-gainsboro" : "";

    public string SetFieldAsDisabled => IsManyEdit ? "disabled" : "";
        
        
        
    public bool IsEdit { get; set; }

    public List<PropertyInfo> Properties { get; set; }

    public List<object> EditingObjValues { get; set; }
        
    public string EditManyAction { get; set; }
        
    public string EntityHrName { get; set; }

    public Dictionary<string, object> FieldsOptions { get; set; }

    public PropertyInfo CurrProp { get; set; }
        
    public string CurrValue { get; set; }
        
    public Dictionary<string, string> ConnectionSelectors { get; set; }
        
    public int? ExistingConnectionsDistrictId { get; set; }
        
    public Dictionary<string, List<object>> ExistingConnections { get; } = new();
}