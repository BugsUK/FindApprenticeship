namespace SFA.Apprenticeships.Application.UnitTests.Candidate.Strategies.RegisterCandidateStrategy
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Interfaces.Communications;
    using Interfaces.Users;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class RegisterCandidateStrategyTests
    {
        [Test]
        public void NewUserRegistration()
        {
            const string emailAddress = "new@user.com";
            const string password = "Password01";

            var candidate = new CandidateBuilder(Guid.Empty).EmailAddress(emailAddress).Build();

            var strategy = new RegisterCandidateStrategyBuilder().Build();

            strategy.RegisterCandidate(candidate, password);
        }

        [Test]
        public void NewUserRegistrationCreatesAuthenticationEntity()
        {
            const string emailAddress = "new@user.com";
            const string password = "Password01";

            var candidate = new CandidateBuilder(Guid.Empty).EmailAddress(emailAddress).Build();

            var authenticationService = new Mock<IAuthenticationService>();
            var strategy = new RegisterCandidateStrategyBuilder().With(authenticationService).Build();

            strategy.RegisterCandidate(candidate, password);

            authenticationService.Verify(s => s.CreateUser(It.Is<Guid>(g => g != Guid.Empty), password), Times.Once);
        }

        [Test]
        public void NewUserRegistrationRegistersUserAccount()
        {
            const string emailAddress = "new@user.com";
            const string password = "Password01";

            var candidate = new CandidateBuilder(Guid.Empty).EmailAddress(emailAddress).Build();

            var userAccountService = new Mock<IUserAccountService>();
            var strategy = new RegisterCandidateStrategyBuilder().With(userAccountService).Build();

            strategy.RegisterCandidate(candidate, password);

            userAccountService.Verify(s => s.Register(emailAddress, It.Is<Guid>(g => g != Guid.Empty), It.IsAny<string>(), UserRoles.Candidate), Times.Once);
        }

        [Test]
        public void NewUserRegistrationCreatesNewGuid()
        {
            const string emailAddress = "new@user.com";
            const string password = "Password01";

            var candidate = new CandidateBuilder(Guid.Empty).EmailAddress(emailAddress).Build();

            var candidateWriteRepository = new Mock<ICandidateWriteRepository>();
            Candidate savedCandidate = null;
            candidateWriteRepository.Setup(r => r.Save(It.IsAny<Candidate>())).Returns<Candidate>(c => c).Callback<Candidate>(c => savedCandidate = c);
            var strategy = new RegisterCandidateStrategyBuilder().With(candidateWriteRepository).Build();

            strategy.RegisterCandidate(candidate, password);

            savedCandidate.Should().NotBeNull();
            savedCandidate.EntityId.Should().NotBeEmpty();
        }

        [Test]
        public void NewUserRegistrationSendsActivationCode()
        {
            const string emailAddress = "new@user.com";
            const string password = "Password01";

            var candidate = new CandidateBuilder(Guid.Empty).EmailAddress(emailAddress).Build();

            var communicationService = new Mock<ICommunicationService>();
            var strategy = new RegisterCandidateStrategyBuilder().With(communicationService).Build();

            strategy.RegisterCandidate(candidate, password);

            communicationService.Verify(s => s.SendMessageToCandidate(It.Is<Guid>(g => g != Guid.Empty), MessageTypes.SendActivationCode, It.IsAny<CommunicationToken[]>()), Times.Once);
        }

        [Test]
        public void ExistingUserMustNotBeActive()
        {
            const string emailAddress = "active@user.com";
            const string password = "Password01";
            var candidateId = Guid.NewGuid();

            var candidate = new CandidateBuilder(candidateId).EmailAddress(emailAddress).Build();
            var user = new UserBuilder(emailAddress, candidateId).Activated(true).Build();

            var userReadRepository = new Mock<IUserReadRepository>();
            userReadRepository.Setup(r => r.Get(emailAddress, false)).Returns(user);
            var strategy = new RegisterCandidateStrategyBuilder().With(userReadRepository).Build();

            Action action = () => strategy.RegisterCandidate(candidate, password);

            action.ShouldThrow<CustomException>();
        }

        [Test]
        public void RegisteringExistingUnactivatedUserIsValid()
        {
            const string emailAddress = "pendingactivation@user.com";
            const string password = "Password01";
            var candidateId = Guid.NewGuid();

            var candidate = new CandidateBuilder(candidateId).EmailAddress(emailAddress).Build();
            var user = new UserBuilder(emailAddress, candidateId).Activated(false).Build();

            var userReadRepository = new Mock<IUserReadRepository>();
            userReadRepository.Setup(r => r.Get(emailAddress, false)).Returns(user);
            var strategy = new RegisterCandidateStrategyBuilder().With(userReadRepository).Build();

            Action action = () => strategy.RegisterCandidate(candidate, password);

            action.ShouldNotThrow<CustomException>();
        }

        [Test]
        public void RegisteringExistingUnactivatedUserSendsActivationCode()
        {
            const string emailAddress = "pendingactivation@user.com";
            const string password = "Password01";
            var candidateId = Guid.NewGuid();

            var candidate = new CandidateBuilder(candidateId).EmailAddress(emailAddress).Build();
            var user = new UserBuilder(emailAddress, candidateId).Activated(false).Build();

            var userReadRepository = new Mock<IUserReadRepository>();
            userReadRepository.Setup(r => r.Get(emailAddress, false)).Returns(user);
            var communicationService = new Mock<ICommunicationService>();
            var strategy = new RegisterCandidateStrategyBuilder().With(userReadRepository).With(communicationService).Build();

            strategy.RegisterCandidate(candidate, password);

            communicationService.Verify(s => s.SendMessageToCandidate(candidateId, MessageTypes.SendActivationCode, It.IsAny<CommunicationToken[]>()), Times.Once);
        }

        [Test]
        public void RegisteringExistingUnactivatedUserDoesntResetPassword()
        {
            const string emailAddress = "pendingactivation@user.com";
            const string password = "Password01";
            var candidateId = Guid.NewGuid();

            var candidate = new CandidateBuilder(candidateId).EmailAddress(emailAddress).Build();
            var user = new UserBuilder(emailAddress, candidateId).Activated(false).Build();

            var userReadRepository = new Mock<IUserReadRepository>();
            userReadRepository.Setup(r => r.Get(emailAddress, false)).Returns(user);
            var authenticationService = new Mock<IAuthenticationService>();
            var strategy = new RegisterCandidateStrategyBuilder().With(userReadRepository).With(authenticationService).Build();

            strategy.RegisterCandidate(candidate, password);

            authenticationService.Verify(s => s.ResetUserPassword(candidateId, password), Times.Never);
        }

        [Test]
        public void RegisteringExistingUnactivatedUserDoesntReRegisterUser()
        {
            const string emailAddress = "pendingactivation@user.com";
            const string password = "Password01";
            var candidateId = Guid.NewGuid();

            var candidate = new CandidateBuilder(candidateId).EmailAddress(emailAddress).Build();
            var user = new UserBuilder(emailAddress, candidateId).Activated(false).Build();

            var userReadRepository = new Mock<IUserReadRepository>();
            userReadRepository.Setup(r => r.Get(emailAddress, false)).Returns(user);
            var userAccountService = new Mock<IUserAccountService>();
            var strategy = new RegisterCandidateStrategyBuilder().With(userReadRepository).With(userAccountService).Build();

            strategy.RegisterCandidate(candidate, password);

            userAccountService.Verify(s => s.Register(emailAddress, candidateId, It.IsAny<string>(), UserRoles.Candidate), Times.Never);
        }

        [Test]
        public void RegisteringExistingUnactivatedExpiredUserIsValid()
        {
            const string emailAddress = "pendingactivation@user.com";
            const string password = "Password01";
            var candidateId = Guid.NewGuid();

            var candidate = new CandidateBuilder(candidateId).EmailAddress(emailAddress).Build();
            var user = new UserBuilder(emailAddress, candidateId).Activated(false).Build();
            user.ActivateCodeExpiry = DateTime.UtcNow.AddMonths(-2);

            var userReadRepository = new Mock<IUserReadRepository>();
            userReadRepository.Setup(r => r.Get(emailAddress, false)).Returns(user);
            var strategy = new RegisterCandidateStrategyBuilder().With(userReadRepository).Build();

            Action action = () => strategy.RegisterCandidate(candidate, password);

            action.ShouldNotThrow<CustomException>();
        }

        [Test]
        public void RegisteringExistingUnactivatedExpiredUserDoesntResetPassword()
        {
            const string emailAddress = "pendingactivation@user.com";
            const string password = "Password01";
            var candidateId = Guid.NewGuid();

            var candidate = new CandidateBuilder(candidateId).EmailAddress(emailAddress).Build();
            var user = new UserBuilder(emailAddress, candidateId).Activated(false).Build();
            user.ActivateCodeExpiry = DateTime.UtcNow.AddMonths(-2);

            var userReadRepository = new Mock<IUserReadRepository>();
            userReadRepository.Setup(r => r.Get(emailAddress, false)).Returns(user);
            var authenticationService = new Mock<IAuthenticationService>();
            var strategy = new RegisterCandidateStrategyBuilder().With(userReadRepository).With(authenticationService).Build();

            strategy.RegisterCandidate(candidate, password);

            authenticationService.Verify(s => s.ResetUserPassword(candidateId, password), Times.Once);
        }

        [Test]
        public void RegisteringExistingUnactivatedExpiredUserDoesntReRegisterUser()
        {
            const string emailAddress = "pendingactivation@user.com";
            const string password = "Password01";
            var candidateId = Guid.NewGuid();

            var candidate = new CandidateBuilder(candidateId).EmailAddress(emailAddress).Build();
            var user = new UserBuilder(emailAddress, candidateId).Activated(false).Build();
            user.ActivateCodeExpiry = DateTime.UtcNow.AddMonths(-2);

            var userReadRepository = new Mock<IUserReadRepository>();
            userReadRepository.Setup(r => r.Get(emailAddress, false)).Returns(user);
            var userAccountService = new Mock<IUserAccountService>();
            var strategy = new RegisterCandidateStrategyBuilder().With(userReadRepository).With(userAccountService).Build();

            strategy.RegisterCandidate(candidate, password);

            userAccountService.Verify(s => s.Register(emailAddress, candidateId, It.IsAny<string>(), UserRoles.Candidate), Times.Once);
        }

        [Test]
        public void RegisteringExistingUnactivatedExpiredUserSendsActivationCode()
        {
            const string emailAddress = "pendingactivation@user.com";
            const string password = "Password01";
            var candidateId = Guid.NewGuid();

            var candidate = new CandidateBuilder(candidateId).EmailAddress(emailAddress).Build();
            var user = new UserBuilder(emailAddress, candidateId).Activated(false).Build();
            user.ActivateCodeExpiry = DateTime.UtcNow.AddMonths(-2);

            var userReadRepository = new Mock<IUserReadRepository>();
            userReadRepository.Setup(r => r.Get(emailAddress, false)).Returns(user);
            var communicationService = new Mock<ICommunicationService>();
            var strategy = new RegisterCandidateStrategyBuilder().With(userReadRepository).With(communicationService).Build();

            strategy.RegisterCandidate(candidate, password);

            communicationService.Verify(s => s.SendMessageToCandidate(candidateId, MessageTypes.SendActivationCode, It.IsAny<CommunicationToken[]>()), Times.Once);
        }

        [Test]
        public void RegisteringUserPendingDeletionIsValid()
        {
            const string emailAddress = "pendingdeletion@user.com";
            const string password = "Password01";
            var candidateId = Guid.NewGuid();

            var candidate = new CandidateBuilder(candidateId).EmailAddress(emailAddress).Build();
            var user = new UserBuilder(emailAddress, candidateId).WithStatus(UserStatuses.PendingDeletion).Build();

            var userReadRepository = new Mock<IUserReadRepository>();
            userReadRepository.Setup(r => r.Get(emailAddress, false)).Returns(user);
            var strategy = new RegisterCandidateStrategyBuilder().With(userReadRepository).With(userReadRepository).Build();

            Action action = () => strategy.RegisterCandidate(candidate, password);

            action.ShouldNotThrow<CustomException>();
        }

        [Test]
        public void RegisteringUserPendingDeletionCreatesAuthenticationEntity()
        {
            const string emailAddress = "pendingdeletion@user.com";
            const string password = "Password01";
            var candidateId = Guid.NewGuid();

            var candidate = new CandidateBuilder(candidateId).EmailAddress(emailAddress).Build();
            var user = new UserBuilder(emailAddress, candidateId).WithStatus(UserStatuses.PendingDeletion).Build();

            var userReadRepository = new Mock<IUserReadRepository>();
            userReadRepository.Setup(r => r.Get(emailAddress, false)).Returns(user);
            var authenticationService = new Mock<IAuthenticationService>();
            var strategy = new RegisterCandidateStrategyBuilder().With(userReadRepository).With(authenticationService).Build();

            strategy.RegisterCandidate(candidate, password);

            authenticationService.Verify(s => s.CreateUser(It.Is<Guid>(g => g != Guid.Empty), password), Times.Once);
        }

        [Test]
        public void RegisteringUserPendingDeletionRegistersUserAccount()
        {
            const string emailAddress = "pendingdeletion@user.com";
            const string password = "Password01";
            var candidateId = Guid.NewGuid();

            var candidate = new CandidateBuilder(candidateId).EmailAddress(emailAddress).Build();
            var user = new UserBuilder(emailAddress, candidateId).WithStatus(UserStatuses.PendingDeletion).Build();

            var userReadRepository = new Mock<IUserReadRepository>();
            userReadRepository.Setup(r => r.Get(emailAddress, false)).Returns(user);
            var userAccountService = new Mock<IUserAccountService>();
            var strategy = new RegisterCandidateStrategyBuilder().With(userReadRepository).With(userAccountService).Build();

            strategy.RegisterCandidate(candidate, password);

            userAccountService.Verify(s => s.Register(emailAddress, It.Is<Guid>(g => g != Guid.Empty), It.IsAny<string>(), UserRoles.Candidate), Times.Once);
        }

        [Test]
        public void RegisteringUserPendingDeletionCreatesNewGuid()
        {
            const string emailAddress = "pendingdeletion@user.com";
            const string password = "Password01";
            var candidateId = Guid.NewGuid();

            var candidate = new CandidateBuilder(candidateId).EmailAddress(emailAddress).Build();
            var user = new UserBuilder(emailAddress, candidateId).WithStatus(UserStatuses.PendingDeletion).Build();

            var userReadRepository = new Mock<IUserReadRepository>();
            userReadRepository.Setup(r => r.Get(emailAddress, false)).Returns(user);
            var candidateWriteRepository = new Mock<ICandidateWriteRepository>();
            Candidate savedCandidate = null;
            candidateWriteRepository.Setup(r => r.Save(It.IsAny<Candidate>())).Returns<Candidate>(c => c).Callback<Candidate>(c => savedCandidate = c);
            var strategy = new RegisterCandidateStrategyBuilder().With(userReadRepository).With(candidateWriteRepository).Build();

            strategy.RegisterCandidate(candidate, password);

            savedCandidate.Should().NotBeNull();
            savedCandidate.EntityId.Should().NotBeEmpty();
        }

        [Test]
        public void RegisteringUserPendingDeletionSendsActivationCode()
        {
            const string emailAddress = "pendingdeletion@user.com";
            const string password = "Password01";
            var candidateId = Guid.NewGuid();

            var candidate = new CandidateBuilder(candidateId).EmailAddress(emailAddress).Build();
            var user = new UserBuilder(emailAddress, candidateId).WithStatus(UserStatuses.PendingDeletion).Build();

            var userReadRepository = new Mock<IUserReadRepository>();
            userReadRepository.Setup(r => r.Get(emailAddress, false)).Returns(user);
            var communicationService = new Mock<ICommunicationService>();
            var strategy = new RegisterCandidateStrategyBuilder().With(userReadRepository).With(communicationService).Build();

            strategy.RegisterCandidate(candidate, password);

            communicationService.Verify(s => s.SendMessageToCandidate(It.Is<Guid>(g => g != Guid.Empty), MessageTypes.SendActivationCode, It.IsAny<CommunicationToken[]>()), Times.Once);
        }
    }
}