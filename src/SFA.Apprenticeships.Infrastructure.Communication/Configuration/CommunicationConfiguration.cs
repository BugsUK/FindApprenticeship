namespace SFA.Apprenticeships.Infrastructure.Communication.Configuration
{
    public class CommunicationConfiguration
    {
        public bool IsEnabled { get; set; }

        public string CandidateSiteDomainName { get; set; }

        public string RecruitSiteDomainName { get; set; }

        public string ManageSiteDomainName { get; set; }

        public string EmailDispatcher { get; set; }

        public string SmsDispatcher { get; set; }
    }
}
