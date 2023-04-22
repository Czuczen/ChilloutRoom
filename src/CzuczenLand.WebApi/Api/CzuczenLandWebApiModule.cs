using System.Linq;
using System.Reflection;
using System.Web.Http;
using Abp.Application.Services;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.WebApi;
using Swashbuckle.Application;

namespace CzuczenLand.Api;

[DependsOn(typeof(AbpWebApiModule), typeof(CzuczenLandApplicationModule))]
public class CzuczenLandWebApiModule : AbpModule
{
    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

        Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
            .ForAll<IApplicationService>(typeof(CzuczenLandApplicationModule).Assembly, "app")
            .Build();

        // https://aspnetboilerplate.com/Pages/Documents/Dynamic-Web-API
        // We can use the ForMethods method to better adjust each method while using the ForAll method. Example:
        // Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
        //     .ForAll<IApplicationService>(typeof(CzuczenLandApplicationModule).Assembly, "app")
        //     .ForMethods(builder =>
        //     {
        //         if (builder.Method.IsDefined(typeof(IgnoreInDynamicApiControllerBuilderAttribute)))
        //         {
        //             builder.DontCreate = true;
        //         }
        //     })
        //     .Build();

        Configuration.Modules.AbpWebApi().HttpConfiguration.Filters.Add(new HostAuthenticationFilter("Bearer"));
        
        ConfigureSwaggerUi();
    }
    
    /// <summary>
    /// https://aspnetboilerplate.com/Pages/Documents/Swagger-UI-Integration
    /// </summary>
    private void ConfigureSwaggerUi()
    {
        Configuration.Modules.AbpWebApi().HttpConfiguration
            .EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "SwaggerIntegrationDemo.WebApi");
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            })
            .EnableSwaggerUi();
        // .EnableSwaggerUi(c =>
        // {
        //     c.InjectJavaScript(Assembly.GetAssembly(typeof(CzuczenLandWebApiModule)), "CzuczenLand.Api.Scripts.Swagger-Custom.js");
        // });
    }
}