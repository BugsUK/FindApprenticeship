namespace SFA.Apprenticeships.Application.UnitTests.Candidates.Strategies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Candidates.Configuration;
    using Application.Candidates.Strategies;
    using Configuration;
    using Domain.Entities.Candidates;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using FluentAssertions;
    using Interfaces.Communications;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SendAccountRemindersStrategyATests
    {
        private Guid CandidateId = Guid.Parse("727d9d37-3962-43d7-bcf2-a96e1bd93397");

        [TestCase(1, true)]
        [TestCase(2, false)]
        [TestCase(3, true)]
        [TestCase(4, false)]
        public void BaseStrategyOnDateOfBirth(int day, bool shouldSendReminder)
        {
            var dateOfBirth = new DateTime(1985, 1, day);

            var dateCreated = DateTime.UtcNow.AddDays(-7);

            var user = new UserBuilder(CandidateId).WithDateCreated(dateCreated).Activated(false).Build();
            var candidate = new CandidateBuilder(CandidateId).WithDateOfBirth(dateOfBirth).Build();

            var communicationService = new Mock<ICommunicationService>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyABuilder().With(successor.Object).With(communicationService).Build();

            Assert(shouldSendReminder, strategy, successor, communicationService, user, candidate);
        }

        [Test]
        public void DoNotSendReminderForActivatedUser()
        {
            var dateCreated = DateTime.UtcNow;

            var user = new UserBuilder(CandidateId).WithDateCreated(dateCreated).Activated(true).Build();
            var candidate = new CandidateBuilder(CandidateId).Build();

            var communicationService = new Mock<ICommunicationService>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyABuilder().With(successor.Object).With(communicationService).Build();

            Assert(false, strategy, successor, communicationService, user, candidate);
        }
        
        [Test]
        public void DoNotSendReminderBeforeSevenDays()
        {
            var dateCreated = DateTime.UtcNow;

            var user = new UserBuilder(CandidateId).WithDateCreated(dateCreated).Activated(false).Build();
            var candidate = new CandidateBuilder(CandidateId).Build();

            var communicationService = new Mock<ICommunicationService>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyABuilder().With(successor.Object).With(communicationService).Build();

            Assert(false, strategy, successor, communicationService, user, candidate);
        }
        
        [Test]
        public void SendReminderAfterSevenDays()
        {
            var dateCreated = DateTime.UtcNow.AddDays(-7);

            var user = new UserBuilder(CandidateId).WithDateCreated(dateCreated).Activated(false).Build();
            var candidate = new CandidateBuilder(CandidateId).Build();

            var communicationService = new Mock<ICommunicationService>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyABuilder().With(successor.Object).With(communicationService).Build();

            Assert(true, strategy, successor, communicationService, user, candidate);
        }
        
        [Test]
        public void DoNotSendReminderAfterTwentyDays()
        {
            var dateCreated = DateTime.UtcNow.AddDays(-20);

            var user = new UserBuilder(CandidateId).WithDateCreated(dateCreated).Activated(false).Build();
            var candidate = new CandidateBuilder(CandidateId).Build();

            var communicationService = new Mock<ICommunicationService>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyABuilder().With(successor.Object).With(communicationService).Build();

            Assert(false, strategy, successor, communicationService, user, candidate);
        }

        [Test]
        public void SendReminderAfterTwentyOneDays()
        {
            var dateCreated = DateTime.UtcNow.AddDays(-21);

            var user = new UserBuilder(CandidateId).WithDateCreated(dateCreated).Activated(false).Build();
            var candidate = new CandidateBuilder(CandidateId).Build();

            var communicationService = new Mock<ICommunicationService>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyABuilder().With(successor.Object).With(communicationService).Build();

            Assert(true, strategy, successor, communicationService, user, candidate);
        }

        [TestCase(0, false)]
        [TestCase(1, false)]
        [TestCase(2, false)]
        [TestCase(3, false)]
        [TestCase(4, false)]
        [TestCase(5, false)]
        [TestCase(6, false)]
        [TestCase(7, true)]
        [TestCase(8, false)]
        [TestCase(9, false)]
        [TestCase(10, false)]
        [TestCase(11, false)]
        [TestCase(12, false)]
        [TestCase(13, false)]
        [TestCase(14, false)]
        [TestCase(15, false)]
        [TestCase(16, false)]
        [TestCase(17, false)]
        [TestCase(18, false)]
        [TestCase(19, false)]
        [TestCase(20, false)]
        [TestCase(21, true)]
        [TestCase(22, false)]
        [TestCase(23, false)]
        [TestCase(24, false)]
        [TestCase(25, false)]
        [TestCase(26, false)]
        [TestCase(27, false)]
        [TestCase(28, false)]
        [TestCase(29, false)]
        [TestCase(30, false)]
        [TestCase(31, false)]
        [TestCase(32, false)]
        [TestCase(33, false)]
        [TestCase(34, false)]
        [TestCase(35, false)]
        [TestCase(36, false)]
        [TestCase(37, false)]
        [TestCase(38, false)]
        [TestCase(39, false)]
        [TestCase(40, false)]
        public void CompleteStrategyATestDays(int days, bool shouldSendReminder)
        {
            var dateCreated = DateTime.UtcNow.AddDays(-days).AddHours(-12);

            var user = new UserBuilder(CandidateId).WithDateCreated(dateCreated).Activated(false).Build();
            var candidate = new CandidateBuilder(CandidateId).Build();

            var communicationService = new Mock<ICommunicationService>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyABuilder().With(successor.Object).With(communicationService).Build();

            Assert(shouldSendReminder, strategy, successor, communicationService, user, candidate);
        }

        [TestCase(0, false)]
        [TestCase(1, false)]
        [TestCase(2, true)]
        [TestCase(3, false)]
        [TestCase(4, true)]
        [TestCase(5, false)]
        [TestCase(6, false)]
        [TestCase(7, false)]
        [TestCase(8, false)]
        [TestCase(9, false)]
        [TestCase(10, false)]
        public void CompleteStrategyATestHours(int hours, bool shouldSendReminder)
        {
            var dateCreated = DateTime.UtcNow.AddHours(-hours).AddMinutes(-30);

            var user = new UserBuilder(CandidateId).WithDateCreated(dateCreated).Activated(false).Build();
            var candidate = new CandidateBuilder(CandidateId).Build();

            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(s => s.Get<HousekeepingConfiguration>()).Returns(new HousekeepingConfigurationBuilder().WithStrategyA(1, 2, 4, 6).Build());
            var communicationService = new Mock<ICommunicationService>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyABuilder().With(configurationService).With(successor.Object).With(communicationService).Build();

            Assert(shouldSendReminder, strategy, successor, communicationService, user, candidate);
        }

        private static void Assert(bool shouldSendReminder, SendAccountRemindersStrategyA strategy, Mock<IHousekeepingStrategy> successor, Mock<ICommunicationService> communicationService, User user, Candidate candidate)
        {
            CommunicationToken[] communicationTokens = null;
            communicationService.Setup(s => s.SendMessageToCandidate(user.EntityId, MessageTypes.SendActivationCodeReminder, It.IsAny<IEnumerable<CommunicationToken>>()))
                .Callback<Guid, MessageTypes, IEnumerable<CommunicationToken>>((id, mt, ct) =>
                {
                    communicationTokens = ct.ToArray();
                });

            strategy.Handle(user, candidate);

            if (shouldSendReminder)
            {
                //Strategy handled the request
                successor.Verify(s => s.Handle(user, candidate), Times.Never);

                //Message was sent
                communicationService.Verify(s => s.SendMessageToCandidate(candidate.EntityId, MessageTypes.SendActivationCodeReminder, It.IsAny<IEnumerable<CommunicationToken>>()), Times.Once);
                communicationTokens.Should().NotBeNull();
                communicationTokens.Length.Should().Be(4);
                communicationTokens[0].Key.Should().Be(CommunicationTokens.CandidateFirstName);
                communicationTokens[0].Value.Should().Be(candidate.RegistrationDetails.FirstName);
                communicationTokens[1].Key.Should().Be(CommunicationTokens.ActivationCode);
                communicationTokens[1].Value.Should().Be(user.ActivationCode);
                communicationTokens[2].Key.Should().Be(CommunicationTokens.ActivationCodeExpiryDays);
                var activationCodeExpiryInDays = user.ActivateCodeExpiry.HasValue ? (user.ActivateCodeExpiry.Value - DateTime.UtcNow).Days : 0;
                var activationCodeExpiryInDaysFormatted = activationCodeExpiryInDays == 1 ? "1 day" : string.Format("{0} days", activationCodeExpiryInDays);
                communicationTokens[2].Value.Should().Be(activationCodeExpiryInDaysFormatted);
                communicationTokens[3].Key.Should().Be(CommunicationTokens.Username);
                communicationTokens[3].Value.Should().Be(candidate.RegistrationDetails.EmailAddress);
            }
            else
            {
                //Strategy did not handle the request
                successor.Verify(s => s.Handle(user, candidate), Times.Once);

                //Message was not sent
                communicationService.Verify(s => s.SendMessageToCandidate(user.EntityId, MessageTypes.SendActivationCodeReminder, It.IsAny<IEnumerable<CommunicationToken>>()), Times.Never);
                communicationTokens.Should().BeNull();
            }
        }
    }
}