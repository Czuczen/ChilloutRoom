namespace CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;

/// <summary>
/// Klasa pomocnicza dla budowniczego odpowiedzi.
/// </summary>
public static class ResponseBuilderHelper
{
    /// <summary>
    /// Pobiera wiadomość informacyjną dotyczącą zmiany.
    /// --------------------
    /// </summary>
    /// <param name="isDistrictEntity">Czy encja jest dzielnicą.</param>
    /// <returns>Wiadomość informacyjna dotycząca zmiany.</returns>
    public static string GetChangeInfoMessage(bool isDistrictEntity)
    {
        string infoMessage; 
        if (isDistrictEntity)
            infoMessage = "Administrator dokonał aktualizacji dzielnicy. Nastąpi odświeżenie strony.";
        else
            infoMessage = "Opiekun dzielnicy dokonał aktualizacji . Nastąpi odświeżenie strony.";

        return infoMessage;
    }
}