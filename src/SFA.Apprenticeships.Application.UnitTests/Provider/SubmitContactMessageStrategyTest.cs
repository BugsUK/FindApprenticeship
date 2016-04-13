using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.Apprenticeships.Application.UnitTests.Provider
{
    using FluentAssertions;

    using Moq;

    using NUnit.Framework;

    using Interfaces.Communications;

    using Apprenticeships.Application.UserAccount.Configuration;
    using Domain.Entities.Communication;

    using SFA.Apprenticeships.Application.UserAccount.Strategies.ProviderUserAccount;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    public class SubmitContactMessageStrategyTest
    {
        private Mock<ILogService> _logService;
        private Mock<IConfigurationService> _configurationService;
        private Mock<IProviderCommunicationService> _communicationService;
        private SubmitContactMessageStrategy _submitContactMessageStrategy;
        private const string ValidUsername = "john.doe@example.com";
        private const string ValidHelpdeskEmailAddress = "helpdesk@example.com";
        

        [SetUp]
        public void SetUp()
        {
            _logService=new Mock<ILogService>();
            _configurationService=new Mock<IConfigurationService>();
            _communicationService = new Mock<IProviderCommunicationService>();
            _submitContactMessageStrategy=new SubmitContactMessageStrategy(_logService.Object,_communicationService.Object,_configurationService.Object);
            _configurationService.Setup(
               cm => cm.Get<UserAccountConfiguration>())
               .Returns(new UserAccountConfiguration
               {
                   HelpdeskEmailAddress = ValidHelpdeskEmailAddress
               });
        }

        [Test]
        public void SubmitContactMessageTest()
        {
            //Arrange
            ProviderContactMessage providerContactMessage = new ProviderContactMessage
                                                                {
                                                                    Email = "someone@sfa.com",
                                                                    Details = "Message Details",
                                                                    Name = ValidUsername,
                                                                    Type =ContactMessageTypes.ContactUs,
                                                                    Enquiry = "Message Enquiry"
                                                                };

            var communicationTokens = default(IEnumerable<CommunicationToken>);

            _communicationService.Setup(mock =>
                mock.SendMessageToProviderUser(
                    It.IsAny<string>(),
                    It.IsAny<MessageTypes>(),
                    It.IsAny<IEnumerable<CommunicationToken>>()))
                .Callback<string, MessageTypes, IEnumerable<CommunicationToken>>(
                    (u, m, t) => communicationTokens = t);

            //Act
            _submitContactMessageStrategy.SubmitMessage(providerContactMessage);

            _communicationService.Verify(mock =>
               mock.SendMessageToProviderUser(
                   ValidUsername,
                   MessageTypes.ProviderContactUsMessage,
                   It.IsAny<IEnumerable<CommunicationToken>>()),
                   Times.Once);

            communicationTokens.ShouldAllBeEquivalentTo(new[]
            {
                new CommunicationToken(CommunicationTokens.UserFullName, providerContactMessage.Name),
                new CommunicationToken(CommunicationTokens.UserEnquiryDetails, providerContactMessage.Details),
                new CommunicationToken(CommunicationTokens.RecipientEmailAddress, ValidHelpdeskEmailAddress),
                new CommunicationToken(CommunicationTokens.UserEmailAddress, providerContactMessage.Email),
                new CommunicationToken(CommunicationTokens.UserEnquiry, providerContactMessage.Enquiry)
            });
        }
    }
}
