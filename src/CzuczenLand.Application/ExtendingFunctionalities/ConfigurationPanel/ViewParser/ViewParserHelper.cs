using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CzuczenLand.ExtendingFunctionalities.Consts.ViewParser;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.ViewParser;

/// <summary>
/// Klasa pomocnicza dostarczająca metody wspomagające operacje przetwarzania wartości obiektów, które będą wyświetlone użytkownikowi.
/// </summary>
public static class ViewParserHelper
{
    /// <summary>
    /// Pobiera nazwę encji pola relacyjnego.
    /// </summary>
    /// <param name="prop">Informacje o właściwości pola relacyjnego.</param>
    /// <returns>Nazwa encji pola relacyjnego lub null, jeśli nie znaleziono.</returns>
    public static string GetRelationFieldEntityName(PropertyInfo prop)
    {
        var customAttrData = prop?.CustomAttributes?.SingleOrDefault(currAttr =>
            currAttr?.AttributeType?.FullName?.Contains("DisplayAttribute") ?? false);
            
        var namedArg = customAttrData?.NamedArguments
            ?.SingleOrDefault(currArg => currArg.MemberName != null && currArg.MemberName == "GroupName");
            
        var ret =  namedArg?.TypedValue.Value?.ToString();

        return ret;
    }

    /// <summary>
    /// Pobiera listę pól relacyjnych z listy właściwości encji.
    /// </summary>
    /// <param name="properties">Lista informacji o właściwościach obiektu.</param>
    /// <returns>Lista pól relacyjnych.</returns>
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