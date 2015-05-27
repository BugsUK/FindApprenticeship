namespace SFA.Apprenticeships.Application.UnitTests.Candidates.Strategies.DormantAccount
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Candidates.Strategies;
    using Application.Candidates.Strategies.DormantAccount;
    using Domain.Entities.Candidates;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Entities.Users;
    using FluentAssertions;
    using Interfaces.Communications;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SendAccountRemindersStrategyTests
    {
        [TestCase(UserStatuses.Unknown, false)]
        [TestCase(UserStatuses.PendingActivation, false)]
        [TestCase(UserStatuses.Active, true)]
        [TestCase(UserStatuses.Inactive, false)]
        [TestCase(UserStatuses.Locked, true)]
        [TestCase(UserStatuses.Dormant, false)]
        [TestCase(UserStatuses.PendingDeletion, false)]
        public void StatusChecks(UserStatuses userStatus, bool shouldSendReminder)
        {
            var lastLogin = DateTime.UtcNow.AddDays(-90);

            var candidateId = Guid.NewGuid();
            var user = new UserBuilder(candidateId).WithLastLogin(lastLogin).WithStatus(userStatus).Build();
            var candidate = new CandidateBuilder(candidateId).Build();

            var communicationService = new Mock<ICommunicationService>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyBuilder().With(successor.Object).With(communicationService).Build();

            Assert(shouldSendReminder, strategy, successor, communicationService, user, candidate);
        }

        [Test]
        public void DoNotSendReminderBefore90Days()
        {
            var lastLogin = DateTime.UtcNow.AddDays(-89);

            var candidateId = Guid.NewGuid();
            var user = new UserBuilder(candidateId).WithLastLogin(lastLogin).WithStatus(UserStatuses.Active).Build();
            var candidate = new CandidateBuilder(candidateId).Build();

            var communicationService = new Mock<ICommunicationService>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyBuilder().With(successor.Object).With(communicationService).Build();

            Assert(false, strategy, successor, communicationService, user, candidate);
        }

        [Test]
        public void SendReminderAfter90Days()
        {
            var lastLogin = DateTime.UtcNow.AddDays(-90);

            var candidateId = Guid.NewGuid();
            var user = new UserBuilder(candidateId).WithLastLogin(lastLogin).WithStatus(UserStatuses.Active).Build();
            var candidate = new CandidateBuilder(candidateId).Build();

            var communicationService = new Mock<ICommunicationService>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyBuilder().With(successor.Object).With(communicationService).Build();

            Assert(true, strategy, successor, communicationService, user, candidate);
        }

        [Test]
        public void DoNotSendReminderBefore330Days()
        {
            var lastLogin = DateTime.UtcNow.AddDays(-329);

            var candidateId = Guid.NewGuid();
            var user = new UserBuilder(candidateId).WithLastLogin(lastLogin).WithStatus(UserStatuses.Active).Build();
            var candidate = new CandidateBuilder(candidateId).Build();

            var communicationService = new Mock<ICommunicationService>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyBuilder().With(successor.Object).With(communicationService).Build();

            Assert(false, strategy, successor, communicationService, user, candidate);
        }

        [Test]
        public void SendReminderAfter330Days()
        {
            var lastLogin = DateTime.UtcNow.AddDays(-330);

            var candidateId = Guid.NewGuid();
            var user = new UserBuilder(candidateId).WithLastLogin(lastLogin).WithStatus(UserStatuses.Active).Build();
            var candidate = new CandidateBuilder(candidateId).Build();

            var communicationService = new Mock<ICommunicationService>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyBuilder().With(successor.Object).With(communicationService).Build();

            Assert(true, strategy, successor, communicationService, user, candidate);
        }

        private static void Assert(bool shouldSendReminder, SendAccountRemindersStrategy strategy, Mock<IHousekeepingStrategy> successor, Mock<ICommunicationService> communicationService, User user, Candidate candidate)
        {
            CommunicationToken[] communicationTokens = null;
            communicationService.Setup(s => s.SendMessageToCandidate(user.EntityId, MessageTypes.SendDormantAccountReminder, It.IsAny<IEnumerable<CommunicationToken>>()))
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
                communicationService.Verify(s => s.SendMessageToCandidate(candidate.EntityId, MessageTypes.SendDormantAccountReminder, It.IsAny<IEnumerable<CommunicationToken>>()), Times.Once);
                communicationTokens.Should().NotBeNull();

                var lastLogin = user.LastLogin ?? DateTime.UtcNow;
                var lastLoginInDays = (DateTime.UtcNow - lastLogin).Days;
                var lastLoginInDaysFormatted = lastLoginInDays > 270 ? "almost a year" : "90 days";

                var expectedCommunicationTokens = new[]
                {
                    new CommunicationToken(CommunicationTokens.CandidateFirstName, candidate.RegistrationDetails.FirstName),
                    new CommunicationToken(CommunicationTokens.LastLogin, lastLoginInDaysFormatted),
                    new CommunicationToken(CommunicationTokens.AccountExpiryDate, lastLogin.AddDays(365).ToLongDateString()),
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