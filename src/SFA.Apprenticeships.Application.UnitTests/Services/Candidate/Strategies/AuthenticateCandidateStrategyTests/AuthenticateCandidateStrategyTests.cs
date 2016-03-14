namespace SFA.Apprenticeships.Application.UnitTests.Services.Candidate.Strategies.AuthenticateCandidateStrategyTests
{
    using System;
    using Apprenticeships.Application.UserAccount.Configuration;
    using Apprenticeships.Application.UserAccount.Strategies;
    using Moq;
    using NUnit.Framework;
    using Apprenticeships.Application.Candidate.Strategies;
    using Domain.Entities.UnitTests.Builder;
    using Interfaces.Users;
    using Domain.Entities.Users;
    using SFA.Infrastructure.Interfaces;
    using Domain.Interfaces.Repositories;

    [TestFixture]
    public class AuthenticateCandidateStrategyTests
    {
        [TestCase]
        public void LoginUser()
        {
            // Arrange
            var userWriteRepository = new Mock<IUserWriteRepository>();

            var authenticateCandidateStrategy =
                new AuthenticateCandidateStrategyBuilder().WithUserAuthenticatingCorrectly(true)
                    .With(userWriteRepository).Build();

            // Act
            authenticateCandidateStrategy.AuthenticateCandidate("username", "password");

            //Assert
            userWriteRepository.Verify(uwr => uwr.Save(It.Is<User>(u => u.LoginIncorrectAttempts == 0 && u.Status == UserStatuses.Active)));
        }

        [TestCase]
        public void UnlockUser()
        {
            // Arrange
            var userWriteRepository = new Mock<IUserWriteRepository>();

            var authenticateCandidateStrategy =
                new AuthenticateCandidateStrategyBuilder().WithUserAuthenticatingCorrectly(true)
                    .With(userWriteRepository).Build();

            // Act
            authenticateCandidateStrategy.AuthenticateCandidate("username", "password");

            //Assert
            userWriteRepository.Verify(uwr => uwr.Save(It.Is<User>(u => u.LoginIncorrectAttempts == 0)));
        }

        [TestCase]
        public void LockUser()
        {
            var lockAccountStrategy = new Mock<ILockAccountStrategy>();

            var authenticateCandidateStrategy =
                new AuthenticateCandidateStrategyBuilder().WithUserAuthenticatingCorrectly(false)
                    .With(lockAccountStrategy).Build();

            // Act
            authenticateCandidateStrategy.AuthenticateCandidate("username", "password");

            //Assert
            lockAccountStrategy.Verify(l => l.LockAccount(It.IsAny<User>()));
        }

        [Test]
        public void DormantUserSetToActiveAfterSuccessfulLogin()
        {
            var user = new UserBuilder(Guid.NewGuid()).WithStatus(UserStatuses.Dormant).Build();

            var userReadRepository = new Mock<IUserReadRepository>();
            userReadRepository.Setup(r => r.Get(It.IsAny<string>(), It.IsAny<bool>())).Returns(user);
            var userWriteRepository = new Mock<IUserWriteRepository>();
            var strategy = new AuthenticateCandidateStrategyBuilder().With(userReadRepository).With(userWriteRepository).Build();

            strategy.AuthenticateCandidate("username", "password");

            userWriteRepository.Verify(uwr => uwr.Save(It.Is<User>(u => u.Status == UserStatuses.Active)));
        }
    }

    public class AuthenticateCandidateStrategyBuilder
    {
        readonly Mock<IConfigurationService> _configService = new Mock<IConfigurationService>();
        readonly Mock<IAuthenticationService> _authenticationService = new Mock<IAuthenticationService>();
        Mock<IUserReadRepository> _userReadRepository = new Mock<IUserReadRepository>();
        readonly Mock<ICandidateReadRepository> _candidateReadRepository = new Mock<ICandidateReadRepository>();
        Mock<IUserWriteRepository> _userWriteRepository = new Mock<IUserWriteRepository>();
        Mock<ILockAccountStrategy> _lockAccountStrategy = new Mock<ILockAccountStrategy>();
        const int MaximumLoginAttemptsAllowed = 3;
        private const int CurrentLoginAttemptsDone = 2;
        private bool _userAutheticatingCorrectly = true;

        public AuthenticateCandidateStrategyBuilder()
        {
            var user = GetAnActiveUserWithSomeLoginIncorrectAttempt(CurrentLoginAttemptsDone);

            _userReadRepository.Setup(urr => urr.Get(It.IsAny<string>(), false))
                .Returns(user);
        }

        public AuthenticateCandidateStrategyBuilder WithUserAuthenticatingCorrectly(bool value)
        {
            _userAutheticatingCorrectly = value;
            return this;
        }

        public AuthenticateCandidateStrategyBuilder With(Mock<IUserWriteRepository> userRepository)
        {
            _userWriteRepository = userRepository;
            return this;
        }

        public AuthenticateCandidateStrategyBuilder With(Mock<ILockAccountStrategy> lockAccountStrategy)
        {
            _lockAccountStrategy = lockAccountStrategy;
            return this;
        }

        public AuthenticateCandidateStrategy Build()
        {
            _configService.Setup(cm => cm.Get<UserAccountConfiguration>())
                .Returns(new UserAccountConfiguration {MaximumPasswordAttemptsAllowed = MaximumLoginAttemptsAllowed});

            _authenticationService.Setup(auth => auth.AuthenticateUser(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(_userAutheticatingCorrectly);

            return new AuthenticateCandidateStrategy(
                _configService.Object, _authenticationService.Object, _userReadRepository.Object,
                _userWriteRepository.Object, _candidateReadRepository.Object, _lockAccountStrategy.Object);
        }

        private static User GetAnActiveUserWithSomeLoginIncorrectAttempt(int incorrectAttempts)
        {
            var user = new User
            {
                LoginIncorrectAttempts = incorrectAttempts,
                Status = UserStatuses.Active,
                EntityId = Guid.NewGuid()
            };
            return user;
        }

        public AuthenticateCandidateStrategyBuilder With(Mock<IUserReadRepository> userReadRepository)
        {
            _userReadRepository = userReadRepository;
            return this;
        }
    }
}
