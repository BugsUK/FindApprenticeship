namespace SFA.Apprenticeships.Web.Candidate
{
    using Common.Constants;
    using Constants;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes, RouteConfigOptions options = RouteConfigOptions.None)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.ico/{*pathInfo}");
            routes.IgnoreRoute("{folder}/{*pathInfo}", new { folder = "Content" });
            routes.IgnoreRoute("{folder}/{*pathInfo}", new { folder = "Scripts" });

            if ((options & RouteConfigOptions.MapMvcAttributeRoutes) == RouteConfigOptions.MapMvcAttributeRoutes)
            {
                // Required for ~/sitemap.xml.
                routes.MapMvcAttributeRoutes();
            }

            routes.MapRoute(
                name: RouteNames.SignOut,
                url: "signout",
                defaults: new { controller = "Login", action = "SignOut" }
                );

            routes.MapRoute(
                name: RouteNames.SignIn,
                url: "signin",
                defaults: new { controller = "Login", action = "Index" }
                );

            routes.MapRoute(
                name: RouteNames.Unlock,
                url: "unlock",
                defaults: new { controller = "Login", action = "Unlock" }
                );

            routes.MapRoute(
                name: RouteNames.ForgottenCredentials,
                url: "forgottencredentials",
                defaults: new { controller = "Login", action = "ForgottenCredentials" }
                );

            routes.MapRoute(
                name: RouteNames.ForgottenPassword,
                url: "forgottenpassword",
                defaults: new { controller = "Login", action = "ForgottenPassword" }
                );

            routes.MapRoute(
                name: RouteNames.ForgottenEmail,
                url: "forgottenemail",
                defaults: new { controller = "Login", action = "ForgottenEmail" }
                );

            routes.MapRoute(
                name: RouteNames.ResetPassword,
                url: "resetpassword",
                defaults: new { controller = "Login", action = "ResetPassword" }
                );

            routes.MapRoute(
                name: RouteNames.Register,
                url: "register",
                defaults: new { controller = "Register", action = "Index" }
                );

            routes.MapRoute(
                name: RouteNames.CheckUsername,
                url: "checkusername",
                defaults: new { controller = "Register", action = "CheckUsername" }
                );

            routes.MapRoute(
                name: RouteNames.Activate,
                url: "activate",
                defaults: new { controller = "Register", action = "Activate" }
                );

            routes.MapRoute(
                name: RouteNames.Activation,
                url: "activation",
                defaults: new { controller = "Register", action = "Activation" }
                );

            routes.MapRoute(
                name: RouteNames.MonitoringInformation,
                url: "tellusmore",
                defaults: new { controller = "Register", action = "MonitoringInformation" }
                );

            routes.MapRoute(
                name: RouteNames.SkipMonitoringInformation,
                url: "skiptellusmore",
                defaults: new { controller = "Register", action = "SkipMonitoringInformation" }
                );

            routes.MapRoute(
                name: RouteNames.WebTrendsOptOut,
                url: "webtrendsoptout",
                defaults: new { controller = "Home", action = "WebTrendsOptOut" }
                );

            routes.MapRoute(
                name: RouteNames.DismissPlannedOutageMessage,
                url: "dismissplannedoutagemessage",
                defaults: new { controller = "Account", action = "DismissPlannedOutageMessage" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.Settings,
                url: "settings",
                defaults: new { controller = "Account", action = "Settings" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.LocationSearch,
                url: "location",
                defaults: new { controller = "Location", action = "Location" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.ResendUpdateEmailAddressCode,
                url: "resendupdateemailaddresscode",
                defaults: new { controller = "Account", action = "ResendUpdateEmailAddressCode" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.SavedSearchesSettings,
                url: "savedsearches",
                defaults: new { controller = "Account", action = "SavedSearchesSettings" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.DeleteAccountSettings,
                url: "deleteaccount",
                defaults: new { controller = "Account", action = "DeleteAccountSettings" }
                );            

            routes.MapRoute(
                name: CandidateRouteNames.VerifyMobile,
                url: "verifymobile",
                defaults: new { controller = "Account", action = "VerifyMobile" }
                );

            routes.MapRoute(
                name: RouteNames.UpdateEmail,
                url: "updateemail",
                defaults: new { controller = "Account", action = "UpdateEmailAddress" }
                );

            routes.MapRoute(
                name: RouteNames.VerifyUpdatedEmail,
                url: "verifyemail",
                defaults: new { controller = "Account", action = "VerifyUpdatedEmailAddress" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.MyApplications,
                url: "myapplications",
                defaults: new { controller = "Account", action = "Index" }
                );

            routes.MapRoute(
                name: RouteNames.UpdatedTermsAndConditions,
                url: "updatedtermsandconditions",
                defaults: new { controller = "Account", action = "UpdatedTermsAndConditions" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.Privacy,
                url: "privacy",
                defaults: new { controller = "Home", action = "Privacy" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.Helpdesk,
                url: "helpdesk",
                defaults: new { controller = "Home", action = "Helpdesk" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.Feedback,
                url: "feedback",
                defaults: new { controller = "Home", action = "Feedback" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.Terms,
                url: "terms",
                defaults: new { controller = "Home", action = "Terms" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.NextSteps,
                url: "nextsteps",
                defaults: new { controller = "Home", action = "NextSteps" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.HowToApply,
                url: "howtoapply",
                defaults: new { controller = "Home", action = "HowToApply" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.ApprenticeshipResults,
                url: "apprenticeships",
                defaults: new { controller = "ApprenticeshipSearch", action = "Results" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.ApprenticeshipSearch,
                url: "apprenticeshipsearch",
                defaults: new { controller = "ApprenticeshipSearch", action = "Index" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.ApprenticeshipSearchSaveSearch,
                url: "apprenticeshipsearch/savesearch",
                defaults: new { controller = "ApprenticeshipSearch", action = "SaveSearch" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.ApprenticeshipDetailsWithDistance,
                url: "apprenticeshipdetail/{id}/{distance}",
                defaults: new { controller = "ApprenticeshipSearch", action = "DetailsWithDistance" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.RedirectToExternalWebsite,
                url: "apprenticeship/redirecttoexternalwebsite/{id}",
                defaults: new { controller = "ApprenticeshipSearch", action = "RedirectToExternalWebsite" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.ApprenticeshipSaveVacancy,
                url: "apprenticeship/save",
                defaults: new { controller = "ApprenticeshipApplication", action = "SaveVacancy" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.ApprenticeshipDeleteSavedVacancy,
                url: "apprenticeship/deletesaved",
                defaults: new { controller = "ApprenticeshipApplication", action = "DeleteSavedVacancy" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.ApprenticeshipDetails,
                url: "apprenticeship/{id}",
                defaults: new { controller = "ApprenticeshipSearch", action = "Details" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.ApprenticeshipApply,
                url: "apprenticeship/apply/{id}",
                defaults: new { controller = "ApprenticeshipApplication", action = "Apply" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.ApprenticeshipPreview,
                url: "apprenticeship/preview/{id}",
                defaults: new { controller = "ApprenticeshipApplication", action = "Preview" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.ApprenticeshipWhatNext,
                url: "apprenticeship/whatnext/{id}",
                defaults: new { controller = "ApprenticeshipApplication", action = "WhatHappensNext" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.ApprenticeshipView,
                url: "apprenticeship/view/{id}",
                defaults: new { controller = "ApprenticeshipApplication", action = "View" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.ApprenticeshipAutoSave,
                url: "apprenticeship/autosave/{id}",
                defaults: new { controller = "ApprenticeshipApplication", action = "AutoSave" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.ApprenticeshipResume,
                url: "apprenticeship/resume/{id}",
                defaults: new { controller = "ApprenticeshipApplication", action = "Resume" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.ApprenticeshipArchive,
                url: "apprenticeship/archive/{id}",
                defaults: new { controller = "Account", action = "Archive" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.ApprenticeshipDelete,
                url: "apprenticeship/delete/{id}",
                defaults: new { controller = "Account", action = "Delete" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.ApprenticeshipTrack,
                url: "apprenticeship/track/{id}",
                defaults: new { controller = "Account", action = "Track" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.TraineeshipOverview,
                url: "traineeships/about",
                defaults: new { controller = "TraineeshipSearch", action = "Overview" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.TraineeshipSearch,
                url: "traineeshipsearch",
                defaults: new { controller = "TraineeshipSearch", action = "Index" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.TraineeshipApply,
                url: "traineeship/apply/{id}",
                defaults: new { controller = "TraineeshipApplication", action = "Apply" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.TraineeshipResults,
                url: "traineeships/search",
                defaults: new { controller = "TraineeshipSearch", action = "Results" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.TraineeshipDetails,
                url: "traineeship/{id}",
                defaults: new { controller = "TraineeshipSearch", action = "Details" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.TraineeshipWhatNext,
                url: "traineeship/whatnext/{id}",
                defaults: new { controller = "TraineeshipApplication", action = "WhatHappensNext" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.TraineeshipView,
                url: "traineeship/view/{id}",
                defaults: new { controller = "TraineeshipApplication", action = "View" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.DismissTraineeshipPrompts,
                url: "traineeships/dismissprompts",
                defaults: new { controller = "Account", action = "DismissTraineeshipPrompts" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.DismissApplicationNotifications,
                url: "apprenticeships/dismissnotifications",
                defaults: new { controller = "Account", action = "DismissApplicationNotifications" }
                );

            routes.MapRoute(
                name: CandidateRouteNames.DeleteSavedSearch,
                url: "savedsearch/delete/{id}",
                defaults: new { controller = "Account", action = "DeleteSavedSearch" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: CandidateRouteNames.Maintenance,
                url: "maintenance",
                defaults: "~/403.aspx"
                );

            routes.MapRoute(
                name: CandidateRouteNames.Unsubscribe,
                url: "unsubscribe",
                defaults: new { controller = "Unsubscribe", action = "Index" }
                );

            routes.LowercaseUrls = true;
        }
    }
}
