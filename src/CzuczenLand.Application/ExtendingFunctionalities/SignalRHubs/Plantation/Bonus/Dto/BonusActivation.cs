using System.Collections.Generic;

namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Bonus.Dto;

/// <summary>
/// Klasa zawierająca informacje o aktywacji bonusu lub odblokowania.
/// </summary>
public class BonusActivation
{
    /// <summary>
    /// Oznacza czy aktywacja bonusu lub odblokowania przebiegła pomyślnie.
    /// </summary>
    public bool SuccessfulActivation { get; set; }

    /// <summary>
    /// Zawiera informacje zwrotne dotyczące aktywacji bonusu lub odblokowania.
    /// </summary>
    public readonly List<string> InfoMessage = new();
}