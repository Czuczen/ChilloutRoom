using System;
using System.Configuration;
using System.IO;
using Abp.Authorization.Users;
using Abp.Extensions;
using Microsoft.AspNet.Identity;

namespace CzuczenLand.Authorization.Users;

public class User : AbpUser<User>
{
    public static string DefaultPassword
    {
        get
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var index = basePath.LastIndexOf("\\src\\", StringComparison.OrdinalIgnoreCase);
            
            if (index < 0) return null;
            
            var pathToSrcFolder = basePath.Substring(0, index + 5);
            var finalPath = Path.Combine(pathToSrcFolder, "CzuczenLand.Web/Web.config");
            var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = finalPath };
            var config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            var adminPassword = config.AppSettings.Settings["AdminPassword"].Value;

            return adminPassword;
        }
    }

    public static string CreateRandomPassword()
    {
        return Guid.NewGuid().ToString("N").Truncate(16);
    }

    public static User CreateTenantAdminUser(int tenantId, string emailAddress, string password)
    {
        var user = new User
        {
            TenantId = tenantId,
            UserName = AdminUserName,
            Name = AdminUserName,
            Surname = AdminUserName,
            EmailAddress = emailAddress,
            Password = new PasswordHasher().HashPassword(password)
        };

        user.SetNormalizedNames();

        return user;
    }
}