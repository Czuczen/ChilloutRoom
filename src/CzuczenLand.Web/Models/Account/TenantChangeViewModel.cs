using Abp.AutoMapper;
using CzuczenLand.Sessions.Dto;

namespace CzuczenLand.Web.Models.Account;

[AutoMapFrom(typeof(GetCurrentLoginInformationsOutput))]
public class TenantChangeViewModel
{
    public TenantLoginInfoDto Tenant { get; set; }
}