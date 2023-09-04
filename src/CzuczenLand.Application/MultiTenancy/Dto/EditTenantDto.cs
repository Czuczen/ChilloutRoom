using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using Abp.MultiTenancy;

namespace CzuczenLand.MultiTenancy.Dto;

/// <summary>
/// Klasa DTO do edycji istniejącego dzierżawcy.
/// </summary>
[AutoMapTo(typeof(Tenant))]
public class EditTenantDto
{
    /// <summary>
    /// Nazwa unikalna dzierżawcy.
    /// </summary>
    [Required]
    [StringLength(AbpTenantBase.MaxTenancyNameLength)]
    [RegularExpression(AbpTenantBase.TenancyNameRegex)]
    public string TenancyName { get; set; }

    /// <summary>
    /// Nazwa wyświetlana dzierżawcy.
    /// </summary>
    [Required]
    [StringLength(AbpTenantBase.MaxNameLength)]
    public string Name { get; set; }
    
    /// <summary>
    /// Status aktywności dzierżawcy.
    /// </summary>
    public bool IsActive { get; set; }
}