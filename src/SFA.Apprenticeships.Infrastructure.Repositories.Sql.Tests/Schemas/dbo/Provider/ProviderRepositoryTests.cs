namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.dbo.Provider
{
    using System.Linq;
    using Common;
    using Domain.Raa.Interfaces.Repositories;
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
        private IProviderReadRepository _providerReadRepository;
        private IProviderWriteRepository _providerWriteRepository;

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            _connection = new GetOpenConnectionFromConnectionString(
                DatabaseConfigurationProvider.Instance.TargetConnectionString);

            _providerReadRepository = new ProviderRepository(_connection, _mapper, _logger.Object);
            _providerWriteRepository = new ProviderRepository(_connection, _mapper, _logger.Object);
        }

        [Test]
        public void ShouldGetProvider()
        {
            // Act.
            var provider = _providerReadRepository.GetByUkprn("10000000");

            // Assert.
            provider.Should().NotBeNull();

            // Act
            provider = _providerReadRepository.GetById(provider.ProviderId);

            // Assert.
            provider.Should().NotBeNull();

            // Act
            var providers = _providerReadRepository.GetByIds(new[] {provider.ProviderId});
            providers.Should().HaveCount(1);
            providers.First().ShouldBeEquivalentTo(provider);
        }

        [Test]
        public void ShouldUpdateTheProvider()
        {
            // Arrange
            var newName = "A new name for the provider";
            var provider = _providerReadRepository.GetByUkprn("10000000");

            provider.Name = newName;

            var newProvider = _providerWriteRepository.Update(provider);
            newProvider.ShouldBeEquivalentTo(provider);

            newProvider = _providerReadRepository.GetById(provider.ProviderId);
            newProvider.ShouldBeEquivalentTo(provider);
        }
    }
}
