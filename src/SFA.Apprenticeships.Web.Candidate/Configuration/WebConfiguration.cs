namespace SFA.Apprenticeships.Web.Candidate.Configuration
{
    public class WebConfiguration
    {
        public const string ConfigurationName = "WebConfiguration";

        public int VacancyResultsPerPage { get; set; }

        public int LocationResultLimit { get; set; }

        public bool EnableWebTrends { get; set; }

        public string WebTrendsDscId { get; set; }

        public string SiteDomainName { get; set; }

        public string SiteRootRedirectUrl { get; set; }

        public string TermsAndConditionsVersion { get; set; }

        public string BlacklistedCategoryCodes { get; set; }
    }
}
