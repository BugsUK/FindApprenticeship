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

            routes.MapRoute(
                name: RecruitmentRouteNames.LandingPage,
                url: "",
                defaults: new { controller = "Home", action = "Index" }
                );

            routes.MapRoute(
                name: RecruitmentRouteNames.Authorize,
                url: "authorize",
                defaults: new { controller = "ProviderUser", action = "Authorize" }
                );

            routes.MapRoute(
                name: RecruitmentRouteNames.AuthorizationError,
                url: "authorize",
                defaults: new { controller = "ProviderUser", action = "AuthorizationError" }
                );

            routes.MapRoute(
                name: RecruitmentRouteNames.Privacy,
                url: "privacy",
                defaults: new { controller = "Home", action = "Privacy" }
                );

            routes.MapRoute(
                name: RecruitmentRouteNames.TermsAndConditions,
                url: "terms",
                defaults: new { controller = "Home", action = "TermsAndConditions" }
                );

            routes.MapRoute(
                name: RecruitmentRouteNames.ContactUs,
                url: "contact",
                defaults: new { controller = "Home", action = "ContactUs" }
                );

            routes.MapRoute(
                name: RecruitmentRouteNames.SignIn,
                url: "signin",
                defaults: new { controller = "Account", action = "SignIn" }
                );

            routes.MapRoute(
                name: RecruitmentRouteNames.SignOut,
                url: "signout",
                defaults: new { controller = "Account", action = "SignOut" }
                );

            routes.MapRoute(
                name: RecruitmentRouteNames.SessionTimeout,
                url: "sessiontimeout",
                defaults: new { controller = "Account", action = "SessionTimeout" }
                );

            routes.MapRoute(
                name: RecruitmentRouteNames.SignOutCallback,
                url: "signedout",
                defaults: new { controller = "Account", action = "SignOutCallback" }
                );

            routes.MapRoute(
               name: RecruitmentRouteNames.RecruitmentHome,
               url: "home",
               defaults: new { controller = "ProviderUser", action = "Home" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.Settings,
               url: "user",
               defaults: new { controller = "ProviderUser", action = "Settings" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.VerifyEmail,
               url: "verifyemail",
               defaults: new { controller = "ProviderUser", action = "VerifyEmail" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.ResendVerificationCode,
               url: "verifyemail/resend",
               defaults: new { controller = "ProviderUser", action = "ResendVerificationCode" }
               );

            routes.MapRoute(
                name: RecruitmentRouteNames.ManageProviderSites,
                url: "sites",
                defaults: new { controller = "Provider", action = "Sites" }
                );

            routes.MapRoute(
               name: RecruitmentRouteNames.AddProviderSite,
               url: "sites/add",
               defaults: new { controller = "Provider", action = "AddSite" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.EditProviderSite,
               url: "sites/edit",
               defaults: new { controller = "Provider", action = "EditSite" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.CreateVacancy,
               url: "vacancy/create",
               defaults: new { controller = "VacancyPosting", action = "CreateVacancy" }
               );

            routes.MapRoute(
                name: RecruitmentRouteNames.SelectExistingEmployer,
                url: "vacancy/employer/select",
                defaults: new {controller = "VacancyPosting", action = "SelectEmployer" }
                );

            routes.MapRoute(
                name: RecruitmentRouteNames.AddEmployer,
                url: "vacancy/employer/add",
                defaults: new { controller = "VacancyPosting", action = "AddEmployer" }
                );

            routes.MapRoute(
                name: RecruitmentRouteNames.ComfirmEmployer,
                url: "vacancy/employer/confirm",
                defaults: new {controller = "VacancyPosting", action = "ConfirmEmployer"}
                );

            routes.MapRoute(
               name: RecruitmentRouteNames.SubmitVacancy,
               url: "vacancy/submit",
               defaults: new { controller = "VacancyPosting", action = "SubmitVacancy" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.VacancySubmitted,
               url: "vacancy/submitted",
               defaults: new { controller = "VacancyPosting", action = "VacancySubmitted" }
               );

            routes.LowercaseUrls = true;

        }
    }
}
