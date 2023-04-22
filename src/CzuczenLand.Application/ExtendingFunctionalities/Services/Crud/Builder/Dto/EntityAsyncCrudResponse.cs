using System.Collections.Generic;

namespace CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder.Dto;

public class EntityAsyncCrudResponse
{
    public List<string> DbProperties { get; set; }
        
    public List<string> HrProperties { get; set; }
        
    public bool CanCreate { get; set; }
        
    public List<object> Records { get; set; }
        
    public string InfoMsg { get; set; }
}