namespace SFA.Apprenticeships.Infrastructure.UnitTests.Communication.Email.EmailMessageFormatters.Builders
{
    using Domain.Interfaces.Configuration;
    using Infrastructure.Communication.Configuration;
    using Infrastructure.Communication.Email.EmailMessageFormatters;
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