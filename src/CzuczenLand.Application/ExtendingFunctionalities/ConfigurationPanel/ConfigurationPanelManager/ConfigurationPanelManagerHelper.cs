using System.Collections.Generic;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.SelectListLoader;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.Utils;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.ConfigurationPanelManager;

public static class ConfigurationPanelManagerHelper
{
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