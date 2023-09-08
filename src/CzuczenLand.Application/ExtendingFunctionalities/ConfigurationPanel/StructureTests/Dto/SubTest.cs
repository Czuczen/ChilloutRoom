using CzuczenLand.ExtendingFunctionalities.Utils;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.StructureTests.Dto;

/// <summary>
/// Klasa reprezentująca podtest struktury.
/// </summary>
public class SubTest
{
    /// <summary>
    /// Status podtestu struktury.
    /// </summary>
    public EnumUtils.StructureTestsStatuses Status { get; set; }
       
    /// <summary>
    /// Opis testu.
    /// </summary>
    public string Description { get; set; }
}