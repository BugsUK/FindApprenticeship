namespace SFA.Apprenticeships.Web.Recruit
{
    using System;
    using System.Configuration;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.WsFederation;
    using Owin;

    public static class AuthenticationConfig
    {
        public const string CookieName = "User.Authentication";

        private static readonly string Realm = ConfigurationManager.AppSettings["ida:Wtrealm"];
        private static readonly string MetadataAddress = ConfigurationManager.AppSettings["ida:MetadataAddress"];

        public static void RegisterProvider(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            var cookieAuthenticationOptions = new CookieAuthenticationOptions
            {
                CookieName = CookieName,
                SlidingExpiration = true,
                //TODO: From config
                ExpireTimeSpan = TimeSpan.FromMinutes(60)
            };

            app.UseCookieAuthentication(cookieAuthenticationOptions);

            app.UseWsFederationAuthentication(
                new WsFederationAuthenticationOptions
                {
                    Wtrealm = Realm,
                    MetadataAddress = MetadataAddress
                });
        }
    }
}