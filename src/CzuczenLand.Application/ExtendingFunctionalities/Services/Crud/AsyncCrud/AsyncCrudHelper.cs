using System;
using System.Collections.Generic;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.CreatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.DeletePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;

namespace CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud;

/// <summary>
/// Klasa pomocnicza dla asynchronicznych serwisów CRUD.
/// </summary>
public static class AsyncCrudHelper
{
    /// <summary>
    /// Lista generycznych interfejsów.
    /// Potrzebna do rejestracji w iniekcji zależności.
    /// </summary>
    public static readonly List<Type> GenericInterfaces = new()
    {
        typeof(ICreateDefinition<>),
        typeof(IUpdateDefinition<>),
        typeof(IDeleteDefinition<>),
        typeof(IResponseBuilder<>),
    };
}