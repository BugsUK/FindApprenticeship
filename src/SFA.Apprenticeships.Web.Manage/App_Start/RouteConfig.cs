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
                name: ManagementRouteNames.Summary,
                url: "vacancy/summary",
                defaults: new { controller = "Vacancy", action = "Summary" }
                );

            routes.MapRoute(
                name: ManagementRouteNames.BasicDetails,
                url: "vacancy/basic",
                defaults: new { controller = "Vacancy", action = "BasicDetails" }
                );

            routes.MapRoute(
                name: ManagementRouteNames.RequirementsAndProspoects,
                url: "vacancy/requirementsprospects",
                defaults: new { controller = "Vacancy", action = "RequirementsAndProspects" }
                );

            routes.MapRoute(
                name: ManagementRouteNames.Questions,
                url: "vacancy/questions",
                defaults: new { controller = "Vacancy", action = "Questions" }
                );

            routes.MapRoute(
                name: ManagementRouteNames.EmployerInformation,
                url: "vacancy/employerinformation",
                defaults: new { controller = "Vacancy", action = "EmployerInformation" }
                );

            routes.MapRoute(
               name: ManagementRouteNames.AddLocations,
               url: "vacancy/locations",
               defaults: new { controller = "Vacancy", action = "Locations" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.SearchAddresses,
               url: "vacancy/searchAddresses",
               defaults: new { controller = "Vacancy", action = "SearchAddresses" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.ShowLocations,
               url: "vacancy/showLocations",
               defaults: new { controller = "Vacancy", action = "ShowLocations" }
               );

            routes.LowercaseUrls = true;
        }
    }
}
