namespace SFA.Apprenticeships.Infrastructure.Communication.UnitTests.Email.EmailMessageFormatters.Builders
{
    using Communication.Email.EmailMessageFormatters;
    using Domain.Interfaces.Configuration;
    using Moq;

    public class EmailSavedSearchAlertMessageFormatterBuilder
    {
        private readonly Mock<IConfigurationManager> _configurationManager;

        public EmailSavedSearchAlertMessageFormatterBuilder()
        {
            _configurationManager = new Mock<IConfigurationManager>();
            _configurationManager.Setup(cm => cm.GetAppSetting<string>("SiteDomainName")).Returns("test.findapprenticeship.service.gov.uk");
        }

        public EmailSavedSearchAlertMessageFormatter Build()
        {
            return new EmailSavedSearchAlertMessageFormatter(_configurationManager.Object);
        }
    }
}