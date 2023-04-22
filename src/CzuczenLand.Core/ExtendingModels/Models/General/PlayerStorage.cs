using Abp.Auditing;
using CzuczenLand.ExtendingModels.Models.Shared;

namespace CzuczenLand.ExtendingModels.Models.General;

[Audited]
public class PlayerStorage : PartStorage
{
    public string PlayerName { get; set; }
        
    public int Honor { get; set; }
        
    public int? LastSelectedDistrictId { get; set; }
}