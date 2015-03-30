﻿namespace SFA.Apprenticeships.Infrastructure.UnitTests.Communication.Email.EmailMessageFormatters.Builders
{
    using Domain.Interfaces.Configuration;
    using Infrastructure.Communication.Configuration;
    using Infrastructure.Communication.Email.EmailMessageFormatters;
    using Moq;

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