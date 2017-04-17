using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;

[assembly: OwinStartup("AzureADManagerPortalConfig", typeof(AzureADManagerPortal.Startup))]
namespace AzureADManagerPortal
{
    using System.Configuration;
    using System.Globalization;
    using System.Threading.Tasks;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.OpenIdConnect;
    using Owin;

    public class Startup
    {
        private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static string aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        private static string tenant = ConfigurationManager.AppSettings["ida:Tenant"];
        private static string postLogoutRedirectUrl = ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"];

        string authority = string.Format(CultureInfo.InvariantCulture, aadInstance, tenant);

        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions()
                {
                    ClientId = clientId,
                    Authority = this.authority,
                    PostLogoutRedirectUri = postLogoutRedirectUrl,
                    Notifications = new OpenIdConnectAuthenticationNotifications()
                    {
                        AuthenticationFailed = context =>
                        {
                            context.HandleResponse();
                            context.Response.Redirect("/Error/message=" + context.Exception.Message);
                            return Task.FromResult(0);
                        }
                    }
                });

        }
    }
}