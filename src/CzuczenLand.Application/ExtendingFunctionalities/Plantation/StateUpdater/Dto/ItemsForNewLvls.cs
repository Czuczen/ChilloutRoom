using System.Collections.Generic;

namespace CzuczenLand.ExtendingFunctionalities.StateUpdater.Dto;

public class ItemsForNewLvls
{
    public List<string> ItemsNames { get; set; }
        
    public List<LvlQuestsData> QuestsData { get; set; }
        
    public object S2DistrictsList { get; set; }
}