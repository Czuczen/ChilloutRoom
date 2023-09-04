using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.MultiTenancy.Dto;

namespace CzuczenLand.MultiTenancy;

/// <summary>
/// Interfejs serwisu aplikacyjnego dla operacji na dzierżawcach.
/// </summary>
public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedResultRequestDto, CreateTenantDto, TenantDto>
{
}