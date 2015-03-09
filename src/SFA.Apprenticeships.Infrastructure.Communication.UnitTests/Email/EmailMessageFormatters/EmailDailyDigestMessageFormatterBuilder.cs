namespace SFA.Apprenticeships.Infrastructure.Communication.UnitTests.Email.EmailMessageFormatters
{
    using Communication.Email.EmailMessageFormatters;
    using Domain.Interfaces.Configuration;
    using Moq;

    public class EmailDailyDigestMessageFormatterBuilder
    {
        private readonly Mock<IConfigurationManager> _configurationManager;

        public EmailDailyDigestMessageFormatterBuilder()
        {
            _configurationManager = new Mock<IConfigurationManager>();
            _configurationManager.Setup(cm => cm.GetAppSetting<string>("SiteDomainName")).Returns("test.findapprenticeship.service.gov.uk");
        }

        public EmailDailyDigestMessageFormatter Build()
        {
            var formatter = new EmailDailyDigestMessageFormatter(_configurationManager.Object);
            return formatter;
        }
    }
}