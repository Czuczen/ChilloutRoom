using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.MultiTenancy;

namespace CzuczenLand.MultiTenancy.Dto;

/// <summary>
/// Klasa DTO do tworzenia nowego dzierżawcy.
/// </summary>
[AutoMapTo(typeof(Tenant))]
public class CreateTenantDto
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
    /// Adres e-mail administratora dzierżawcy.
    /// </summary>
    [Required]
    [StringLength(AbpUserBase.MaxEmailAddressLength)]
    public string AdminEmailAddress { get; set; }
    
    /// <summary>
    /// Informacje połączenia z bazą danych.
    /// </summary>
    [MaxLength(AbpTenantBase.MaxConnectionStringLength)]
    public string ConnectionString { get; set; }
    
    /// <summary>
    /// Status aktywności dzierżawcy.
    /// </summary>
    public bool IsActive { get; set; }
}