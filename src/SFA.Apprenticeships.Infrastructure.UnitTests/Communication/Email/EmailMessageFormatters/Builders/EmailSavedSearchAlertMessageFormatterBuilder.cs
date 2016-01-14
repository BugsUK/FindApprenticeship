namespace SFA.Apprenticeships.Infrastructure.UnitTests.Communication.Email.EmailMessageFormatters.Builders
{
    using SFA.Infrastructure.Interfaces;
    using Infrastructure.Communication.Configuration;
    using Infrastructure.Communication.Email.EmailMessageFormatters;
    using Moq;

    public class EmailSavedSearchAlertMessageFormatterBuilder
    {
        private readonly Mock<IConfigurationService> _configurationService;

        public EmailSavedSearchAlertMessageFormatterBuilder()
        {
            _configurationService = new Mock<IConfigurationService>();
            _configurationService.Setup(cm => cm.Get<CommunicationConfiguration>()).Returns(new CommunicationConfiguration { SiteDomainName = "test.findapprenticeship.service.gov.uk" });
            _configurationService.Setup(cm => cm.Get<EmailConfiguration>()).Returns(new EmailConfiguration { SubCategoriesFullNamesLimit = 5});
        }

        public EmailSavedSearchAlertMessageFormatter Build()
        {
            return new EmailSavedSearchAlertMessageFormatter(_configurationService.Object);
        }
    }
}