using System.Collections.Generic;

namespace CzuczenLand.ExtendingFunctionalities.Services.Crud.Dto;

/// <summary>
/// Klasa reprezentująca dane wejściowe obejmujące informacje o obiekcie oraz jego połączeniach..
/// </summary>
/// <typeparam name="TInput">Typ danych wejściowych.</typeparam>
public class InputWithConnections<TInput> // nie może być abstract. Walidacja modelu wybucha
{
    /// <summary>
    /// Dane wejściowe/obiekt.
    /// </summary>
    public TInput Input { get; set; }
        
    /// <summary>
    /// Słownik zawierający połączenia.
    /// Klucz reprezentuje nazwę encji, a wartość to lista identyfikatorów.
    /// </summary>
    public Dictionary<string, List<int>> Connections { get; set; }
}