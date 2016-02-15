namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.Provider
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Domain.Entities.Users;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;
    using Sql.Common;
    using Sql.Schemas.Provider;
    using ProviderUser = Sql.Schemas.Provider.Entities.ProviderUser;

    [TestFixture(Category = "Integration")]
    public class ProviderUserRepositoryTests
    {
        private IGetOpenConnection _connection;
        private readonly IMapper _mapper = new ProviderUserMappers();

        private ProviderUser _providerUser;

        private ProviderUserRepository _repository;

        [SetUp]
        public void SetUpFixture()
        {
            var dbInitialiser = new DatabaseInitialiser();

            _providerUser = new ProviderUser()
            {
                ProviderUserId = 1,
                ProviderUserGuid = Guid.NewGuid(),
                ProviderUserStatusId = (int)ProviderUserStatuses.Registered,
                ProviderId = 1,
                Username = "jane.doe",
                Fullname = "Jane Doe",
                PreferredSiteErn = 90392821,
                Email = "jane.doe@example.com",
                EmailVerificationCode = "ABC123",
                EmailVerifiedDateTime = DateTime.UtcNow,
                PhoneNumber = "07999555123",
                DateCreated = DateTime.UtcNow.AddDays(-1),
                DateUpdated = DateTime.UtcNow.AddHours(-10)
            };

            dbInitialiser.Publish(false);

            dbInitialiser.Seed(new List<object>()
            {
                _providerUser
            });

            _connection = dbInitialiser.GetOpenConnection();

            var logger = new Mock<ILogService>();

            _repository = new ProviderUserRepository(
                _connection, _mapper, logger.Object);
        }

        [Test]
        [Ignore]
        public void ShouldGetProviderUserById()
        {
            // Arrange.
            
            // Act.
            var providerUser = _repository.Get(_providerUser.ProviderUserId);

            // Assert.            
            providerUser.Should().NotBeNull();
        }

        [Test]
        [Ignore]
        public void ShouldGetProviderUsername()
        {
            // Arrange.

            // Act.
            var providerUser = _repository.Get(_providerUser.Username);

            // Assert.            
            providerUser.Should().NotBeNull();
        }
    }
}
