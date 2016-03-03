namespace SFA.Apprenticeships.Application.UnitTests.Candidates.Strategies.DormantAccount
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apprenticeships.Application.Candidates.Strategies;
    using Apprenticeships.Application.Candidates.Strategies.DormantAccount;
    using Domain.Entities.Candidates;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
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
        [TestCase(UserStatuses.Dormant, true)]
        [TestCase(UserStatuses.PendingDeletion, false)]
        public void StatusChecks(UserStatuses userStatus, bool shouldSendReminder)
        {
            var lastLogin = DateTime.UtcNow.AddDays(-90);

            var candidateId = Guid.NewGuid();
            var user = new UserBuilder(candidateId).WithLastLogin(lastLogin).WithStatus(userStatus).Build();
            var candidate = new CandidateBuilder(candidateId).Build();
            candidate.EnableAllOptionalCommunications();

            var userWriteRepository = new Mock<IUserWriteRepository>();
            var candidateWriteRepository = new Mock<ICandidateWriteRepository>();
            var communicationService = new Mock<ICommunicationService>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyBuilder().With(successor.Object).With(communicationService).With(userWriteRepository).With(candidateWriteRepository).Build();

            Assert(shouldSendReminder && userStatus != UserStatuses.Dormant, shouldSendReminder, strategy, successor, userWriteRepository, candidateWriteRepository, communicationService, user, candidate);
        }

        [Test]
        public void DoNotSendReminderBefore90Days()
        {
            var lastLogin = DateTime.UtcNow.AddDays(-89);

            var candidateId = Guid.NewGuid();
            var user = new UserBuilder(candidateId).WithLastLogin(lastLogin).WithStatus(UserStatuses.Active).Build();
            var candidate = new CandidateBuilder(candidateId).Build();
            candidate.EnableAllOptionalCommunications();

            var userWriteRepository = new Mock<IUserWriteRepository>();
            var candidateWriteRepository = new Mock<ICandidateWriteRepository>();
            var communicationService = new Mock<ICommunicationService>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyBuilder().With(successor.Object).With(communicationService).With(userWriteRepository).With(candidateWriteRepository).Build();

            Assert(false, false, strategy, successor, userWriteRepository, candidateWriteRepository, communicationService, user, candidate);
        }

        [Test]
        public void SendReminderAfter90Days()
        {
            var lastLogin = DateTime.UtcNow.AddDays(-90);

            var candidateId = Guid.NewGuid();
            var user = new UserBuilder(candidateId).WithLastLogin(lastLogin).WithStatus(UserStatuses.Active).Build();
            var candidate = new CandidateBuilder(candidateId).Build();
            candidate.EnableAllOptionalCommunications();

            var userWriteRepository = new Mock<IUserWriteRepository>();
            var candidateWriteRepository = new Mock<ICandidateWriteRepository>();
            var communicationService = new Mock<ICommunicationService>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyBuilder().With(successor.Object).With(communicationService).With(userWriteRepository).With(candidateWriteRepository).Build();

            Assert(true, true, strategy, successor, userWriteRepository, candidateWriteRepository, communicationService, user, candidate);
        }

        [Test]
        public void DoNotSendReminderAfter91Days()
        {
            var lastLogin = DateTime.UtcNow.AddDays(-91);

            var candidateId = Guid.NewGuid();
            var user = new UserBuilder(candidateId).WithLastLogin(lastLogin).WithStatus(UserStatuses.Dormant).Build();
            var candidate = new CandidateBuilder(candidateId).Build();
            candidate.EnableAllOptionalCommunications();

            var userWriteRepository = new Mock<IUserWriteRepository>();
            var candidateWriteRepository = new Mock<ICandidateWriteRepository>();
            var communicationService = new Mock<ICommunicationService>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyBuilder().With(successor.Object).With(communicationService).With(userWriteRepository).With(candidateWriteRepository).Build();

            Assert(false, false, strategy, successor, userWriteRepository, candidateWriteRepository, communicationService, user, candidate);
        }

        [Test]
        public void SendReminderAfter91DaysIfNotDormant()
        {
            var lastLogin = DateTime.UtcNow.AddDays(-91);

            var candidateId = Guid.NewGuid();
            var user = new UserBuilder(candidateId).WithLastLogin(lastLogin).WithStatus(UserStatuses.Active).Build();
            var candidate = new CandidateBuilder(candidateId).Build();
            candidate.EnableAllOptionalCommunications();

            var userWriteRepository = new Mock<IUserWriteRepository>();
            var candidateWriteRepository = new Mock<ICandidateWriteRepository>();
            var communicationService = new Mock<ICommunicationService>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyBuilder().With(successor.Object).With(communicationService).With(userWriteRepository).With(candidateWriteRepository).Build();

            Assert(true, true, strategy, successor, userWriteRepository, candidateWriteRepository, communicationService, user, candidate);
        }

        [Test]
        public void DoNotSendReminderBefore330Days()
        {
            var lastLogin = DateTime.UtcNow.AddDays(-329);

            var candidateId = Guid.NewGuid();
            var user = new UserBuilder(candidateId).WithLastLogin(lastLogin).WithStatus(UserStatuses.Dormant).Build();
            var candidate = new CandidateBuilder(candidateId).Build();
            candidate.EnableAllOptionalCommunications();

            var userWriteRepository = new Mock<IUserWriteRepository>();
            var candidateWriteRepository = new Mock<ICandidateWriteRepository>();
            var communicationService = new Mock<ICommunicationService>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyBuilder().With(successor.Object).With(communicationService).With(userWriteRepository).With(candidateWriteRepository).Build();

            Assert(false, false, strategy, successor, userWriteRepository, candidateWriteRepository, communicationService, user, candidate);
        }

        [Test]
        public void SendReminderAfter330Days()
        {
            var lastLogin = DateTime.UtcNow.AddDays(-330);

            var candidateId = Guid.NewGuid();
            var user = new UserBuilder(candidateId).WithLastLogin(lastLogin).WithStatus(UserStatuses.Dormant).Build();
            var candidate = new CandidateBuilder(candidateId).Build();
            candidate.EnableAllOptionalCommunications();

            var userWriteRepository = new Mock<IUserWriteRepository>();
            var candidateWriteRepository = new Mock<ICandidateWriteRepository>();
            var communicationService = new Mock<ICommunicationService>();
            var successor = new Mock<IHousekeepingStrategy>();
            var strategy = new SendAccountRemindersStrategyBuilder().With(successor.Object).With(communicationService).With(userWriteRepository).With(candidateWriteRepository).Build();

            Assert(false, true, strategy, successor, userWriteRepository, candidateWriteRepository, communicationService, user, candidate);
        }

        private static void Assert(bool shouldSetDormant, bool shouldSendReminder, SendAccountRemindersStrategy strategy, Mock<IHousekeepingStrategy> successor, Mock<IUserWriteRepository> userWriteRepository, Mock<ICandidateWriteRepository> candidateWriteRepository, Mock<ICommunicationService> communicationService, User user, Candidate candidate)
        {
            User savedUser = null;
            userWriteRepository.Setup(r => r.Save(It.IsAny<User>())).Callback<User>(u => savedUser = u);
            Candidate savedCandidate = null;
            candidateWriteRepository.Setup(r => r.Save(It.IsAny<Candidate>())).Callback<Candidate>(c => savedCandidate = c);

            CommunicationToken[] communicationTokens = null;
            communicationService.Setup(s => s.SendMessageToCandidate(user.EntityId, MessageTypes.SendDormantAccountReminder, It.IsAny<IEnumerable<CommunicationToken>>()))
                .Callback<Guid, MessageTypes, IEnumerable<CommunicationToken>>((id, mt, ct) =>
                {
                    communicationTokens = ct.ToArray();
                });

            strategy.Handle(user, candidate);

            if (shouldSetDormant)
            {
                //User and Candidate were updated
                userWriteRepository.Verify(r => r.Save(It.IsAny<User>()), Times.Once);
                savedUser.Should().NotBeNull();
                candidateWriteRepository.Verify(r => r.Save(It.IsAny<Candidate>()), Times.Once);
                savedCandidate.Should().NotBeNull();

                //User was set as dormant and comms were disabled
                savedUser.Status.Should().Be(UserStatuses.Dormant);
                CandidateHelper.IndividualCommunicationPreferences(savedCandidate.CommunicationPreferences)
                    .Any(p => p.EnableEmail && p.EnableText)
                    .Should()
                    .BeFalse();
            }
            else
            {
                //User and Candidate were not updated
                userWriteRepository.Verify(r => r.Save(It.IsAny<User>()), Times.Never);
                candidateWriteRepository.Verify(r => r.Save(It.IsAny<Candidate>()), Times.Never);

                savedUser.Should().BeNull();
                savedCandidate.Should().BeNull();
            }

            if (shouldSendReminder)
            {
                //Strategy handled the request
                successor.Verify(s => s.Handle(user, candidate), Times.Never);

                //Message was sent
                communicationService.Verify(s => s.SendMessageToCandidate(candidate.EntityId, MessageTypes.SendDormantAccountReminder, It.IsAny<IEnumerable<CommunicationToken>>()), Times.Once);
                communicationTokens.Should().NotBeNull();

                var lastLogin = user.LastLogin ?? DateTime.UtcNow;
                var lastLoginInDays = (DateTime.UtcNow - lastLogin).Days;
                var lastLoginInDaysFormatted = lastLoginInDays > 270 ? "almost a year" : string.Format("{0} days", lastLoginInDays);

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