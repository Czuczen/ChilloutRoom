using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CzuczenLand.Authorization.Users;

namespace CzuczenLand.Sessions.Dto;

/// <summary>
/// Klasa DTO przechowująca informacje o użytkowniku dla sesji logowania.
/// </summary>
[AutoMapFrom(typeof(User))]
public class UserLoginInfoDto : EntityDto<long>
{
    /// <summary>
    /// Nazwa użytkownika.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Nazwisko użytkownika.
    /// </summary>
    public string Surname { get; set; }

    /// <summary>
    /// Nazwa użytkownika (login).
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Adres email użytkownika.
    /// </summary>
    public string EmailAddress { get; set; }
}