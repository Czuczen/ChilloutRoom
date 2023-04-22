using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.MultiTenancy.Dto;

namespace CzuczenLand.MultiTenancy;

public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedResultRequestDto, CreateTenantDto, TenantDto>
{
}