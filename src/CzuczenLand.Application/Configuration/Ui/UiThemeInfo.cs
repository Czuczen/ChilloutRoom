namespace CzuczenLand.Configuration.Ui;

/// <summary>
/// Informacje o motywie interfejsu użytkownika.
/// </summary>
public class UiThemeInfo
{
    /// <summary>
    /// Nazwa motywu.
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Klasa CSS motywu.
    /// </summary>
    public string CssClass { get; }

    /// <summary>
    /// Inicjalizuje nową instancję klasy UiThemeInfo.
    /// </summary>
    /// <param name="name">Nazwa motywu.</param>
    /// <param name="cssClass">Klasa CSS motywu.</param>
    public UiThemeInfo(string name, string cssClass)
    {
        Name = name;
        CssClass = cssClass;
    }
}