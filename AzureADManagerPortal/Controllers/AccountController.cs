
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security;

namespace AzureADManagerPortal.Controllers
{
    public class AccountController : Controller
    {
        public void SignIn()
        {
            if (!this.Request.IsAuthenticated)
            {
                this.HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties
                    {
                        RedirectUri = "/"
                    }, OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        public void SignOut()
        {
            this.HttpContext.GetOwinContext().Authentication.SignOut(
                OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
        }
    }
}