namespace SFA.Apprenticeships.Infrastructure.Communication.IntegrationTests
{
    using System;
    using Application.Interfaces.Communications;
    using FluentAssertions;
    using Logging.IoC;
    using NUnit.Framework;
    using Domain.Entities.Exceptions;
    using Common.IoC;
    using IoC;
    using Sms;
    using StructureMap;

    [TestFixture]
    public class ReachSmsDispatcherTests
    {
        private ISmsDispatcher _dispatcher;
        private const string InvalidTestToNumber = "X";
        private const string TestToNumber = "447876546700";

        [SetUp]
        public void SetUp()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<CommunicationRegistry>();
            });

            _dispatcher = container.GetInstance<ISmsDispatcher>("ReachSmsDispatcher");
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShoudConstructReachEmailDispatcher()
        {
            Assert.IsNotNull(_dispatcher);
            Assert.IsInstanceOf<ReachSmsDispatcher>(_dispatcher);
        }

        [Test, Category("Integration")]
        public void ShouldSendApprenticeshipApplicationSubmittedSms()
        {
            var request = new SmsRequest
            {
                ToNumber = TestToNumber,
                Tokens = TokenGenerator.CreateApprenticeshipApplicationSubmittedTokens(),
                MessageType = MessageTypes.ApprenticeshipApplicationSubmitted
            };

            _dispatcher.SendSms(request);
        }

        [Test, Category("Integration")]
        public void ShouldSendTraineeshipApplicationSubmittedSms()
        {
            var request = new SmsRequest
            {
                ToNumber = TestToNumber,
                Tokens = TokenGenerator.CreateTraineeshipApplicationSubmittedTokens(),
                MessageType = MessageTypes.TraineeshipApplicationSubmitted
            };

            _dispatcher.SendSms(request);
        }

        [Test, Category("Integration")]
        public void ShoudSendDailyDigestSms()
        {
            var request = new SmsRequest
            {
                ToNumber = TestToNumber,
                Tokens = TokenGenerator.CreateDailyDigestTokens(2),
                MessageType = MessageTypes.DailyDigest
            };

            _dispatcher.SendSms(request);
        }

        [Test, Category("Integration")]
        public void ShoudSendSavedSearchAlertSms()
        {
            var request = new SmsRequest
            {
                ToNumber = TestToNumber,
                Tokens = TokenGenerator.CreateSavedSearchAlertTokens(5),
                MessageType = MessageTypes.SavedSearchAlert
            };

            _dispatcher.SendSms(request);
        }

        [Test, Category("Integration")]
        public void ShouldThrowIfRequestIsInvalid()
        {
            // Arrange.
            var request = new SmsRequest
            {
                ToNumber = InvalidTestToNumber,
                Tokens = TokenGenerator.CreatePasswordResetConfirmationTokens(),
                MessageType = MessageTypes.PasswordChanged
            };

            // Act.
            Action action = () => _dispatcher.SendSms(request);

            // Assert.
            action.ShouldThrow<DomainException>();
        }
    }
}