using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CzuczenLand.ExtendingFunctionalities.Consts.ViewParser;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.ViewParser;

public static class ViewParserHelper
{
    public static string GetRelationFieldEntityName(PropertyInfo prop)
    {
        var customAttrData = prop?.CustomAttributes?.SingleOrDefault(currAttr =>
            currAttr?.AttributeType?.FullName?.Contains("DisplayAttribute") ?? false);
            
        var namedArg = customAttrData?.NamedArguments
            ?.SingleOrDefault(currArg => currArg.MemberName != null && currArg.MemberName == "GroupName");
            
        var ret =  namedArg?.TypedValue.Value?.ToString();

        return ret;
    }

    public static List<PropertyInfo> GetRelationFields(List<PropertyInfo> properties)
    {
        return properties.Where(currProp =>
            (currProp.Name.Contains("Id") || currProp.Name == RelationFieldsNames.EntityName ||
             currProp.Name == RelationFieldsNames.QuestType || currProp.Name == RelationFieldsNames.Condition ||
             currProp.Name == RelationFieldsNames.Comparer || currProp.Name == RelationFieldsNames.CustomEntityName
            ) &&
            currProp.Name != "Id").ToList();
    }
}