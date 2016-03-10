namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.dbo.Provider
{
    using Common;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;
    using Sql.Common;
    using Sql.Schemas.Provider;

    public class ProviderRepositoryTests
    {
        private readonly IMapper _mapper = new ProviderMappers();
        private readonly Mock<ILogService> _logger = new Mock<ILogService>();
        private IGetOpenConnection _connection;

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            _connection = new GetOpenConnectionFromConnectionString(
                DatabaseConfigurationProvider.Instance.TargetConnectionString);
        }

        [Test]
        public void ShouldGetProviderById()
        {
            // Arrange.
            var providerReadRepository = new ProviderRepository(_connection, _mapper, _logger.Object);

            // Act.
            var provider = providerReadRepository.GetByUkprn("10000000");

            // Assert.
            provider.Should().NotBeNull();
        }
    }
}
