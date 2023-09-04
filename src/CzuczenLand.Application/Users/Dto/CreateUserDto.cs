using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using CzuczenLand.Authorization.Users;

namespace CzuczenLand.Users.Dto;

/// <summary>
/// Klasa DTO do tworzenia użytkownika.
/// </summary>
[AutoMapTo(typeof(User))]
public class CreateUserDto
{
    /// <summary>
    /// Nazwa użytkownika.
    /// </summary>
    [Required]
    [StringLength(AbpUserBase.MaxUserNameLength)]
    public string UserName { get; set; }

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
    /// Adres email użytkownika.
    /// </summary>
    [Required]
    [EmailAddress]
    [StringLength(AbpUserBase.MaxEmailAddressLength)]
    public string EmailAddress { get; set; }

    /// <summary>
    /// Wskazuje, czy użytkownik jest aktywny.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Tablica ról przypisanych użytkownikowi.
    /// </summary>
    public string[] RoleNames { get; set; }

    /// <summary>
    /// Hasło użytkownika.
    /// </summary>
    [Required]
    [StringLength(AbpUserBase.MaxPlainPasswordLength)]
    [DisableAuditing]
    public string Password { get; set; }
}