namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.IntegrationTests.Schemas.Provider
{
    using System.Linq;
    using Common;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;
    using Sql.Common;
    using Sql.Schemas.Provider;

    [TestFixture(Category = "Integration")]
    public class ProviderUserRepositoryTests
    {
        private IGetOpenConnection _connection;
        private readonly IMapper _mapper = new ProviderUserMappers();

        private ProviderUserRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _connection = new GetOpenConnectionFromConnectionString(
                DatabaseConfigurationProvider.Instance.TargetConnectionString);

            var logger = new Mock<ILogService>();

            _repository = new ProviderUserRepository(
                _connection, _mapper, logger.Object);
        }

        [Test]
        public void ShouldGetProviderUser()
        {
            // Act.
            var providerUserByUsername = _repository.GetByUsername(SeedData.ProviderUsers.JaneDoe.Username);

            // Assert.            
            providerUserByUsername.Should().NotBeNull();
            providerUserByUsername.Username.Should().Be(SeedData.ProviderUsers.JaneDoe.Username);

            // Act.
            var providerUserById = _repository.GetById(providerUserByUsername.ProviderUserId);

            // Assert.            
            providerUserById.Should().NotBeNull();
            providerUserById.ProviderUserId.Should().Be(providerUserByUsername.ProviderUserId);
        }

        [Test]
        public void ShouldNotGetProviderUserWithInvalidUsername()
        {
            // Act.
            var username = new string(SeedData.ProviderUsers.JaneDoe.Username.Reverse().ToArray());
            var providerUser = _repository.GetByUsername(username);

            // Assert.            
            providerUser.Should().BeNull();
        }
    }
}
