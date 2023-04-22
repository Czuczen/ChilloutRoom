namespace CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;

public static class ResponseBuilderHelper
{
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