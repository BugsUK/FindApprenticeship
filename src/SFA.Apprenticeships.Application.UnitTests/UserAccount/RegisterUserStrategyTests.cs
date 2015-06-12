namespace SFA.Apprenticeships.Application.UnitTests.UserAccount
{
    using System;
    using Builders;
    using Domain.Entities.Exceptions;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class RegisterUserStrategyTests
    {
        [Test]
        public void CannotRegisterExistingActivatedUser()
        {
            const string username = "activated@user.com";
            var userId = Guid.NewGuid();

            var user = new UserBuilder(username, userId).Activated(true).Build();
            
            var userReadRepository = new Mock<IUserReadRepository>();
            userReadRepository.Setup(r => r.Get(username, false)).Returns(user);
            var strategy = new RegisterUserStrategyBuilder().With(userReadRepository).Build();

            Action register = () => strategy.Register(username, userId, "ABC123", UserRoles.Candidate);

            register.ShouldThrow<CustomException>();
        }

        [Test]
        public void RegisteringExistingUnactivatedUserIsValid()
        {
            const string username = "pendingactivation@user.com";
            var userId = Guid.NewGuid();

            var user = new UserBuilder(username, userId).Activated(false).Build();

            var userReadRepository = new Mock<IUserReadRepository>();
            userReadRepository.Setup(r => r.Get(username, false)).Returns(user);
            var strategy = new RegisterUserStrategyBuilder().With(userReadRepository).Build();

            Action register = () => strategy.Register(username, userId, "ABC123", UserRoles.Candidate);

            register.ShouldNotThrow<CustomException>();
        }

        [Test]
        public void RegisteringExistingUnactivatedUserRecreatesUser()
        {
            const string username = "pendingactivation@user.com";
            var userId = Guid.NewGuid();
            const string activationCode = "321CBA";

            var user = new UserBuilder(username, userId).Activated(false).Build();

            var userReadRepository = new Mock<IUserReadRepository>();
            userReadRepository.Setup(r => r.Get(username, false)).Returns(user);
            var userWriteRepository = new Mock<IUserWriteRepository>();
            User savedUser = null;
            userWriteRepository.Setup(r => r.Save(It.IsAny<User>())).Callback<User>(u => savedUser = u);
            var strategy = new RegisterUserStrategyBuilder().With(userReadRepository).With(userWriteRepository).Build();

            strategy.Register(username, userId, activationCode, UserRoles.Candidate);

            savedUser.Should().NotBeNull();
            savedUser.Should().NotBeSameAs(user);
            savedUser.ActivationCode.Should().NotBe(user.ActivationCode);
            savedUser.ActivationCode.Should().Be(activationCode);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void RegisteringExistingUserPendingDeletionIsValid(bool activated)
        {
            const string username = "pendingadeletion@user.com";
            var userId = Guid.NewGuid();

            var user = new UserBuilder(username, userId).Activated(activated).WithStatus(UserStatuses.PendingDeletion).Build();

            var userReadRepository = new Mock<IUserReadRepository>();
            userReadRepository.Setup(r => r.Get(username, false)).Returns(user);
            var strategy = new RegisterUserStrategyBuilder().With(userReadRepository).Build();

            Action register = () => strategy.Register(username, userId, "ABC123", UserRoles.Candidate);

            register.ShouldNotThrow<CustomException>();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void RegisteringExistingUserPendingDeletionRecreatesUser(bool activated)
        {
            const string username = "pendingadeletion@user.com";
            var userId = Guid.NewGuid();
            const string activationCode = "321CBA";

            var user = new UserBuilder(username, userId).Activated(activated).WithStatus(UserStatuses.PendingDeletion).Build();

            var userReadRepository = new Mock<IUserReadRepository>();
            userReadRepository.Setup(r => r.Get(username, false)).Returns(user);
            var userWriteRepository = new Mock<IUserWriteRepository>();
            User savedUser = null;
            userWriteRepository.Setup(r => r.Save(It.IsAny<User>())).Callback<User>(u => savedUser = u);
            var strategy = new RegisterUserStrategyBuilder().With(userReadRepository).With(userWriteRepository).Build();

            strategy.Register(username, userId, activationCode, UserRoles.Candidate);

            savedUser.Should().NotBeNull();
            savedUser.Should().NotBeSameAs(user);
            savedUser.ActivationCode.Should().NotBe(user.ActivationCode);
            savedUser.ActivationCode.Should().Be(activationCode);
        }
    }
}