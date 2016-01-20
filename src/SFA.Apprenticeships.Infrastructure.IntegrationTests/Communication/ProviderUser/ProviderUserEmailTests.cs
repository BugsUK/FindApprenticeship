namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.Communication.ProviderUser
{
    using System;
    using Application.Interfaces.Communications;
    using Common.Configuration;
    using SFA.Infrastructure.Interfaces;
    using Common.IoC;
    using Infrastructure.Communication.IoC;
    using Moq;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class ProviderUserEmailTests
    {
        private const string TestToEmail = "valtechnas@gmail.com";

        private IEmailDispatcher _dispatcher;

        private readonly Mock<ILogService> _logServiceMock = new Mock<ILogService>();

        [SetUp]
        public void SetUp()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<CommunicationRegistry>();
            });

            container.Configure(c => c.For<ILogService>().Use(_logServiceMock.Object));

            _dispatcher = container.GetInstance<IEmailDispatcher>("SendGridEmailDispatcher");
            _logServiceMock.ResetCalls();
        }

        [Test, Category("Integration")]
        public void ShouldSendEmailVerificationCodeEmail()
        {
            var request = new EmailRequest
            {
                ToEmail = TestToEmail,
                Tokens = ProviderUserEmailTokenGenerator.SendEmailVerificationCodeTokens(),
                MessageType = MessageTypes.SendProviderUserEmailVerificationCode
            };

            _dispatcher.SendEmail(request);
            VerifyErrorsLogged(Times.Never());
        }

        #region Helpers

        private void VerifyErrorsLogged(Times times)
        {
            _logServiceMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<object[]>()), times);
        }

        #endregion
    }
}