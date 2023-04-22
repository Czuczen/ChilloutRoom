using System.Linq;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using CzuczenLand.Authorization;
using CzuczenLand.Authorization.Roles;
using CzuczenLand.Authorization.Users;
using CzuczenLand.EntityFramework;
using Microsoft.AspNet.Identity;

namespace CzuczenLand.Migrations.SeedData;

public class HostRoleAndUserCreator
{
    private readonly CzuczenLandDbContext _context;

    public HostRoleAndUserCreator(CzuczenLandDbContext context)
    {
        _context = context;
    }

    public void Create()
    {
        CreateHostRoleAndUsers();
    }

    private void CreateHostRoleAndUsers()
    {
        //Admin role for host

        var adminRoleForHost = _context.Roles.FirstOrDefault(r => r.TenantId == null && r.Name == StaticRoleNames.Host.Admin);
        if (adminRoleForHost == null)
        {

            adminRoleForHost = new Role
            {
                Name = StaticRoleNames.Host.Admin,
                DisplayName = StaticRoleNames.Host.Admin,
                IsStatic = true
            };

            adminRoleForHost.SetNormalizedName();

            _context.Roles.Add(adminRoleForHost);
            _context.SaveChanges();

            //Grant all tenant permissions
            var permissions = PermissionFinder
                .GetAllPermissions(new CzuczenLandAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Host))
                .ToList();

            foreach (var permission in permissions)
            {
                _context.Permissions.Add(
                    new RolePermissionSetting
                    {
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = adminRoleForHost.Id
                    });
            }

            _context.SaveChanges();
        }

        //Admin user for tenancy host

        var adminUserForHost = _context.Users.FirstOrDefault(u => u.TenantId == null && u.UserName == AbpUserBase.AdminUserName);
        if (adminUserForHost == null)
        {
            adminUserForHost = _context.Users.Add(
                new User
                {
                    UserName = AbpUserBase.AdminUserName,
                    Name = "System",
                    Surname = "Administrator",
                    EmailAddress = "admin@aspnetboilerplate.com",
                    IsEmailConfirmed = true,
                    Password = new PasswordHasher().HashPassword(User.DefaultPassword)
                });

            adminUserForHost.SetNormalizedNames();

            _context.SaveChanges();

            _context.UserRoles.Add(new UserRole(null, adminUserForHost.Id, adminRoleForHost.Id));
            _context.SaveChanges();
        }
    }
}