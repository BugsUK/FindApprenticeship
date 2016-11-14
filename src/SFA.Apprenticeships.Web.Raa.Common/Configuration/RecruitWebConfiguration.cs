namespace SFA.Apprenticeships.Web.Raa.Common.Configuration
{
    using Web.Common.Configuration;

    public class RecruitWebConfiguration : AnalyticsConfiguration
    {
        public int PageSize { get; set; }
        public int AutoSaveTimeoutInSeconds { get; set; }
    }
}