using System.ComponentModel.DataAnnotations;

namespace CzuczenLand.Configuration.Dto;

/// <summary>
/// Klasa DTO reprezentująca dane wejściowe do zmiany motywu interfejsu użytkownika.
/// </summary>
public class ChangeUiThemeInput
{
    /// <summary>
    /// Nowy motyw interfejsu użytkownika.
    /// </summary>
    [Required]
    [MaxLength(32)]
    public string Theme { get; set; }
}