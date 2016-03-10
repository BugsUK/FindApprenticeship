namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.UnitTests.Provider
{
    using System;
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

            domainProviderSite.Address.AddressLine1.Should().Be(dbProviderSite.AddressLine1);
            domainProviderSite.Address.AddressLine2.Should().Be(dbProviderSite.AddressLine2);
            domainProviderSite.Address.AddressLine3.Should().Be(dbProviderSite.AddressLine3);
            domainProviderSite.Address.AddressLine4.Should().Be(dbProviderSite.AddressLine4);
            domainProviderSite.Address.AddressLine5.Should().Be(dbProviderSite.AddressLine5);

            domainProviderSite.Address.Postcode.Should().Be(dbProviderSite.PostCode);
            domainProviderSite.Address.Town.Should().Be(dbProviderSite.Town);

            domainProviderSite.Address.GeoPoint.Should().NotBeNull();
            domainProviderSite.Address.GeoPoint.Latitude.Should().Be(Convert.ToDouble(dbProviderSite.Latitude));
            domainProviderSite.Address.GeoPoint.Longitude.Should().Be(Convert.ToDouble(dbProviderSite.Longitude));
        }

        [Test]
        public void ShouldMapFromDomainToDatabaseProviderSite()
        {
            // Arrange.
            var domainProviderSite = new Fixture().Create<DomainProviderSite>();

            // Act.
            var dbProviderSite = _mapper.Map<DomainProviderSite, DatabaseProviderSite>(domainProviderSite);

            // Assert.
            dbProviderSite.ProviderSiteId.Should().Be(dbProviderSite.ProviderSiteId);

            dbProviderSite.AddressLine1.Should().Be(domainProviderSite.Address.AddressLine1);
            dbProviderSite.AddressLine2.Should().Be(domainProviderSite.Address.AddressLine2);
            dbProviderSite.AddressLine3.Should().Be(domainProviderSite.Address.AddressLine3);
            dbProviderSite.AddressLine4.Should().Be(domainProviderSite.Address.AddressLine4);
            dbProviderSite.AddressLine5.Should().Be(domainProviderSite.Address.AddressLine5);

            dbProviderSite.PostCode.Should().Be(domainProviderSite.Address.Postcode);
            dbProviderSite.Town.Should().Be(domainProviderSite.Address.Town);

            dbProviderSite.Latitude.Should().Be(Convert.ToDecimal(domainProviderSite.Address.GeoPoint.Latitude));
            dbProviderSite.Longitude.Should().Be(Convert.ToDecimal(domainProviderSite.Address.GeoPoint.Longitude));
        }
    }
}
