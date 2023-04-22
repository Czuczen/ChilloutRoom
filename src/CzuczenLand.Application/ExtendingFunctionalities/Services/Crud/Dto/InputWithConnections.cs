using System.Collections.Generic;

namespace CzuczenLand.ExtendingFunctionalities.Services.Crud.Dto;

public class InputWithConnections<TInput> // nie może być abstract. Walidacja modelu wybucha
{
    public TInput Input { get; set; }
        
    public Dictionary<string, List<int>> Connections { get; set; }
}