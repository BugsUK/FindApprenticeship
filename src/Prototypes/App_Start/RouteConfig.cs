using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Prototypes
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                 name: "LandingPage",
                 url: "",
                 defaults: new { controller = "Home", action = "Index" }
                 );

            routes.MapRoute(
                name: "Privacy",
                url: "privacy",
                defaults: new { controller = "Home", action = "Privacy" }
                );

            routes.MapRoute(
                name: "TermsAndConditions",
                url: "terms",
                defaults: new { controller = "Home", action = "TermsAndConditions" }
                );

            routes.MapRoute(
                name: "ContactUs",
                url: "contact",
                defaults: new { controller = "Home", action = "ContactUs" }
                );

            routes.MapRoute(
                name: "SignIn",
                url: "signin",
                defaults: new { controller = "Account", action = "SignIn" }
                );

            /*
            routes.MapRoute(
                name: ManagementRouteNames.SignOut,
                url: "signout",
                defaults: new { controller = "Account", action = "SignOut" }
                );

            routes.MapRoute(
                name: ManagementRouteNames.SessionTimeout,
                url: "sessiontimeout",
                defaults: new { controller = "Account", action = "SessionTimeout" }
                );

            routes.MapRoute(
                name: ManagementRouteNames.SignOutCallback,
                url: "signedout",
                defaults: new { controller = "Account", action = "SignOutCallback" }
                );

            routes.MapRoute(
                name: ManagementRouteNames.Authorize,
                url: "authorize",
                defaults: new { controller = "AgencyUser", action = "Authorize" }
                );

            routes.MapRoute(
                name: ManagementRouteNames.AuthorizationError,
                url: "authorizationerror",
                defaults: new { controller = "AgencyUser", action = "AuthorizationError" }
                );
                */

            routes.MapRoute(
                name: "Dashboard",
                url: "dashboard",
                defaults: new { controller = "AgencyUser", action = "Dashboard" }
                );
        }
    }
}
