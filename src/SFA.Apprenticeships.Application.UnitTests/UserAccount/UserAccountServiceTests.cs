namespace SFA.Apprenticeships.Application.UnitTests.UserAccount
{
    using System;
    using Builders;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class UserAccountServiceTests
    {
        [Test]
        public void IsUsernameAvailable_UserNull()
        {
            const string username = "new@user.com";

            var userReadRepository = new Mock<IUserReadRepository>();
            userReadRepository.Setup(r => r.Get(username, false)).Returns((User) null);
            var service = new UserAccountServiceBuilder().With(userReadRepository).Build();

            service.IsUsernameAvailable(username).Should().BeTrue();
        }

        [Test]
        public void IsUsernameAvailable_ExistingActiveUser()
        {
            const string username = "new@user.com";
            var user = new UserBuilder(username, Guid.NewGuid()).Activated(true).Build();

            var userReadRepository = new Mock<IUserReadRepository>();
            userReadRepository.Setup(r => r.Get(username, false)).Returns(user);
            var service = new UserAccountServiceBuilder().With(userReadRepository).Build();

            service.IsUsernameAvailable(username).Should().BeFalse();
        }

        [Test]
        public void IsUsernameAvailable_ExistingUserPendingActivation()
        {
            const string username = "pendingactivation@user.com";
            var user = new UserBuilder(username, Guid.NewGuid()).Activated(false).Build();

            var userReadRepository = new Mock<IUserReadRepository>();
            userReadRepository.Setup(r => r.Get(username, false)).Returns(user);
            var service = new UserAccountServiceBuilder().With(userReadRepository).Build();

            service.IsUsernameAvailable(username).Should().BeTrue();
        }

        [Test]
        public void IsUsernameAvailable_ExistingUserPendingDeletion()
        {
            const string username = "pendingdeletion@user.com";
            var user = new UserBuilder(username, Guid.NewGuid()).WithStatus(UserStatuses.PendingDeletion).Build();

            var userReadRepository = new Mock<IUserReadRepository>();
            userReadRepository.Setup(r => r.Get(username, false)).Returns(user);
            var service = new UserAccountServiceBuilder().With(userReadRepository).Build();

            service.IsUsernameAvailable(username).Should().BeTrue();
        }
    }
}