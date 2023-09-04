using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;

namespace CzuczenLand.Roles.Dto;

/// <summary>
/// Klasa DTO reprezentująca dane uprawnienia.
/// </summary>
[AutoMapFrom(typeof(Permission))]
public class PermissionDto : EntityDto<long>
{
    /// <summary>
    /// Nazwa uprawnienia.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Wyświetlana nazwa uprawnienia.
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// Opis uprawnienia.
    /// </summary>
    public string Description { get; set; }
}