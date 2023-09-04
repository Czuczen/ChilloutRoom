using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using CzuczenLand.Authorization.Users;

namespace CzuczenLand.Users.Dto;

/// <summary>
/// Klasa DTO do aktualizacji danych użytkownika.
/// </summary>
[AutoMapTo(typeof(User))]
public class UpdateUserDto: EntityDto<long>
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
}