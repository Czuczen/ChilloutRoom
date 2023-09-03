using System.Collections.Generic;
using System.Reflection;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.ConfigurationPanelManager.Dto;

/// <summary>
/// Klasa reprezentująca dane potrzebne do wygenerowania formularza edycji lub tworzenia obiektu.
/// </summary>
public class EditOrCreate
{
    /// <summary>
    /// Słownik zawierający selektory dla formularza encji, która ma możliwość ustawiania relacji Many-to-many.
    /// </summary>
    public Dictionary<string, string> ConnectionSelectors { get; set; }
        
    /// <summary>
    /// Identyfikator dzielnicy dla istniejących połączeń.
    /// </summary>
    public int? ExistingConnectionsDistrictId { get; set; }
      
    /// <summary>
    /// Akcja edycji wielu obiektów (jeśli nie puste).
    /// </summary>
    public string EditManyAction { get; set; }
        
    /// <summary>
    /// Flaga informująca o edycji.
    /// </summary>
    public bool IsEdit { get; set; }
        
    /// <summary>
    /// Nazwa encji dla której będzie generowany formularz edycji lub tworzenia.
    /// </summary>
    public string EntityName { get; set; }
        
    /// <summary>
    /// Nazwa encji w formie czytelnej dla człowieka.
    /// </summary>
    public string EntityHrName { get; set; }
        
    /// <summary>
    /// Lista właściwości encji.
    /// </summary>
    public List<PropertyInfo> Properties { get; set; }
        
    /// <summary>
    /// Słownik zawierający opcje pól typy select.
    /// </summary>
    public Dictionary<string, object> FieldsOptions { get; set; }
        
    /// <summary>
    /// Identyfikator obiektu (jeśli edycja).
    /// </summary>
    public int? ObjectId { get; set; }
        
    /// <summary>
    /// Słownik zawierający istniejące połączenia dla formularza encji, która ma możliwość ustawiania relacji Many-to-many..
    /// </summary>
    public Dictionary<string, List<object>> ExistingConnections { get; } = new();
        
    /// <summary>
    /// Lista wartości edytowanego obiektu.
    /// </summary>
    public List<object> EditingObjValues { get; set; } = new();
}