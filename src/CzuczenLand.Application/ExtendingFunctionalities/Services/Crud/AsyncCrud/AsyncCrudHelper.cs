using System;
using System.Collections.Generic;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.DeletePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;

namespace CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud;

public static class AsyncCrudHelper
{
    public static readonly List<Type> GenericInterfaces = new()
    {
        typeof(ICreateDefinition<>),
        typeof(IUpdateDefinition<>),
        typeof(IDeleteDefinition<>),
        typeof(IResponseBuilder<>),
    };
}