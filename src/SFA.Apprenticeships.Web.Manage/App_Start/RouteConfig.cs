namespace SFA.Apprenticeships.Web.Manage
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Constants;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.ico/{*pathInfo}");
            routes.IgnoreRoute("{folder}/{*pathInfo}", new { folder = "Content" });
            routes.IgnoreRoute("{folder}/{*pathInfo}", new { folder = "Scripts" });

            routes.MapRoute(
                name: ManagementRouteNames.LandingPage,
                url: "",
                defaults: new { controller = "Home", action = "Index" }
                );

            routes.MapRoute(
                name: ManagementRouteNames.Privacy,
                url: "privacy",
                defaults: new { controller = "Home", action = "Privacy" }
                );

            routes.MapRoute(
                name: ManagementRouteNames.TermsAndConditions,
                url: "terms",
                defaults: new { controller = "Home", action = "TermsAndConditions" }
                );

            routes.MapRoute(
                name: ManagementRouteNames.ContactUs,
                url: "contact",
                defaults: new { controller = "Home", action = "ContactUs" }
                );

            routes.MapRoute(
                name: ManagementRouteNames.SignIn,
                url: "signin",
                defaults: new { controller = "Account", action = "SignIn" }
                );

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

            routes.MapRoute(
                name: ManagementRouteNames.Dashboard,
                url: "dashboard",
                defaults: new {controller = "AgencyUser", action = "Dashboard"}
                );

            routes.MapRoute(
                name: ManagementRouteNames.ReviewVacancy,
                url: "vacancy/review",
                defaults: new { controller = "Vacancy", action = "Review" }
                );

            routes.MapRoute(
                name: ManagementRouteNames.ApproveVacancy,
                url: "vacancy/approve",
                defaults: new { controller = "Vacancy", action = "Approve" }
                );

            routes.MapRoute(
                name: ManagementRouteNames.EditSummary,
                url: "vacancy/editsummary",
                defaults: new { controller = "Vacancy", action = "EditSummary" }
                );

            routes.MapRoute(
                name: ManagementRouteNames.SaveSummary,
                url: "vacancy/savesummary",
                defaults: new { controller = "Vacancy", action = "SaveSummary" }
                );

            routes.LowercaseUrls = true;

        }
    }
}
