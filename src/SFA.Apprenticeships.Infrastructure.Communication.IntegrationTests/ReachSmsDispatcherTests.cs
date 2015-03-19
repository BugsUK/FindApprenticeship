﻿namespace SFA.Apprenticeships.Infrastructure.Communication.IntegrationTests
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
        private const string BadToNumber = "0677878788978";
        private const string TestToNumber = "447876546700";

        private ISmsDispatcher _dispatcher;

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
        public void ShouldThrowIfRequestIsInvalid()
        {
            // Arrange.
            var request = new SmsRequest
            {
                ToNumber = "X",
                Tokens = TokenGenerator.CreatePasswordResetConfirmationTokens(),
                MessageType = MessageTypes.PasswordChanged
            };

            // Act.
            Action action = () => _dispatcher.SendSms(request);

            // Assert.
            action.ShouldThrow<DomainException>();
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
        public void ShoudSendApprenticeshipApplicationSuccessfulSms()
        {
            var request = new SmsRequest
            {
                ToNumber = TestToNumber,
                Tokens = TokenGenerator.CreateApprenticeshipApplicationStatusAlertTokens(),
                MessageType = MessageTypes.ApprenticeshipApplicationSuccessful
            };

            _dispatcher.SendSms(request);
        }

        [Test, Category("Integration")]
        public void ShoudSendApprenticeshipApplicationUnsuccessfulSms()
        {
            var request = new SmsRequest
            {
                ToNumber = TestToNumber,
                Tokens = TokenGenerator.CreateApprenticeshipApplicationStatusAlertTokens(),
                MessageType = MessageTypes.ApprenticeshipApplicationUnsuccessful
            };

            _dispatcher.SendSms(request);
        }

        [Test, Category("Integration")]
        public void ShoudSendApprenticeshipApplicationsUnsuccessfulSummarySms()
        {
            var request = new SmsRequest
            {
                ToNumber = TestToNumber,
                Tokens = TokenGenerator.CreateApprenticeshipApplicationStatusAlertsTokens(42),
                MessageType = MessageTypes.ApprenticeshipApplicationsUnsuccessfulSummary
            };

            _dispatcher.SendSms(request);
        }

        [Test, Category("Integration")]
        public void ShoudSendApprenticeshipApplicationExpiringDraftSms()
        {
            var request = new SmsRequest
            {
                ToNumber = TestToNumber,
                Tokens = TokenGenerator.CreateApprenticeshipApplicationExpiringDraftTokens(),
                MessageType = MessageTypes.ApprenticeshipApplicationExpiringDraft
            };

            _dispatcher.SendSms(request);
        }

        [Test, Category("Integration")]
        public void ShoudSendApprenticeshipApplicationExpiringDraftsSummarySms()
        {
            var request = new SmsRequest
            {
                ToNumber = TestToNumber,
                Tokens = TokenGenerator.CreateApprenticeshipApplicationExpiringDraftsTokens(42),
                MessageType = MessageTypes.ApprenticeshipApplicationExpiringDraftsSummary
            };

            _dispatcher.SendSms(request);
        }

        [Test, Category("Integration")]
        public void ShouldNotThrowIfToNumberIsBad()
        {
            // Arrange.
            var request = new SmsRequest
            {
                ToNumber = BadToNumber,
                Tokens = TokenGenerator.CreateMobileVerificationCodeTokens(),
                MessageType = MessageTypes.SendMobileVerificationCode
            };

            // Act.
            Action action = () => _dispatcher.SendSms(request);

            // Assert.
            action.ShouldNotThrow();
        }
    }
}