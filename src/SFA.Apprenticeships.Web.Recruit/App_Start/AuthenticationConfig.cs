namespace SFA.Apprenticeships.Web.Recruit
{
    using System.Configuration;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.WsFederation;
    using Owin;

    public static class AuthenticationConfig
    {
        private static readonly string Realm = ConfigurationManager.AppSettings["ida:Wtrealm"];
        private static readonly string MetadataAddress = ConfigurationManager.AppSettings["ida:MetadataAddress"];

        public static void RegisterProvider(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseWsFederationAuthentication(
                new WsFederationAuthenticationOptions
                {
                    Wtrealm = Realm,
                    MetadataAddress = MetadataAddress
                });
        }
    }
}