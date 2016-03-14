namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.dbo.Provider
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Common;
    using Domain.Raa.Interfaces.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;
    using Sql.Common;
    using Sql.Schemas.Provider;

    [TestFixture]
    public class ProviderRepositoryTests
    {
        private readonly IMapper _mapper = new ProviderMappers();
        private readonly Mock<ILogService> _logger = new Mock<ILogService>();
        private IGetOpenConnection _connection;
        private IProviderReadRepository _providerReadRepository;
        private IProviderWriteRepository _providerWriteRepository;

        [SetUp]
        public void SetUp()
        {
            _connection = new GetOpenConnectionFromConnectionString(
                DatabaseConfigurationProvider.Instance.TargetConnectionString);

            _providerReadRepository = new ProviderRepository(_connection, _mapper, _logger.Object);
            _providerWriteRepository = new ProviderRepository(_connection, _mapper, _logger.Object);
        }

        [Test]
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public void ShouldGetProvider()
        {
            // Act.
            var provider = _providerReadRepository.GetByUkprn(SeedData.Providers.HopwoodHallCollege.Ukprn);

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
        public void ShouldUpdateProvider()
        {
            // Arrange.
            var provider = _providerReadRepository.GetByUkprn(SeedData.Providers.HopwoodHallCollege.Ukprn);

            provider.Name = new string(provider.Name.Reverse().ToArray());

            // Act.
            var newProvider = _providerWriteRepository.Update(provider);

            // Assert.
            newProvider.ShouldBeEquivalentTo(provider);

            // Act.
            newProvider = _providerReadRepository.GetById(provider.ProviderId);

            // Assert.
            newProvider.ShouldBeEquivalentTo(provider);
        }
    }
}
