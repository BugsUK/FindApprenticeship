namespace SFA.Apprenticeships.Web.Common.Configuration
{
    public class CommonWebConfiguration
    {
        public int VacancyResultsPerPage { get; set; }

        public int LocationResultLimit { get; set; }

        public string SiteDomainName { get; set; }

        public string SiteRootRedirectUrl { get; set; }

        public string TermsAndConditionsVersion { get; set; }

        public string BlacklistedCategoryCodes { get; set; }

        public string ApprenticeshipFeedbackUrl { get; set; }

        public string TraineeshipFeedbackUrl { get; set; }

        public bool IsWebsiteOffline { get; set; }

        public string WebsiteOfflineMessage { get; set; }

        public string CodeGenerator { get; set; }

        public int UnsuccessfulApplicationsToShowTraineeshipsPrompt { get; set; }

        public string PlannedOutageMessage { get; set; }

        public bool ShowAbout { get; set; }

        public string Environment { get; set; }

        public Features Features { get ; set; }

        public int SubCategoriesFullNamesLimit { get; set; }

        public string RaaApiBaseUrl { get; set; }

        public string GoogleMapsPrivateKey { get; set; }
    }

    public class Features
    {
        public bool SavedSearchesEnabled { get; set; }

        public bool SmsEnabled { get; set; }

        public bool RaaApiEnabled { get; set; }
    }
}
