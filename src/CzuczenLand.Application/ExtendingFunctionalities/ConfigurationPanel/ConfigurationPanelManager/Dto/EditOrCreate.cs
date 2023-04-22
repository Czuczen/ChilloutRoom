using System.Collections.Generic;
using System.Reflection;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.ConfigurationPanelManager.Dto;

public class EditOrCreate
{
    public Dictionary<string, string> ConnectionSelectors { get; set; }
        
    public int? ExistingConnectionsDistrictId { get; set; }
        
    public string EditManyAction { get; set; }
        
    public bool IsEdit { get; set; }
        
    public string EntityName { get; set; }
        
    public string EntityHrName { get; set; }
        
    public List<PropertyInfo> Properties { get; set; }
        
    public Dictionary<string, object> FieldsOptions { get; set; }
        
    public int? ObjectId { get; set; }
        
    public Dictionary<string, List<object>> ExistingConnections { get; } = new();
        
    public List<object> EditingObjValues { get; set; } = new();
}