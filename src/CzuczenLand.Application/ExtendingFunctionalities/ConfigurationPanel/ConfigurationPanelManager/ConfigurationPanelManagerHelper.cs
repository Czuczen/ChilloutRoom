using System.Collections.Generic;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.SelectListLoader;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.Utils;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.ConfigurationPanelManager;

/// <summary>
/// Klasa pomocnicza dla zarządzania panelem konfiguracyjnym.
/// </summary>
public static class ConfigurationPanelManagerHelper
{
    /// <summary>
    /// Pobiera połączenia encji, która ma możliwość ustawiania relacji Many-to-many w formularzu.
    /// </summary>
    /// <param name="entity">Nazwa encji.</param>
    /// <returns>Słownik zawierający połączenia encji.</returns>
    public static Dictionary<string, string> GetEntityConnections(string entity)
    {
        var ret = new Dictionary<string, string>();

        var entityEnum = SelectListLoaderHelper.GetEntityEnum(entity);
        switch (entityEnum)
        {
            case EnumUtils.Entities.Quest:
                ret[EntitiesDbNames.Drop] = EntitiesHrNames.Drop;
                ret[EntitiesDbNames.Requirement] = EntitiesHrNames.Requirement;
                break;
        }

        return ret;
    }
}