namespace SFA.Apprenticeships.Infrastructure.Communication.Configuration
{
    public class CommunicationConfiguration
    {
        public bool IsEnabled { get; set; }

        public string SiteDomainName { get; set; }

        public string EmailDispatcher { get; set; }

        public string SmsDispatcher { get; set; }
    }
}
