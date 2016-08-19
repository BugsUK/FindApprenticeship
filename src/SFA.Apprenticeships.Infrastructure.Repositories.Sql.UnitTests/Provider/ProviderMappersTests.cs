namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.UnitTests.Provider
{
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Schemas.Provider;
    using DatabaseProvider = Schemas.Provider.Entities.Provider;
    using DomainProvider = Domain.Entities.Raa.Parties.Provider;

    [TestFixture]
    [Parallelizable]
    public class ProviderMappersTests
    {
        private ProviderMappers _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapper = new ProviderMappers();
            _mapper.Initialise();
        }

        [Test]
        public void ShouldBeValidMapperConfiguration()
        {
            // Assert
            _mapper.Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void ShouldMapFromDatabaseToDomainProvider()
        {
            // Arrange.
            var dbProvider = new Fixture().Create<DatabaseProvider>();

            // Act.
            var domainProvider = _mapper.Map<DatabaseProvider, DomainProvider>(dbProvider);

            // Assert.
            domainProvider.ProviderId.Should().Be(dbProvider.ProviderId);
            domainProvider.Ukprn.Should().Be(dbProvider.Ukprn.ToString());
            domainProvider.FullName.Should().Be(dbProvider.FullName);
            domainProvider.TradingName.Should().Be(dbProvider.TradingName);
        }

        [Test]
        public void ShouldMapFromDomainToDatabaseProvider()
        {
            // Arrange
            const string ukprn = "123456789";

            var domainProvider = new Fixture()
                .Build<DomainProvider>()
                .With(p => p.Ukprn, ukprn)
                .Create();

            //Act
            var dbProvider = _mapper.Map<DomainProvider, DatabaseProvider>(domainProvider);
            dbProvider.Ukprn.ToString().Should().Be(domainProvider.Ukprn);
            dbProvider.FullName.Should().Be(domainProvider.FullName);
            dbProvider.TradingName.Should().Be(domainProvider.TradingName);
        }

        [TestCase(null, false)]
        [TestCase(0, false)]
        [TestCase(1, false)]
        [TestCase(2, true)]
        public void ShouldMapProviderToUseFaaToIsMigrated(int? providerToUseFaa, bool isMigrated)
        {
            var dbProvider = new Fixture()
                .Build<DatabaseProvider>()
                .With(p => p.ProviderToUseFAA, providerToUseFaa)
                .Create();

            var domainProvider = _mapper.Map<DatabaseProvider, DomainProvider>(dbProvider);

            domainProvider.IsMigrated.Should().Be(isMigrated);
        }
    }
}