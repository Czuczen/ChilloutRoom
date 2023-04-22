using System;
using System.Configuration;
using Abp.Owin;
using Abp.Timing;
using CzuczenLand.Api.Controllers;
using CzuczenLand.ExtendingFunctionalities.SignalRHubs;
using CzuczenLand.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace CzuczenLand.Web;

public class Startup
{
    public void Configuration(IAppBuilder app)
    {
        app.UseAbp();
           
        app.UseOAuthBearerAuthentication(AccountController.OAuthBearerOptions);
            
        app.UseCookieAuthentication(new CookieAuthenticationOptions
        {
            AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            LoginPath = new PathString("/Account/Login"),
            // by setting following values, the auth cookie will expire after the configured amount of time (default 14 days) when user set the (IsPermanent == true) on the login
            ExpireTimeSpan = new TimeSpan(int.Parse(ConfigurationManager.AppSettings["AuthSession.ExpireTimeInDays.WhenPersistent"] ?? "14"), 0, 0, 0),
            SlidingExpiration = bool.Parse(ConfigurationManager.AppSettings["AuthSession.SlidingExpirationEnabled"] ?? bool.FalseString)
 
        });

        app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            
        // normalizacja czasu
        // https://stackoverflow.com/questions/47705195/asp-net-boilerplate-how-to-add-timezone-to-user-profile
        // https://aspnetboilerplate.com/Pages/Documents/Timing
        Clock.Provider = ClockProviders.Utc;

        // provider id użytkownika dla SignalR
        var idProvider = new CustomUserIdProvider();
        GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => idProvider);
            
        // Wyjątek - Wymagany licznik wydajności nie jest licznikiem niestandardowym; musi być on inicjowany jako tylko do odczytu.
        // Można zignorować
        // https://stackoverflow.com/questions/17533706/the-requested-performance-counter-is-not-a-custom-counter-it-has-to-be-initiali
        // You can safely ignore these exceptions, but you obviously won't get performance counter data.
        app.MapSignalR(new HubConfiguration{EnableDetailedErrors = true});

        //ENABLE TO USE HANGFIRE dashboard (Requires enabling Hangfire in CzuczenLandWebModule)
        // app.UseHangfireDashboard("/hangfire", new DashboardOptions
        // {
        //     Authorization = new[] { new AbpHangfireAuthorizationFilter() } //You can remove this line to disable authorization
        // });
    }
}