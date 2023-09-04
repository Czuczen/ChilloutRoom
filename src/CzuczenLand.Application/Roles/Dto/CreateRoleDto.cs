using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Roles;
using CzuczenLand.Authorization.Roles;

namespace CzuczenLand.Roles.Dto;

/// <summary>
/// Klasa DTO do tworzenia roli.
/// </summary>
public class CreateRoleDto
{
    /// <summary>
    /// Nazwa roli.
    /// </summary>
    [Required]
    [StringLength(AbpRoleBase.MaxNameLength)]
    public string Name { get; set; }

    /// <summary>
    /// Wyświetlana nazwa roli.
    /// </summary>
    [Required]
    [StringLength(AbpRoleBase.MaxDisplayNameLength)]
    public string DisplayName { get; set; }

    /// <summary>
    /// Znormalizowana nazwa roli.
    /// </summary>
    public string NormalizedName { get; set; }

    /// <summary>
    /// Opis roli.
    /// </summary>
    [StringLength(Role.MaxDescriptionLength)]
    public string Description { get; set; }

    /// <summary>
    /// Wskazuje, czy rola jest statyczna.
    /// </summary>
    public bool IsStatic { get; set; }

    /// <summary>
    /// Lista przyznanych uprawnień roli.
    /// </summary>
    public List<string> GrantedPermissions { get; set; }
}