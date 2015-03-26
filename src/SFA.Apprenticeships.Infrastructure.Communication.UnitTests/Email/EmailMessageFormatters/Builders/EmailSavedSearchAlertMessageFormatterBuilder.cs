namespace SFA.Apprenticeships.Infrastructure.Communication.UnitTests.Email.EmailMessageFormatters.Builders
{
    using Communication.Email.EmailMessageFormatters;
    using Configuration;
    using Domain.Interfaces.Configuration;
    using Moq;

    public class EmailSavedSearchAlertMessageFormatterBuilder
    {
        private readonly Mock<IConfigurationService> _configurationService;

        public EmailSavedSearchAlertMessageFormatterBuilder()
        {
            _configurationService = new Mock<IConfigurationService>();
            _configurationService.Setup(cm => cm.Get<CommunicationConfiguration>()).Returns(new CommunicationConfiguration() { SiteDomainName = "test.findapprenticeship.service.gov.uk" });
        }

        public EmailSavedSearchAlertMessageFormatter Build()
        {
            return new EmailSavedSearchAlertMessageFormatter(_configurationService.Object);
        }
    }
}