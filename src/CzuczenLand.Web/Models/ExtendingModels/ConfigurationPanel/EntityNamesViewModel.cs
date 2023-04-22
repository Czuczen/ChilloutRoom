using System.Collections.Generic;

namespace CzuczenLand.Web.Models.ExtendingModels.ConfigurationPanel;

public class EntityNamesViewModel
{
    public List<string> DbNames { get; set; }
        
    public Dictionary<string, string> DbToHrNames { get; set; }
}