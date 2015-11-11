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
                url: "authorizationerror",
                defaults: new { controller = "ProviderUser", action = "AuthorizationError" }
                );

            /* LEO: These are in all 3 applications
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
            */

            routes.MapRoute(
                name: RecruitmentRouteNames.SessionTimeout,
                url: "Recruit/Account/sessiontimeout",
                defaults: new { controller = "Account", action = "SessionTimeout" }
                );

            routes.MapRoute(
                name: RecruitmentRouteNames.SignOutCallback,
                url: "Recruit/Account/signedout",
                defaults: new { controller = "Account", action = "SignOutCallback" }
                );

            routes.MapRoute(
               name: RecruitmentRouteNames.RecruitmentHome,
               url: "Recruit/ProviderUser/home",
               defaults: new { controller = "ProviderUser", action = "Home" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.Settings,
               url: "Recruit/ProviderUser/user",
               defaults: new { controller = "ProviderUser", action = "Settings" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.VerifyEmail,
               url: "Recruit/ProviderUser/verifyemail",
               defaults: new { controller = "ProviderUser", action = "VerifyEmail" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.ResendVerificationCode,
               url: "Recruit/ProviderUser/verifyemail/resend",
               defaults: new { controller = "ProviderUser", action = "ResendVerificationCode" }
               );

            routes.MapRoute(
                name: RecruitmentRouteNames.ManageProviderSites,
                url: "Recruit/Provider/Sites",
                defaults: new { controller = "Provider", action = "Sites" }
                );

            routes.MapRoute(
               name: RecruitmentRouteNames.AddProviderSite,
               url: "Recruit/Provider/sites/add",
               defaults: new { controller = "Provider", action = "AddSite" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.EditProviderSite,
               url: "Recruit/Provider/sites/edit",
               defaults: new { controller = "Provider", action = "EditSite" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.CreateVacancy,
               url: "Recruit/vacancy/create",
               defaults: new { controller = "VacancyPosting", action = "CreateVacancy" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.EditVacancy,
               url: "Recruit/vacancy/edit",
               defaults: new { controller = "VacancyPosting", action = "EditVacancy" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.VacancySummary,
               url: "Recruit/vacancy/summary",
               defaults: new { controller = "VacancyPosting", action = "VacancySummary" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.VacancyRequirementsProspects,
               url: "Recruit/vacancy/requirementsprospects",
               defaults: new { controller = "VacancyPosting", action = "VacancyRequirementsProspects" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.VacancyQuestions,
               url: "Recruit/vacancy/question",
               defaults: new { controller = "VacancyPosting", action = "VacancyQuestions" }
               );

            routes.MapRoute(
                name: RecruitmentRouteNames.SelectExistingEmployer,
                url: "Recruit/vacancy/employer/select",
                defaults: new {controller = "VacancyPosting", action = "SelectEmployer" }
                );

            routes.MapRoute(
                name: RecruitmentRouteNames.SearchExistingEmployer,
                url: "Recruit/vacancyposting/searchexistingemployer",
                defaults: new { controller = "VacancyPosting", action = "SearchExistingEmployer" }
                );
            
            routes.MapRoute(
                name: RecruitmentRouteNames.AddEmployer,
                url: "Recruit/vacancy/employer/add",
                defaults: new { controller = "VacancyPosting", action = "AddEmployer" }
                );

            routes.MapRoute(
                name: RecruitmentRouteNames.ComfirmEmployer,
                url: "Recruit/vacancy/employer/confirm",
                defaults: new {controller = "VacancyPosting", action = "ConfirmEmployer"}
                );

            routes.MapRoute(
               name: RecruitmentRouteNames.PreviewVacancy,
               url: "Recruit/vacancy/preview",
               defaults: new { controller = "VacancyPosting", action = "PreviewVacancy" }
               );

            routes.MapRoute(
                name: RecruitmentRouteNames.SelectNewEmployer,
                url: "Recruit/vacancy/employer/selectnew",
                defaults: new { controller = "VacancyPosting", action = "SelectNewEmployer" }
                );

            routes.MapRoute(
                name: RecruitmentRouteNames.ComfirmNewEmployer,
                url: "Recruit/vacancy/employer/confirmnew",
                defaults: new { controller = "VacancyPosting", action = "ConfirmNewEmployer" }
                );

            routes.MapRoute(
               name: RecruitmentRouteNames.SubmitVacancy,
               url: "Recruit/vacancy/submit",
               defaults: new { controller = "VacancyPosting", action = "SubmitVacancy" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.VacancySubmitted,
               url: "Recruit/vacancy/submitted",
               defaults: new { controller = "VacancyPosting", action = "VacancySubmitted" }
               );

            routes.LowercaseUrls = true;

        }
    }
}
