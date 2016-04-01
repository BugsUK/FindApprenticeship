namespace SFA.Apprenticeships.Infrastructure.FAAUnitTests.Communication.Email.EmailMessageFormatters.Builders
{
    using Infrastructure.Communication.Configuration;
    using Infrastructure.Communication.Email.EmailMessageFormatters;
    using Moq;
    using SFA.Infrastructure.Interfaces;

    public class EmailDailyDigestMessageFormatterBuilder
    {
        private readonly Mock<IConfigurationService> _configurationService;

        public EmailDailyDigestMessageFormatterBuilder()
        {
            _configurationService = new Mock<IConfigurationService>();
            _configurationService.Setup(cm => cm.Get<CommunicationConfiguration>()).Returns(new CommunicationConfiguration(){SiteDomainName = "test.findapprenticeship.service.gov.uk"});
        }

        public EmailDailyDigestMessageFormatter Build()
        {
            return new EmailDailyDigestMessageFormatter(_configurationService.Object);
        }
    }
}