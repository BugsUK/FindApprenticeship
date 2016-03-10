namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.dbo.ProviderSite
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

    public class ProviderSiteRepositoryTests
    {
        private readonly IMapper _mapper = new ProviderMappers();
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
            var providerSite = _providerSiteReadRepository.GetById(-1);

            // Assert.
            providerSite.Should().NotBeNull();
            providerSite.ProviderSiteId.Should().Be(-1);
        }
    }
}
