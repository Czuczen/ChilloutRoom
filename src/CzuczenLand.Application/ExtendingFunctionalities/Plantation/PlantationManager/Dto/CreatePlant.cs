using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;

public class CreatePlant
{
    public string RequirementsIsNotOkMessage { get; set; }
        
    public bool RequirementsIsOk { get; set; }
        
    public Plant Plant { get; set; }
        
    public Lamp Lamp { get; set; }
        
    public Manure Manure { get; set; }
        
    public Soil Soil { get; set; }
        
    public Water Water { get; set; }
        
    public Seed Seed { get; set; }
        
    public Pot Pot { get; set; }
}