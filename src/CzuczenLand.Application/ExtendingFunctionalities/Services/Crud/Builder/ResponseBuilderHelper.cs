namespace CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;

/// <summary>
/// Klasa pomocnicza dla budowniczego odpowiedzi.
/// </summary>
public static class ResponseBuilderHelper
{
    /// <summary>
    /// Pobiera wiadomość informującą o zmianie w konfiguracji dzielnicy.
    /// </summary>
    /// <param name="wardenDistrictId">Id dzielnicy opiekuna</param>
    /// <returns>Wiadomość informacyjna dotycząca zmiany.</returns>
    public static string GetChangeInfoMessage(int? wardenDistrictId)
    {
        string infoMessage; 
        if (wardenDistrictId == null)
            infoMessage = "Administrator dokonał aktualizacji dzielnicy. Nastąpi odświeżenie strony.";
        else
            infoMessage = "Opiekun dzielnicy dokonał aktualizacji . Nastąpi odświeżenie strony.";

        return infoMessage;
    }
}