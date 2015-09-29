namespace SFA.Apprenticeships.Web.Manage
{
    using System;
    using System.Configuration;
    using System.Threading.Tasks;
    using Microsoft.Owin;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.WsFederation;
    using Owin;

    public static class AuthenticationConfig
    {
        public const string CookieName = "User.Authentication";

        private static readonly string Realm = ConfigurationManager.AppSettings["ida:Wtrealm"];
        private static readonly string MetadataAddress = ConfigurationManager.AppSettings["ida:MetadataAddress"];
        private static readonly int SessionTimeout = int.Parse(ConfigurationManager.AppSettings["ida:SessionTimeout"]);

        public static void RegisterProvider(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(WsFederationAuthenticationDefaults.AuthenticationType);

            var cookieAuthenticationOptions = new CookieAuthenticationOptions
            {
                CookieName = CookieName,
                AuthenticationType = WsFederationAuthenticationDefaults.AuthenticationType,
                SlidingExpiration = true,
                ExpireTimeSpan = TimeSpan.FromMinutes(SessionTimeout)
            };

            app.UseCookieAuthentication(cookieAuthenticationOptions);

            app.UseWsFederationAuthentication(
                new WsFederationAuthenticationOptions
                {
                    //http://stackoverflow.com/questions/28627061/owin-ws-federation-setting-up-token-sliding-expiration
                    UseTokenLifetime = false,
                    Wtrealm = Realm,
                    MetadataAddress = MetadataAddress
                });
        }
    }
}