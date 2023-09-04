using System.Collections.Generic;

namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Storage.Dto;

/// <summary>
/// Klasa reprezentująca informacje o wykonanej transakcji zakupu/sprzedaży w sklepie.
/// </summary>
public class ShopTransaction
{
    /// <summary>
    /// Informuje o powodzeniu transakcji.
    /// </summary>
    public bool SuccessfulTransaction { get; set; }

    /// <summary>
    /// Informacje o transakcji, które można wykorzystać do wyświetlenia na stronie.
    /// </summary>
    public readonly List<string> InfoMessage = new();
}