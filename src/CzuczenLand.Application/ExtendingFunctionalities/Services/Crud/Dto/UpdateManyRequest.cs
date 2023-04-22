using System.Collections.Generic;

namespace CzuczenLand.ExtendingFunctionalities.Services.Crud.Dto;

public class UpdateManyRequest
{
    public object FieldsToUpdate { get; set; }
        
    public List<int> Ids { get; set; }
}