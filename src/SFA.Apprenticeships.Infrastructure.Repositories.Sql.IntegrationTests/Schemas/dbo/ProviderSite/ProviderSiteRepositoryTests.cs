namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.IntegrationTests.Schemas.dbo.ProviderSite
{
    using System.Linq;
    using Common;
    using Domain.Raa.Interfaces.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Application.Interfaces;
    using Sql.Common;
    using Sql.Schemas.Provider;

    [TestFixture]
    public class ProviderSiteRepositoryTests
    {
        private readonly IMapper _mapper = new ProviderSiteMappers();
        private readonly Mock<ILogService> _logger = new Mock<ILogService>();
        private readonly Mock<IConfigurationService> _configurationService = new Mock<IConfigurationService>();
        private IGetOpenConnection _connection;
        private IProviderSiteReadRepository _providerSiteReadRepository;
        private IProviderSiteWriteRepository _providerSiteWriteRepository;

        [SetUp]
        public void SetUp()
        {
            _connection = new GetOpenConnectionFromConnectionString(
                DatabaseConfigurationProvider.Instance.TargetConnectionString);

            _providerSiteReadRepository = new ProviderSiteRepository(_connection, _mapper, _logger.Object, _configurationService.Object);
            _providerSiteWriteRepository = new ProviderSiteRepository(_connection, _mapper, _logger.Object, _configurationService.Object);
        }

        [Test]
        public void ShouldGetProviderSite()
        {
            // Act.
            var edsUrn = SeedData.ProviderSites.HopwoodCampus.EdsUrn;
            var providerSiteByEdsUrn = _providerSiteReadRepository.GetByEdsUrn(edsUrn);

            // Assert.
            providerSiteByEdsUrn.Should().NotBeNull();
            providerSiteByEdsUrn.EdsUrn.Should().Be(edsUrn);

            // Act.
            var providerSiteById = _providerSiteReadRepository.GetById(providerSiteByEdsUrn.ProviderSiteId);

            // Assert.
            providerSiteById.Should().NotBeNull();
            providerSiteById.ShouldBeEquivalentTo(providerSiteByEdsUrn);
        }

        [Test]
        public void ShouldNotGetProviderSiteWithInvalidEdsUrn()
        {
            // Act.
            var edsUrn = new string(SeedData.ProviderSites.HopwoodCampus.EdsUrn.Reverse().ToArray());
            var providerSiteByEdsUrn = _providerSiteReadRepository.GetByEdsUrn(edsUrn);

            // Assert.
            providerSiteByEdsUrn.Should().BeNull();
        }

        [Test]
        public void ShouldUpdateProviderSite()
        {
            // Arrange.
            var edsUrn = SeedData.ProviderSites.HopwoodCampus.EdsUrn;
            var originalProvideSite = _providerSiteReadRepository.GetByEdsUrn(edsUrn);

            originalProvideSite.FullName = new string(originalProvideSite.FullName.Reverse().ToArray());
            originalProvideSite.ProviderSiteRelationships[0].ProviderSiteFullName = originalProvideSite.FullName;

            // Act.
            var updatedProviderSite = _providerSiteWriteRepository.Update(originalProvideSite);
            var newProviderSite = _providerSiteReadRepository.GetByEdsUrn(edsUrn);

            // Assert.
            newProviderSite.ShouldBeEquivalentTo(updatedProviderSite);
            newProviderSite.ShouldBeEquivalentTo(originalProvideSite);
        }
    }
}
