namespace SFA.Apprenticeships.Web.Recruit
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
                name: RecruitmentRouteNames.OnBoardingComplete,
                url: "onboardingcomplete",
                defaults: new { controller = "ProviderUser", action = "OnBoardingComplete" }
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
               name: RecruitmentRouteNames.DismissReleaseNotes,
               url: "dismissreleasenotes",
               defaults: new { controller = "ProviderUser", action = "DismissReleaseNotes" }
               );

            routes.MapRoute(
                name: RecruitmentRouteNames.ManageProviderSites,
                url: "sites",
                defaults: new { controller = "Provider", action = "Sites" }
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
                defaults: new { controller = "VacancyPosting", action = "SelectEmployer" }
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
                defaults: new { controller = "VacancyPosting", action = "ConfirmEmployer" }
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
               name: RecruitmentRouteNames.DeleteVacancy,
               url: "vacancy/delete",
               defaults: new { controller = "VacancyManagement", action = "Delete" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.AddLocations,
               url: "vacancy/locations",
               defaults: new { controller = "VacancyPosting", action = "Locations" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.AutoSaveLocations,
               url: "vacancy/autosavelocations",
               defaults: new { controller = "VacancyPosting", action = "AutoSaveLocations" }
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
               name: RecruitmentRouteNames.CloseVacancy,
               url: "vacancy/closeVacancy",
               defaults: new { controller = "VacancyPosting", action = "CloseVacancy" }
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
               name: RecruitmentRouteNames.ShareApplications,
               url: "vacancy/shareapplications",
               defaults: new { controller = "Application", action = "ShareApplications" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.ArchiveVacancy,
               url: "vacancy/archive",
               defaults: new { controller = "VacancyStatus", action = "Archive" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.BulkDeclineCandidates,
               url: "vacancy/bulkdeclinecandidates",
               defaults: new { controller = "Application", action = "BulkDeclineCandidates" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.BulkDeclineCandidatesSearch,
               url: "vacancy/bulkdeclinecandidatessearch",
               defaults: new { controller = "Application", action = "BulkDeclineCandidatesSearch" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.ConfirmBulkDeclineCandidates,
               url: "vacancy/confirmbulkdeclinecandidates",
               defaults: new { controller = "Application", action = "ConfirmBulkDeclineCandidates" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.ConfirmArchiveVacancy,
               url: "vacancy/confirmarchive",
               defaults: new { controller = "VacancyStatus", action = "ConfirmArchive" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.ReviewApprenticeshipApplication,
               url: "apprenticeshipapplication/review",
               defaults: new { controller = "ApprenticeshipApplication", action = "Review" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.ViewAnonymousApprenticeshipApplication,
               url: "apprenticeshipapplication/anonymous",
               defaults: new { controller = "EmployerApplication", action = "ViewAnonymisedApprenticeship" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.ViewAnonymousTraineeshipApplication,
               url: "traineeshipapplication/anonymous",
               defaults: new { controller = "EmployerApplication", action = "ViewAnonymisedTraineeship" }
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
              name: RecruitmentRouteNames.ConfirmBulkUnsuccessfulApprenticeshipApplication,
              url: "apprenticeshipapplication/confirmbulkunsuccessfuldecision",
              defaults: new { controller = "ApprenticeshipApplication", action = "ConfirmBulkUnsuccessfulDecision" }
              );

            routes.MapRoute(
               name: RecruitmentRouteNames.ConfirmRevertToInProgress,
               url: "apprenticeshipapplication/confirmreverttoinprogress",
               defaults: new { controller = "ApprenticeshipApplication", action = "ConfirmRevertToInProgress" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.ReviewTraineeshipApplication,
               url: "traineeshipapplication/review",
               defaults: new { controller = "TraineeshipApplication", action = "Review" }
               );

            routes.MapRoute(
                name: RecruitmentRouteNames.WebTrendsOptOut,
                url: "webtrendsoptout",
                defaults: new { controller = "Home", action = "WebTrendsOptOut" }
                );

            routes.MapRoute(
                name: RecruitmentRouteNames.Cookies,
                url: "cookies",
                defaults: new { controller = "Home", action = "Cookies" }
                );

            routes.MapRoute(
               name: RecruitmentRouteNames.ReportList,
               url: "reports",
               defaults: new { controller = "Report", action = "Index" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.ReportApplicationsReceived,
               url: "reports/applicationsreceived",
               defaults: new { controller = "Report", action = "ApplicationsReceived" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.ReportCandidatesWithApplications,
               url: "reports/candidateswithapplications",
               defaults: new { controller = "Report", action = "CandidatesWithApplications" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.AdminList,
               url: "admin",
               defaults: new { controller = "Admin", action = "Index" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.AdminProviderUsers,
               url: "admin/providerusers",
               defaults: new { controller = "Admin", action = "ProviderUsers" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.AdminViewProviderUser,
               url: "admin/provideruser",
               defaults: new { controller = "Admin", action = "ProviderUser" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.AdminChangeUkprn,
               url: "admin/changeukprn",
               defaults: new { controller = "Admin", action = "ChangeUkprn" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.AdminResetUkprn,
               url: "admin/resetukprn",
               defaults: new { controller = "Admin", action = "ResetUkprn" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.TransferVacancies,
               url: "admin/transfervacancies",
               defaults: new { controller = "Admin", action = "TransferVacancies" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.SearchProvider,
               url: "admin/transfervacancies/searchprovider",
               defaults: new { controller = "Admin", action = "SearchProvider" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.ConfirmVacancies,
               url: "admin/transfervacancies/confirmvacancies",
               defaults: new { controller = "Admin", action = "ChooseProvider" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.AdminProviders,
               url: "admin/providers",
               defaults: new { controller = "Admin", action = "Providers" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.AdminViewProvider,
               url: "admin/provider",
               defaults: new { controller = "Admin", action = "Provider" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.ManageVacanciesTransfers,
               url: "admin/managevacanciestransfers",
               defaults: new { controller = "Admin", action = "ManageVacanciesTransfers" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.AdminViewProviderSite,
               url: "admin/providersite",
               defaults: new { controller = "Admin", action = "ProviderSite" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.CandidateSearch,
               url: "candidates",
               defaults: new { controller = "Candidate", action = "Index" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.SearchCandidates,
               url: "candidates/search",
               defaults: new { controller = "Candidate", action = "Search" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.ViewCandidate,
               url: "candidate",
               defaults: new { controller = "Candidate", action = "Candidate" }
               );

            routes.MapRoute(
               name: RecruitmentRouteNames.SortCandidate,
               url: "candidate/sort",
               defaults: new { controller = "Candidate", action = "SortCandidate" }
               );

            routes.MapRoute(
                name: RecruitmentRouteNames.InformationRadiator,
                url: "informationradiator",
                defaults: new { controller = "InformationRadiator", action = "Index" }
            );

            routes.LowercaseUrls = true;
        }
    }
}
