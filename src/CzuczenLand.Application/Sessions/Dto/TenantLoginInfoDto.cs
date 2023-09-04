using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CzuczenLand.MultiTenancy;

namespace CzuczenLand.Sessions.Dto;

/// <summary>
/// Klasa DTO przechowująca informacje o dzierżawcy dla sesji logowania.
/// </summary>
[AutoMapFrom(typeof(Tenant))]
public class TenantLoginInfoDto : EntityDto
{
    /// <summary>
    /// Nazwa unikalna dzierżawcy.
    /// </summary>
    public string TenancyName { get; set; }
    
    /// <summary>
    /// Nazwa wyświetlana dzierżawcy.
    /// </summary>
    public string Name { get; set; }
}