namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.UnitTests.Provider
{
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Schemas.Provider;
    using DatabaseProviderSite = Schemas.Provider.Entities.ProviderSite;
    using DomainProviderSite = Domain.Entities.Raa.Parties.ProviderSite;

    [TestFixture]
    public class ProviderSiteMappersTests
    {
        private ProviderSiteMappers _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapper = new ProviderSiteMappers();
            _mapper.Initialise();
        }

        [Test]
        public void ShouldBeValidMapperConfiguration()
        {
            // Assert
            _mapper.Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void ShouldMapFromDatabaseToDomainProviderSite()
        {
            // Arrange.
            var dbProviderSite = new Fixture().Create<DatabaseProviderSite>();

            // Act.
            var domainProviderSite = _mapper.Map<DatabaseProviderSite, DomainProviderSite>(dbProviderSite);

            // Assert.
            domainProviderSite.ProviderSiteId.Should().Be(dbProviderSite.ProviderSiteId);
        }
    }
}
