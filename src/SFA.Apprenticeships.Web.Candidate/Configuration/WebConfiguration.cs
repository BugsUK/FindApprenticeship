namespace SFA.Apprenticeships.Infrastructure.Web.Configuration
{
    public class WebConfiguration
    {
        public static string WebConfigurationName { get { return "WebConfiguration"; } }

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
