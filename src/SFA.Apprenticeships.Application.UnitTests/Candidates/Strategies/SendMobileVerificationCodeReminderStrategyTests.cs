namespace SFA.Apprenticeships.Application.UnitTests.Candidates.Strategies
{
    using System;
    using System.Collections.Generic;
    using Apprenticeships.Application.Candidates.Configuration;
    using Apprenticeships.Application.Candidates.Strategies;
    using Configuration;
    using Domain.Entities.UnitTests.Builder;
    using SFA.Infrastructure.Interfaces;
    using FluentAssertions;
    using Interfaces.Communications;
    using Moq;
    using NUnit.Framework;

    using SFA.Apprenticeships.Application.Interfaces;

    [TestFixture]
    public class SendMobileVerificationCodeReminderStrategyTests
    {
        private const string MobileVerificationCode = "6789";
        private const string PhoneNumber = "07999424242";

        [Test]
        public void ShouldSendWellFormedMobileVerificationCodeReminder()
        {
            // Arrange.
            var mockSuccessorStrategy = new Mock<IHousekeepingStrategy>();
            var mockCommunicationService = new Mock<ICommunicationService>();

            var strategy = new SendMobileVerificationCodeReminderStrategyBuilder()
                .With(mockSuccessorStrategy)
                .With(mockCommunicationService)
                .Build();

            var candidateId = Guid.NewGuid();

            var user = new UserBuilder(candidateId).Build();

            var candidate = new CandidateBuilder(candidateId)
                .PhoneNumber(PhoneNumber)
                .MobileVerificationCode(MobileVerificationCode)
                .MobileVerificationCodeDateCreated(UtcYesterday)
                .EnableAllCommunications()
                .Build();

            var expectedCommunicationTokens = new[]
            {
                new CommunicationToken(CommunicationTokens.CandidateMobileNumber, PhoneNumber),
                new CommunicationToken(CommunicationTokens.MobileVerificationCode, MobileVerificationCode)
            };

            IEnumerable<CommunicationToken> actualCommunicationTokens = null;

            mockCommunicationService
                .Setup(mock => mock.SendMessageToCandidate(
                    candidateId, MessageTypes.SendMobileVerificationCodeReminder, It.IsAny<IEnumerable<CommunicationToken>>()))
                .Callback<Guid, MessageTypes, IEnumerable<CommunicationToken>>((id, code, tokens) => actualCommunicationTokens = tokens);

            // Act.
            strategy.Handle(user, candidate);

            // Assert.
            mockSuccessorStrategy.Verify(mock => mock.Handle(user, candidate), Times.Once);

            mockCommunicationService.Verify(mock => mock.SendMessageToCandidate(
                candidateId, MessageTypes.SendMobileVerificationCodeReminder, It.IsAny<IEnumerable<CommunicationToken>>()), Times.Once);

            actualCommunicationTokens.ShouldBeEquivalentTo(expectedCommunicationTokens);
        }

        [TestCase(1, 1)]
        [TestCase(24, 1)]
        [TestCase(24, 5)]
        [TestCase(48, 2)]
        public void ShouldSendMobileVerificationCodeReminderInConfiguredHousekeepingCycle(
            int housekeepingCycleInHours,
            int sendMobileVerificationCodeReminderAfterCycles)
        {
            // Arrange.
            var mockSuccessorStrategy = new Mock<IHousekeepingStrategy>();
            var mockCommunicationService = new Mock<ICommunicationService>();

            var strategy = new SendMobileVerificationCodeReminderStrategyBuilder()
                .With(mockSuccessorStrategy)
                .With(mockCommunicationService)
                .WithHousekeepingCycleInHours(housekeepingCycleInHours)
                .WithSendMobileVerificationCodeReminderAfterCycles(sendMobileVerificationCodeReminderAfterCycles)
                .Build();

            var candidateId = Guid.NewGuid();
            var user = new UserBuilder(candidateId).Build();
            var hoursAgo = -(sendMobileVerificationCodeReminderAfterCycles * housekeepingCycleInHours);

            var candidate = new CandidateBuilder(candidateId)
                .PhoneNumber(PhoneNumber)
                .MobileVerificationCode(MobileVerificationCode)
                .MobileVerificationCodeDateCreated(DateTime.UtcNow.AddHours(hoursAgo))
                .EnableAllCommunications()
                .Build();

            // Act.
            strategy.Handle(user, candidate);

            // Assert.
            mockSuccessorStrategy.Verify(mock => mock.Handle(user, candidate), Times.Once);

            mockCommunicationService.Verify(mock => mock.SendMessageToCandidate(
                candidateId, MessageTypes.SendMobileVerificationCodeReminder, It.IsAny<IEnumerable<CommunicationToken>>()), Times.Once);
        }

        [TestCase(6)]
        [TestCase(48)]
        public void ShouldNotSendMobileVerificationCodeReminderOutsideConfiguredHousekeepingCycle(int hoursAgo)
        {
            // Arrange.
            var mockSuccessorStrategy = new Mock<IHousekeepingStrategy>();
            var mockCommunicationService = new Mock<ICommunicationService>();

            var strategy = new SendMobileVerificationCodeReminderStrategyBuilder()
                .With(mockSuccessorStrategy)
                .With(mockCommunicationService)
                .Build();

            var candidateId = Guid.NewGuid();
            var user = new UserBuilder(candidateId).Build();

            var candidate = new CandidateBuilder(candidateId)
                .PhoneNumber(PhoneNumber)
                .MobileVerificationCode(MobileVerificationCode)
                .MobileVerificationCodeDateCreated(DateTime.UtcNow.AddHours(-hoursAgo))
                .EnableAllCommunications()
                .Build();

            // Act.
            strategy.Handle(user, candidate);

            // Assert.
            mockSuccessorStrategy.Verify(mock => mock.Handle(user, candidate), Times.Once);

            mockCommunicationService.Verify(mock => mock.SendMessageToCandidate(
                candidateId, MessageTypes.SendMobileVerificationCode, It.IsAny<IEnumerable<CommunicationToken>>()), Times.Never);
        }

        [TestCase(null, true, true, true, true, false)]
        [TestCase("", true, true, true, true, false)]
        [TestCase("1234", false, false, false, false, false)]
        [TestCase("1234", false, false, false, true, true)]
        [TestCase("1234", false, false, true, false, true)]
        [TestCase("1234", false, true, false, false, true)]
        [TestCase("1234", true, false, false, false, true)]
        public void ShouldSendMobileVerificationCodeReminderBasedOnCandidateCommunicationPreferences(
            string mobileVerificationCode,
            bool applicationStatusChangePreferencesEnableText,
            bool expiringApplicationPreferencesEnableText,
            bool savedSearchPreferencesEnableText,
            bool marketingPreferencesEnableText,
            bool shouldSendMobileVerificationCode)
        {
            // Arrange.
            var mockSuccessorStrategy = new Mock<IHousekeepingStrategy>();
            var mockCommunicationService = new Mock<ICommunicationService>();

            var strategy = new SendMobileVerificationCodeReminderStrategyBuilder()
                .With(mockSuccessorStrategy)
                .With(mockCommunicationService)
                .Build();

            var candidateId = Guid.NewGuid();

            var user = new UserBuilder(candidateId).Build();

            var candidate = new CandidateBuilder(candidateId)
                .PhoneNumber(PhoneNumber)
                .MobileVerificationCode(mobileVerificationCode)
                .MobileVerificationCodeDateCreated(UtcYesterday)
                .EnableApplicationStatusChangeAlertsViaText(applicationStatusChangePreferencesEnableText)
                .EnableExpiringApplicationAlertsViaText(expiringApplicationPreferencesEnableText)
                .EnableSavedSearchAlertsViaEmailAndText(savedSearchPreferencesEnableText)
                .EnableMarketingViaText(marketingPreferencesEnableText)
                .Build();

            // Act.
            strategy.Handle(user, candidate);

            // Assert.
            mockSuccessorStrategy.Verify(mock => mock.Handle(user, candidate), Times.Once);

            var callCount = shouldSendMobileVerificationCode ? 1 : 0;

            mockCommunicationService.Verify(mock => mock.SendMessageToCandidate(
                candidateId, MessageTypes.SendMobileVerificationCodeReminder, It.IsAny<IEnumerable<CommunicationToken>>()), Times.Exactly(callCount));
        }

        private static DateTime UtcYesterday
        {
            get { return DateTime.UtcNow.AddDays(-1); }
        }

        private class SendMobileVerificationCodeReminderStrategyBuilder
        {
            private readonly Mock<ILogService> _mockLogger = new Mock<ILogService>();

            private Mock<ICommunicationService> _mockCommunicationService;
            private Mock<IHousekeepingStrategy> _mockSuccessorStrategy;

            private int _housekeepingCycleInHours = 24;
            private int _sendMobileVerificationCodeReminderAfterCycles = 1;

            public SendMobileVerificationCodeReminderStrategyBuilder()
            {
                _mockCommunicationService = new Mock<ICommunicationService>();
                _mockSuccessorStrategy = new Mock<IHousekeepingStrategy>();
            }

            public SendMobileVerificationCodeReminderStrategyBuilder WithHousekeepingCycleInHours(int housekeepingCycleInHours)
            {
                _housekeepingCycleInHours = housekeepingCycleInHours;
                return this;
            }

            public SendMobileVerificationCodeReminderStrategyBuilder WithSendMobileVerificationCodeReminderAfterCycles(int sendMobileVerificationCodeReminderAfterCycles)
            {
                _sendMobileVerificationCodeReminderAfterCycles = sendMobileVerificationCodeReminderAfterCycles;
                return this;
            }

            public SendMobileVerificationCodeReminderStrategyBuilder With(Mock<ICommunicationService> mockCommunicationService)
            {
                _mockCommunicationService = mockCommunicationService;
                return this;
            }

            public SendMobileVerificationCodeReminderStrategyBuilder With(Mock<IHousekeepingStrategy> mockHousekeepingStrategy)
            {
                _mockSuccessorStrategy = mockHousekeepingStrategy;
                return this;
            }

            public SendMobileVerificationCodeReminderStrategy Build()
            {
                var mockConfigurationService = new Mock<IConfigurationService>();

                mockConfigurationService.Setup(s =>
                    s.Get<HousekeepingConfiguration>())
                    .Returns(new HousekeepingConfigurationBuilder()
                    .SendMobileVerificationCodeReminderStrategy(_housekeepingCycleInHours, _sendMobileVerificationCodeReminderAfterCycles)
                    .Build());

                var strategy = new SendMobileVerificationCodeReminderStrategy(
                    _mockLogger.Object,
                    mockConfigurationService.Object,
                    _mockCommunicationService.Object);

                strategy.SetSuccessor(_mockSuccessorStrategy.Object);

                return strategy;
            }
        }
    }
}