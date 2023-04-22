using System.Collections.Generic;

namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Bonus.Dto;

public class BonusActivation
{
    public bool SuccessfulActivation { get; set; }

    public readonly List<string> InfoMessage = new();
}