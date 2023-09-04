using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.MultiTenancy;

namespace CzuczenLand.MultiTenancy.Dto;

/// <summary>
/// Klasa DTO reprezentująca dane dzierżawcy.
/// </summary>
[AutoMapTo(typeof(Tenant)), AutoMapFrom(typeof(Tenant))]
public class TenantDto : EntityDto
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