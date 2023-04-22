using System.Collections.Generic;
using CzuczenLand.ExtendingFunctionalities.DistrictCloner.Dto;

namespace CzuczenLand.Web.Models.ExtendingModels.ConfigurationPanel;

public class DistrictCloneViewModel
{
    public List<DistrictContext> DistrictContexts { get; set; }
        
    public bool IsValid => string.IsNullOrWhiteSpace(ValidationMessage);
        
    public string ValidationMessage { get; set; }
}