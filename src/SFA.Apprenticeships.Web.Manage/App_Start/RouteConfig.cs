namespace SFA.Apprenticeships.Web.Manage
{
    using Constants;
    using System.Web.Mvc;
    using System.Web.Routing;

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
                defaults: new { controller = "AgencyUser", action = "Dashboard" }
                );

            routes.MapRoute(
                name: ManagementRouteNames.ChangeTeam,
                url: "changeteam",
                defaults: new { controller = "AgencyUser", action = "ChangeTeam" }
                );

            routes.MapRoute(
                name: ManagementRouteNames.ReviewVacancy,
                url: "vacancy/review",
                defaults: new { controller = "Vacancy", action = "Review" }
                );

            routes.MapRoute(
                name: ManagementRouteNames.ReserveForQA,
                url: "vacancy/reserveForQA",
                defaults: new { controller = "Vacancy", action = "ReserveForQA" }
                );

            routes.MapRoute(
                name: ManagementRouteNames.UnReserveForQA,
                url: "vacancy/unReserveForQA",
                defaults: new { controller = "Vacancy", action = "UnReserveForQA" }
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
                name: ManagementRouteNames.TrainingDetails,
                url: "vacancy/training",
                defaults: new { controller = "Vacancy", action = "TrainingDetails" }
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

            routes.MapRoute(
               name: ManagementRouteNames.Candidates,
               url: "candidates",
               defaults: new { controller = "Candidate", action = "Index" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.SearchCandidates,
               url: "candidates/search",
               defaults: new { controller = "Candidate", action = "Search" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.ViewCandidate,
               url: "candidate",
               defaults: new { controller = "Candidate", action = "Candidate" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.ViewCandidateApprenticeshipApplication,
               url: "candidate/apprenticeship",
               defaults: new { controller = "Candidate", action = "Apprenticeship" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.ViewCandidateApprenticeshipApplicationFeedback,
               url: "candidate/applicationfeedback",
               defaults: new { controller = "Candidate", action = "ApplicationFeedback" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.ViewCandidateTraineeshipApplication,
               url: "candidate/traineeship",
               defaults: new { controller = "Candidate", action = "Traineeship" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.ViewCandidateApprenticeshipVacancy,
               url: "candidate/apprenticeship/vacancy",
               defaults: new { controller = "Candidate", action = "ApprenticeshipVacancy" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.ViewCandidateTraineeshipVacancy,
               url: "candidate/traineeship/vacancy",
               defaults: new { controller = "Candidate", action = "TraineeshipVacancy" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.ReportList,
               url: "reports",
               defaults: new { controller = "Report", action = "Index" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.ReportVacanciesList,
               url: "reports/vacancies",
               defaults: new { controller = "Report", action = "VacanciesListCsv" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.ReportSuccessfulCandidates,
               url: "reports/successfulcandidates",
               defaults: new { controller = "Report", action = "SuccessfulCandidatesCsv" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.ReportUnsuccessfulCandidates,
               url: "reports/unsuccessfulcandidates",
               defaults: new { controller = "Report", action = "UnsuccessfulCandidatesCsv" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.ReportVacancyExtensions,
               url: "reports/vacancyextensions",
               defaults: new { controller = "Report", action = "VacancyExtensionsCsv" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.ReportRegisteredCandidates,
               url: "reports/registeredcandidates",
               defaults: new { controller = "Report", action = "RegisteredCandidatesCsv" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.VacancyTracker,
               url: "reports/vacancytracker",
               defaults: new { controller = "Report", action = "VacancyTrackerCsv" }
               );

            routes.MapRoute(
                name: ManagementRouteNames.WebTrendsOptOut,
                url: "webtrendsoptout",
                defaults: new { controller = "Home", action = "WebTrendsOptOut" }
                );

            routes.MapRoute(
                name: ManagementRouteNames.Cookies,
                url: "cookies",
                defaults: new { controller = "Home", action = "Cookies" }
                );

            routes.MapRoute(
                name: ManagementRouteNames.InformationRadiator,
                url: "informationradiator",
                defaults: new { controller = "InformationRadiator", action = "Index" }
                );

            routes.MapRoute(
               name: ManagementRouteNames.AdminList,
               url: "admin",
               defaults: new { controller = "Admin", action = "Index" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.AdminProviders,
               url: "admin/providers",
               defaults: new { controller = "Admin", action = "Providers" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.AdminViewProvider,
               url: "admin/provider",
               defaults: new { controller = "Admin", action = "Provider" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.AdminCreateProvider,
               url: "admin/providers/create",
               defaults: new { controller = "Admin", action = "CreateProvider" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.AdminEditProvider,
               url: "admin/providers/edit",
               defaults: new { controller = "Admin", action = "EditProvider" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.AdminProviderSites,
               url: "admin/providersites",
               defaults: new { controller = "Admin", action = "ProviderSites" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.AdminViewProviderSite,
               url: "admin/providersite",
               defaults: new { controller = "Admin", action = "ProviderSite" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.AdminCreateProviderSite,
               url: "admin/providersites/create",
               defaults: new { controller = "Admin", action = "CreateProviderSite" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.AdminEditProviderSite,
               url: "admin/providersite/edit",
               defaults: new { controller = "Admin", action = "EditProviderSite" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.AdminDeleteProviderSiteRelationship,
               url: "admin/providersiterelationship/delete",
               defaults: new { controller = "Admin", action = "DeleteProviderSiteRelationship" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.AdminApiUsers,
               url: "admin/apiusers",
               defaults: new { controller = "Admin", action = "ApiUsers" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.AdminViewApiUser,
               url: "admin/apiuser",
               defaults: new { controller = "Admin", action = "ApiUser" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.AdminCreateApiUser,
               url: "admin/apiuser/create",
               defaults: new { controller = "Admin", action = "CreateApiUser" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.AdminEditApiUser,
               url: "admin/apiuser/edit",
               defaults: new { controller = "Admin", action = "EditApiUser" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.AdminResetApiUserPassword,
               url: "admin/apiuser/resetpassword",
               defaults: new { controller = "Admin", action = "ResetApiUserPassword" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.AdminExportApiUsers,
               url: "admin/apiusers/csv",
               defaults: new { controller = "Admin", action = "ExportApiUsers" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.AdminEmployers,
               url: "admin/employers",
               defaults: new { controller = "Admin", action = "Employers" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.AdminViewEmployer,
               url: "admin/employer",
               defaults: new { controller = "Admin", action = "Employer" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.AdminEditEmployer,
               url: "admin/employers/edit",
               defaults: new { controller = "Admin", action = "EditEmployer" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.AdminStandards,
               url: "admin/standards",
               defaults: new { controller = "Admin", action = "Standards" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.AdminUpdateStandard,
               url: "admin/standard",
               defaults: new { controller = "Admin", action = "UpdateStandard" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.AdminStandardsCsv,
               url: "admin/standards/csv",
               defaults: new { controller = "Admin", action = "DownloadStandardsCsv" }
               );

            routes.MapRoute(
                name: ManagementRouteNames.AdminFrameworks,
                url: "admin/frameworks",
                defaults: new { controller = "Admin", action = "Frameworks" }
                );

            routes.MapRoute(
               name: ManagementRouteNames.AdminUpdateFramework,
               url: "admin/framework",
               defaults: new { controller = "Admin", action = "UpdateFramework" }
               );

            routes.MapRoute(
               name: ManagementRouteNames.AdminFrameworksCsv,
               url: "admin/frameworks/csv",
               defaults: new { controller = "Admin", action = "DownloadFrameworksCsv" }
               );

            routes.LowercaseUrls = true;
        }
    }
}
