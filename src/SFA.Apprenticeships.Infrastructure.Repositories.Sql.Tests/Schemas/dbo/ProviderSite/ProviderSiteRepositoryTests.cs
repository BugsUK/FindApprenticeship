namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.dbo.ProviderSite
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

    public class ProviderSiteRepositoryTests
    {
        private readonly IMapper _mapper = new ProviderSiteMappers();
        private readonly Mock<ILogService> _logger = new Mock<ILogService>();
        private IGetOpenConnection _connection;
        private IProviderSiteReadRepository _providerSiteReadRepository;
        private IProviderSiteWriteRepository _providerSiteWriteRepository;

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            _connection = new GetOpenConnectionFromConnectionString(
                DatabaseConfigurationProvider.Instance.TargetConnectionString);

            _providerSiteReadRepository = new ProviderSiteRepository(_connection, _mapper, _logger.Object);
            _providerSiteWriteRepository = new ProviderSiteRepository(_connection, _mapper, _logger.Object);
        }

        [Test]
        public void ShouldGetProviderSite()
        {
            // Act.
            var providerSiteByEdsUrn = _providerSiteReadRepository.GetByEdsUrn("100339794");

            // Assert.
            providerSiteByEdsUrn.Should().NotBeNull();
            providerSiteByEdsUrn.EdsUrn.Should().Be("100339794");

            // Act.
            var providerSiteById = _providerSiteReadRepository.GetById(providerSiteByEdsUrn.ProviderSiteId);

            // Assert.
            providerSiteById.Should().NotBeNull();
            providerSiteById.ShouldBeEquivalentTo(providerSiteByEdsUrn);
        }

        [Test]
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public void ShouldGetProviderSiteByUkprn()
        {
            // Act.
            var providerSitesByUkprn = _providerSiteReadRepository.GetByUkprn("10000000");

            // Assert.
            providerSitesByUkprn.Should().NotBeNull();
            providerSitesByUkprn.Count().Should().BeGreaterThan(0);

            // Act.
            var providerSiteIds = providerSitesByUkprn
                .Select(each => each.ProviderSiteId).ToArray();

            var providerSitesByProviderSiteIds = _providerSiteReadRepository.GetByIds(providerSiteIds);

            // Assert.
            providerSitesByProviderSiteIds.Should().NotBeNull();
            providerSitesByProviderSiteIds.Count().Should().Be(providerSiteIds.Length);
        }

        [Test]
        public void ShouldUpdateProviderSite()
        {
            // Arrange.
            var originalProvideSite = _providerSiteReadRepository.GetByEdsUrn("100339794");

            originalProvideSite.Name = new string(originalProvideSite.Name.Reverse().ToArray());

            // Act.
            var updatedProviderSite = _providerSiteWriteRepository.Update(originalProvideSite);
            var newProviderSite = _providerSiteReadRepository.GetByEdsUrn("100339794");

            // Assert.
            newProviderSite.ShouldBeEquivalentTo(updatedProviderSite);
            newProviderSite.ShouldBeEquivalentTo(originalProvideSite);
        }
    }
}
