using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.ObjectMapping;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.ConfigurationPanelManager.Dto;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.StructureTests.Dto;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.ConfigurationPanelManager;

public interface IConfigurationPanelManager : ITransientDependency
{
    Logs FormatLogs();
        
    Task<List<StructureTest>> StartTests(bool isAdmin, long userId);
        
    Task<EditOrCreate> EditOrCreateObject(string entity, IObjectMapper objectMapper, bool isAdmin, int? objectId = null,
        string editManyAction = null);

    Task<List<object>> SearchSuggestions(string valueToSearch, string entity, int? generatedTypeId, int? districtId);
}