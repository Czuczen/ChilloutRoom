using System.ComponentModel.DataAnnotations;
using Abp.MultiTenancy;

namespace CzuczenLand.Authorization.Accounts.Dto;

/// <summary>
/// Klasa DTO reprezentująca dane wejściowe do sprawdzania dostępności dzierżawy.
/// </summary>
public class IsTenantAvailableInput
{
    /// <summary>
    /// Nazwa dzierżawy.
    /// </summary>
    [Required]
    [MaxLength(AbpTenantBase.MaxTenancyNameLength)]
    public string TenancyName { get; set; }
}