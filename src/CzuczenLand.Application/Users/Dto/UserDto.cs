using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using CzuczenLand.Authorization.Users;

namespace CzuczenLand.Users.Dto;

/// <summary>
/// Klasa DTO przechowująca informacje o użytkowniku.
/// </summary>
[AutoMapFrom(typeof(User))]
public class UserDto : EntityDto<long>
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
    /// Pełne imię i nazwisko użytkownika.
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// Data ostatniego logowania użytkownika.
    /// </summary>
    public DateTime? LastLoginTime { get; set; }

    /// <summary>
    /// Data utworzenia użytkownika.
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// Tablica ról przypisanych użytkownikowi.
    /// </summary>
    public string[] Roles { get; set; }
}