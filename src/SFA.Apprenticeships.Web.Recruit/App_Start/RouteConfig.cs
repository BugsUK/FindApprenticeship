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
               name: RecruitmentRouteNames.AutoSaveCreateVacancy,
               url: "vacancy/autosavecreate",
               defaults: new { controller = "VacancyPosting", action = "AutoSaveCreateVacancy" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.ReviewCreateVacancy,
               url: "vacancy/create/review",
               defaults: new { controller = "VacancyPosting", action = "ReviewCreateVacancy" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.TrainingDetails,
               url: "vacancy/training",
               defaults: new { controller = "VacancyPosting", action = "TrainingDetails" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.AutoSaveTrainingDetails,
               url: "vacancy/autosavetrainingdetails",
               defaults: new { controller = "VacancyPosting", action = "AutoSaveTrainingDetails" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.ReviewTrainingDetails,
               url: "vacancy/training/review",
               defaults: new { controller = "VacancyPosting", action = "ReviewTrainingDetails" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.VacancySummary,
               url: "vacancy/summary",
               defaults: new { controller = "VacancyPosting", action = "VacancySummary" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.AutoSaveVacancySummary,
               url: "vacancy/autosaveVacancySummary",
               defaults: new { controller = "VacancyPosting", action = "AutoSaveVacancySummary" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.ReviewVacancySummary,
               url: "vacancy/summary/review",
               defaults: new { controller = "VacancyPosting", action = "ReviewVacancySummary" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.VacancyRequirementsProspects,
               url: "vacancy/requirementsprospects",
               defaults: new { controller = "VacancyPosting", action = "VacancyRequirementsProspects" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.AutoSaveRequirementsProspects,
               url: "vacancy/autosaveRequirementsProspects",
               defaults: new { controller = "VacancyPosting", action = "AutoSaveRequirementsProspects" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.ReviewVacancyRequirementsProspects,
               url: "vacancy/requirementsprospects/review",
               defaults: new { controller = "VacancyPosting", action = "ReviewVacancyRequirementsProspects" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.VacancyQuestions,
               url: "vacancy/questions",
               defaults: new { controller = "VacancyPosting", action = "VacancyQuestions" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.AutoSaveVacancyQuestions,
               url: "vacancy/autosaveVacancyQuestions",
               defaults: new { controller = "VacancyPosting", action = "AutoSaveVacancyQuestions" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.ReviewVacancyQuestions,
               url: "vacancy/questions/review",
               defaults: new { controller = "VacancyPosting", action = "ReviewVacancyQuestions" }
               );

            routes.MapRoute(
                name: RecruitmentRouteNames.SelectExistingEmployer,
                url: "vacancy/employer/select",
                defaults: new {controller = "VacancyPosting", action = "SelectEmployer" }
                );

            routes.MapRoute(
                name: RecruitmentRouteNames.ConfirmEmployerSelection,
                url: "vacancy/employer/confirmSelection",
                defaults: new { controller = "VacancyPosting", action = "ConfirmEmployerSelection" }
                );

            routes.MapRoute(
                name: RecruitmentRouteNames.SearchExistingEmployer,
                url: "vacancy/employer/searchexisting",
                defaults: new { controller = "VacancyPosting", action = "SearchExistingEmployer" }
                );
            
            routes.MapRoute(
                name: RecruitmentRouteNames.AddEmployer,
                url: "vacancy/employer/add",
                defaults: new { controller = "VacancyPosting", action = "AddEmployer" }
                );

            routes.MapRoute(
                name: RecruitmentRouteNames.ConfirmEmployer,
                url: "vacancy/employer/confirm",
                defaults: new {controller = "VacancyPosting", action = "ConfirmEmployer"}
                );

            routes.MapRoute(
               name: RecruitmentRouteNames.PreviewVacancy,
               url: "vacancy/preview",
               defaults: new { controller = "VacancyPosting", action = "PreviewVacancy" }
               );

            routes.MapRoute(
                name: RecruitmentRouteNames.SelectNewEmployer,
                url: "vacancy/employer/selectnew",
                defaults: new { controller = "VacancyPosting", action = "SelectNewEmployer" }
                );

            routes.MapRoute(
                name: RecruitmentRouteNames.ConfirmNewEmployerSelection,
                url: "vacancy/employer/confirmSelectNew",
                defaults: new { controller = "VacancyPosting", action = "ConfirmNewEmployerSelection" }
                );

            routes.MapRoute(
                name: RecruitmentRouteNames.ComfirmNewEmployer,
                url: "vacancy/employer/confirmnew",
                defaults: new { controller = "VacancyPosting", action = "ConfirmNewEmployer" }
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

            routes.MapRoute(
               name: RecruitmentRouteNames.CloneVacancy,
               url: "vacancy/clone",
               defaults: new { controller = "VacancyPosting", action = "CloneVacancy" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.AddLocations,
               url: "vacancy/locations",
               defaults: new { controller = "VacancyPosting", action = "Locations" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.SearchAddresses,
               url: "vacancy/searchAddresses",
               defaults: new { controller = "VacancyPosting", action = "SearchAddresses" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.ShowLocations,
               url: "vacancy/showLocations",
               defaults: new { controller = "VacancyPosting", action = "ShowLocations" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.ManageDates,
               url: "vacancy/manageDates",
               defaults: new { controller = "VacancyPosting", action = "ManageDates" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.AutoSaveManageDates,
               url: "vacancy/autosaveManageDates",
               defaults: new { controller = "VacancyPosting", action = "AutoSaveManageDates" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.VacancyApplications,
               url: "vacancy/applications",
               defaults: new { controller = "Application", action = "VacancyApplications" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.ReviewApprenticeshipApplication,
               url: "apprenticeshipapplication/review",
               defaults: new { controller = "ApprenticeshipApplication", action = "Review" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.ConfirmSuccessfulApprenticeshipApplication,
               url: "apprenticeshipapplication/confirmsuccessfuldecision",
               defaults: new { controller = "ApprenticeshipApplication", action = "ConfirmSuccessfulDecision" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.ConfirmUnsuccessfulApprenticeshipApplication,
               url: "apprenticeshipapplication/confirmunsuccessfuldecision",
               defaults: new { controller = "ApprenticeshipApplication", action = "ConfirmUnsuccessfulDecision" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.ReviewTraineeshipApplication,
               url: "traineeshipapplication/review",
               defaults: new { controller = "TraineeshipApplication", action = "Review" }
               );

            routes.LowercaseUrls = true;
        }
    }
}
