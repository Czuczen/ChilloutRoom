using System.Collections.Generic;

namespace CzuczenLand.ExtendingFunctionalities.StateUpdater.Dto;

/// <summary>
/// Klasa reprezentująca przedmioty dla nowych poziomów.
/// Wysyłane do klienta.
/// </summary>
public class ItemsForNewLvls
{
    /// <summary>
    /// Lista nazw przedmiotów.
    /// </summary>
    public List<string> ItemsNames { get; set; }
        
    /// <summary>
    /// Lista danych zadań.
    /// </summary>
    public List<LvlQuestsData> QuestsData { get; set; }
        
    /// <summary>
    /// Lista danych dzielnic dla biblioteki Select2.
    /// </summary>
    public object S2DistrictsList { get; set; }
}