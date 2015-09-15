namespace SFA.Apprenticeships.Web.Recruit
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

            //TO BE DELETED
            routes.MapRoute(
                name: "LoginDummy",
                url: "login",
                defaults: new {controller = "Home", action = "LoginDummy" }
                );

            routes.MapRoute(
                name: RecruitmentRouteNames.LandingPage,
                url: "",
                defaults: new { controller = "Home", action = "Index" }
                );

            routes.MapRoute(
                name: RecruitmentRouteNames.Authorize,
                url: "authorize",
                defaults: new { controller = "Home", action = "Authorize" }
                );

            routes.MapRoute(
               name: RecruitmentRouteNames.RecruitmentHome,
               url: "home",
               defaults: new { controller = "ProviderUser", action = "Home" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.UserInfo,
               url: "user",
               defaults: new { controller = "ProviderUser", action = "UserInfo" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.VertifyEmail,
               url: "verifyemail",
               defaults: new { controller = "ProviderUser", action = "VerifyEmail" }
               );

            routes.MapRoute(
                name: RecruitmentRouteNames.ManageProviderSites,
                url: "sites",
                defaults: new { controller = "Provider", action = "Sites" }
                );

            routes.MapRoute(
               name: RecruitmentRouteNames.AddProviderSite,
               url: "addsite",
               defaults: new { controller = "Provider", action = "AddSite" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.EditProviderSite,
               url: "editsite",
               defaults: new { controller = "Provider", action = "EditSite" }
               );

            routes.LowercaseUrls = true;

        }
    }
}
