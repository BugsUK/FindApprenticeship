namespace SFA.Apprenticeships.Web.Common.Configuration
{
    public class AnalyticsConfiguration
    {
        public bool EnableWebTrends { get; set; }
        public string WebTrendsDscId { get; set; }
        public bool EnableGoogleTagManager { get; set; }
        public string GoogleContainerId { get; set; }
        public bool EnableAppInsights { get; set; }
        public string AppInsightsInstrumentationKey { get; set; }
    }
}