namespace SFA.Apprenticeships.Infrastructure.Communication.UnitTests.Email.EmailMessageFormatters.Builders
{
    using Communication.Email.EmailMessageFormatters;
    using Configuration;
    using Domain.Interfaces.Configuration;
    using Moq;

    public class EmailDailyDigestMessageFormatterBuilder
    {
        private readonly Mock<IConfigurationService> _configurationService;

        public EmailDailyDigestMessageFormatterBuilder()
        {
            _configurationService = new Mock<IConfigurationService>();
            _configurationService.Setup(cm => cm.Get<CommunicationConfiguration>(CommunicationConfiguration.ConfigurationName)).Returns(new CommunicationConfiguration(){SiteDomainName = "test.findapprenticeship.service.gov.uk"});
        }

        public EmailDailyDigestMessageFormatter Build()
        {
            return new EmailDailyDigestMessageFormatter(_configurationService.Object);
        }
    }
}