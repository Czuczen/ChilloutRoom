using System;
using System.Linq;
using System.Reflection;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.AutoMapper;
using Abp.Modules;
using Castle.MicroKernel.Registration;
using CzuczenLand.Authorization.Roles;
using CzuczenLand.Authorization.Users;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud;
using CzuczenLand.Roles.Dto;
using CzuczenLand.Users.Dto;

namespace CzuczenLand;

[DependsOn(typeof(CzuczenLandCoreModule), typeof(AbpAutoMapperModule))]
public class CzuczenLandApplicationModule : AbpModule
{
    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            
        // Rejestracja generycznych interfejsów
        AsyncCrudHelper.GenericInterfaces.ForEach(item => IocManager.IocContainer.Register(
            Classes.FromAssembly(Assembly.GetExecutingAssembly())
                .BasedOn(item)
                .WithService.Base()
                .LifestyleTransient()
                .Configure(configurer => configurer.Named(Guid.NewGuid().ToString()))
        ));

        // TODO: Is there somewhere else to store these, with the dto classes
        Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg =>
        {
            // Role and permission
            cfg.CreateMap<Permission, string>().ConvertUsing(r => r.Name);
            cfg.CreateMap<RolePermissionSetting, string>().ConvertUsing(r => r.Name);

            cfg.CreateMap<CreateRoleDto, Role>();
            cfg.CreateMap<RoleDto, Role>();
            cfg.CreateMap<Role, RoleDto>().ForMember(x => x.GrantedPermissions,
                opt => opt.MapFrom(x => x.Permissions.Where(p => p.IsGranted)));

            cfg.CreateMap<UserDto, User>();
            cfg.CreateMap<UserDto, User>().ForMember(x => x.Roles, opt => opt.Ignore());

            cfg.CreateMap<CreateUserDto, User>();
            cfg.CreateMap<CreateUserDto, User>().ForMember(x => x.Roles, opt => opt.Ignore());
        });
    }
}