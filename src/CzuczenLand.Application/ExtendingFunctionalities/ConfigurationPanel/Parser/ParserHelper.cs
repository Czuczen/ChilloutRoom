﻿using System;
using System.Reflection;
using CzuczenLand.ExtendingFunctionalities.Utils;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser;

/// <summary>
/// Klasa pomocnicza zawierająca metody do wspomagania przetwarzania właściwości.
/// </summary>
public static class ParserHelper
{
    /// <summary>
    /// Pobiera typ właściwości i przyporządkowuje go do odpowiedniego typu wyliczeniowego (enum).
    /// </summary>
    /// <param name="prop">Właściwość, dla której ma zostać pobrany typ.</param>
    /// <returns>Typ wyliczeniowy (enum) opisujący typ właściwości lub null, jeśli nie można określić typu.</returns>
    public static EnumUtils.PropTypes? GetPropType(PropertyInfo prop)
    {
        EnumUtils.PropTypes? ret = null;

        if (prop == null) return null;

        if (prop.PropertyType == typeof(string))
            ret = EnumUtils.PropTypes.String;

        if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
            ret = EnumUtils.PropTypes.DateTime;

        if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
            ret = EnumUtils.PropTypes.Boolean;

        if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(double?))
            ret = EnumUtils.PropTypes.Double;
            
        if (prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?))
            ret = EnumUtils.PropTypes.Decimal;

        if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
            ret = EnumUtils.PropTypes.Int;

        if (prop.PropertyType == typeof(long) || prop.PropertyType == typeof(long?))
            ret = EnumUtils.PropTypes.Long;
            
        if (prop.PropertyType == typeof(Guid) || prop.PropertyType == typeof(Guid?))
            ret = EnumUtils.PropTypes.Guid;

        return ret;
    }
}