namespace SFA.Apprenticeships.Application.UnitTests.Candidates.Strategies.ActivationReminder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apprenticeships.Application.Candidates.Configuration;
    using Apprenticeships.Application.Candidates.Strategies;
    using Apprenticeships.Application.Candidates.Strategies.ActivationReminder;
    using Configuration;
    using Domain.Entities.Candidates;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Entities.Users;
    using SFA.Infrastructure.Interfaces;
    using FluentAssertions;
    using Interfaces.Communications;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SendAccountRemindersStrategyTests
    {
        private readonly Guid _candidateId = Guid.Parse("727d9d37-3962-43d7-bcf2-a96e1bd93396");

        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(3, true)]
        [TestCase(4, true)]
        public void DoNotBaseStrategyOnDateOfBirth(int day, bool shouldSendReminder)
        {
            var dateOfBirth = new DateTime(1985, 1, day);
            var dateCreated = DateTime.UtcNow.AddDays(-1);

            var user = new UserBuilder(_candidateId).WithDateCreated(dateCreated).Activated(false).Build();
            var candidate = new CandidateBuilder(_candidateId).WithDateOfBirth(dateOfBirth).Build();

            var communicationService = new Mock<ICommunicationService>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyBuilder().With(successor.Object).With(communicationService).Build();

            Assert(shouldSendReminder, strategy, successor, communicationService, user, candidate);
        }

        [Test]
        public void DoNotSendReminderForActivatedUser()
        {
            var dateOfBirth = new DateTime(1985, 1, 2);

            var dateCreated = DateTime.UtcNow;

            var user = new UserBuilder(_candidateId).WithDateCreated(dateCreated).Activated(true).Build();
            var candidate = new CandidateBuilder(_candidateId).WithDateOfBirth(dateOfBirth).Build();

            var communicationService = new Mock<ICommunicationService>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyBuilder().With(successor.Object).With(communicationService).Build();

            Assert(false, strategy, successor, communicationService, user, candidate);
        }
        
        [Test]
        public void DoNotSendReminderBeforeOneDay()
        {
            var dateOfBirth = new DateTime(1985, 1, 2);

            var dateCreated = DateTime.UtcNow;

            var user = new UserBuilder(_candidateId).WithDateCreated(dateCreated).Activated(false).Build();
            var candidate = new CandidateBuilder(_candidateId).WithDateOfBirth(dateOfBirth).Build();

            var communicationService = new Mock<ICommunicationService>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyBuilder().With(successor.Object).With(communicationService).Build();

            Assert(false, strategy, successor, communicationService, user, candidate);
        }
        
        [Test]
        public void SendReminderAfterOneDay()
        {
            var dateOfBirth = new DateTime(1985, 1, 2);

            var dateCreated = DateTime.UtcNow.AddDays(-1);

            var user = new UserBuilder(_candidateId).WithDateCreated(dateCreated).Activated(false).Build();
            var candidate = new CandidateBuilder(_candidateId).WithDateOfBirth(dateOfBirth).Build();

            var communicationService = new Mock<ICommunicationService>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyBuilder().With(successor.Object).With(communicationService).Build();

            Assert(true, strategy, successor, communicationService, user, candidate);
        }
        
        [Test]
        public void DoNotSendReminderAfterTwoDays()
        {
            var dateOfBirth = new DateTime(1985, 1, 2);

            var dateCreated = DateTime.UtcNow.AddDays(-2);

            var user = new UserBuilder(_candidateId).WithDateCreated(dateCreated).Activated(false).Build();
            var candidate = new CandidateBuilder(_candidateId).WithDateOfBirth(dateOfBirth).Build();

            var communicationService = new Mock<ICommunicationService>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyBuilder().With(successor.Object).With(communicationService).Build();

            Assert(false, strategy, successor, communicationService, user, candidate);
        }

        [TestCase(0, false)]
        [TestCase(1, true)]
        [TestCase(2, false)]
        [TestCase(3, false)]
        [TestCase(4, false)]
        [TestCase(5, false)]
        [TestCase(6, false)]
        [TestCase(7, false)]
        [TestCase(8, true)]
        [TestCase(9, false)]
        [TestCase(10, false)]
        [TestCase(11, false)]
        [TestCase(12, false)]
        [TestCase(13, false)]
        [TestCase(14, false)]
        [TestCase(15, true)]
        [TestCase(16, false)]
        [TestCase(17, false)]
        [TestCase(18, false)]
        [TestCase(19, false)]
        [TestCase(20, false)]
        [TestCase(21, false)]
        [TestCase(22, true)]
        [TestCase(23, false)]
        [TestCase(24, false)]
        [TestCase(25, false)]
        [TestCase(26, false)]
        [TestCase(27, false)]
        [TestCase(28, false)]
        [TestCase(29, true)]
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
        public void CompleteStrategyTestDays(int days, bool shouldSendReminder)
        {
            var dateOfBirth = new DateTime(1985, 1, 2);

            var dateCreated = DateTime.UtcNow.AddDays(-days).AddHours(-12);

            var user = new UserBuilder(_candidateId).WithDateCreated(dateCreated).Activated(false).Build();
            var candidate = new CandidateBuilder(_candidateId).WithDateOfBirth(dateOfBirth).Build();

            var communicationService = new Mock<ICommunicationService>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyBuilder().With(successor.Object).With(communicationService).Build();

            Assert(shouldSendReminder, strategy, successor, communicationService, user, candidate);
        }

        [TestCase(0, false)]
        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(3, true)]
        [TestCase(4, true)]
        [TestCase(5, true)]
        [TestCase(6, false)]
        [TestCase(7, false)]
        [TestCase(8, false)]
        [TestCase(9, false)]
        [TestCase(10, false)]
        public void CompleteStrategyTestHours(int hours, bool shouldSendReminder)
        {
            var dateOfBirth = new DateTime(1985, 1, 2);

            var dateCreated = DateTime.UtcNow.AddHours(-hours).AddMinutes(-30);

            var user = new UserBuilder(_candidateId).WithDateCreated(dateCreated).Activated(false).Build();
            var candidate = new CandidateBuilder(_candidateId).WithDateOfBirth(dateOfBirth).Build();

            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(s => s.Get<HousekeepingConfiguration>()).Returns(new HousekeepingConfigurationBuilder().WithStrategy(1, 1, 1, 6).Build());
            var communicationService = new Mock<ICommunicationService>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyBuilder().With(configurationService).With(successor.Object).With(communicationService).Build();

            Assert(shouldSendReminder, strategy, successor, communicationService, user, candidate);
        }

        private static void Assert(bool shouldSendReminder, SendAccountRemindersStrategy strategy, Mock<IHousekeepingStrategy> successor, Mock<ICommunicationService> communicationService, User user, Candidate candidate)
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
                
                var activationCodeExpiryInDays = user.ActivateCodeExpiry.HasValue ? (user.ActivateCodeExpiry.Value - DateTime.UtcNow).Days : 0;
                var activationCodeExpiryInDaysFormatted = activationCodeExpiryInDays == 1 ? "1 day" : string.Format("{0} days", activationCodeExpiryInDays);
                if (user.ActivateCodeExpiry != null)
                {
                    activationCodeExpiryInDaysFormatted += " on " + user.ActivateCodeExpiry.Value.ToLongDateString();
                }

                var expectedCommunicationTokens = new[]
                {
                    new CommunicationToken(CommunicationTokens.CandidateFirstName, candidate.RegistrationDetails.FirstName),
                    new CommunicationToken(CommunicationTokens.ActivationCode, user.ActivationCode),
                    new CommunicationToken(CommunicationTokens.ActivationCodeExpiryDays, activationCodeExpiryInDaysFormatted),
                    new CommunicationToken(CommunicationTokens.Username, candidate.RegistrationDetails.EmailAddress)
                };

                communicationTokens.ShouldBeEquivalentTo(expectedCommunicationTokens);
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