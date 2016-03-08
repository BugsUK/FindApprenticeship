namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.Provider
{
    using Common;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
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
        public void SetUpFixture()
        {
            _connection = new GetOpenConnectionFromConnectionString(DatabaseConfigurationProvider.Instance.TargetConnectionString);

            var logger = new Mock<ILogService>();

            _repository = new ProviderUserRepository(
                _connection, _mapper, logger.Object);
        }

        [Test]
        public void ShouldGetProviderUserById()
        {
            // Arrange.

            // Act.
            var providerUser = _repository.GetById(SeedData.ProviderUser1.ProviderUserId);

            // Assert.            
            providerUser.Should().NotBeNull();
        }

        [Test]
        public void ShouldGetProviderUsername()
        {
            // Arrange.

            // Act.
            var providerUser = _repository.GetByUsername(SeedData.ProviderUser1.Username);

            // Assert.            
            providerUser.Should().NotBeNull();
        }
    }
}
