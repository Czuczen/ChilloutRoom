using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using CzuczenLand.Validation;
using Abp.Extensions;

namespace CzuczenLand.Authorization.Accounts.Dto;

/// <summary>
/// Klasa DTO reprezentująca dane wejściowe do rejestracji użytkownika.
/// </summary>
public class RegisterInput : IValidatableObject
{
    /// <summary>
    /// Imię użytkownika.
    /// </summary>
    [Required]
    [StringLength(AbpUserBase.MaxNameLength)]
    public string Name { get; set; }

    /// <summary>
    /// Nazwisko użytkownika.
    /// </summary>
    [Required]
    [StringLength(AbpUserBase.MaxSurnameLength)]
    public string Surname { get; set; }

    /// <summary>
    /// Nazwa użytkownika.
    /// </summary>
    [Required]
    [StringLength(AbpUserBase.MaxUserNameLength)]
    public string UserName { get; set; }

    /// <summary>
    /// Adres email użytkownika.
    /// </summary>
    [Required]
    [EmailAddress]
    [StringLength(AbpUserBase.MaxEmailAddressLength)]
    public string EmailAddress { get; set; }

    /// <summary>
    /// Hasło użytkownika.
    /// </summary>
    [Required]
    [StringLength(AbpUserBase.MaxPlainPasswordLength)]
    [DisableAuditing]
    public string Password { get; set; }

    /// <summary>
    /// Odpowiedź na wyzwanie Captcha.
    /// </summary>
    [DisableAuditing]
    public string CaptchaResponse { get; set; }

    /// <summary>
    /// Walidacja danych wejściowych rejestracji użytkownika.
    /// </summary>
    /// <param name="validationContext">Kontekst walidacji.</param>
    /// <returns>Wynik walidacji.</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!UserName.IsNullOrEmpty())
        {
            if (!UserName.Equals(EmailAddress) && ValidationHelper.IsEmail(UserName))
            {
                yield return new ValidationResult("Username cannot be an email address unless it's same with your email address !");
            }
        }
    }
}