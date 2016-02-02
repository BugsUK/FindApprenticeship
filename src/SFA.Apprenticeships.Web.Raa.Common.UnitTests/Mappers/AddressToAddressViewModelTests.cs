namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Mappers
{
    using SFA.Infrastructure.Interfaces;
    using Common.Mappers;
    using Domain.Entities.Locations;
    using FluentAssertions;
    using NUnit.Framework;
    using Web.Common.ViewModels.Locations;

    [TestFixture]
    public class AddressToAddressViewModelTests
    {
        private IMapper mapper;

        [TestFixtureSetUp]
        public void Setup()
        {
            mapper = new RaaCommonWebMappers();
        }

        [Test]
        public void ShouldCreateMap()
        {
            new RaaCommonWebMappers().Mapper.AssertConfigurationIsValid();
        }

        [TestCase("add 1", "add 2", "add 3", "add 4", "add 5", "town", "uprn", "postcode")]
        [TestCase(null, "add 2", "add 3", "add 4", "add 5", "town", "uprn", "postcode")]
        [TestCase("add 1", null, "add 3", "add 4", "add 5", "town", "uprn", "postcode")]
        [TestCase("add 1", "add 2", null, "add 4", "add 5", "town", "uprn", "postcode")]
        [TestCase("add 1", "add 2", "add 3", null, "add 5", "town", "uprn", "postcode")]
        [TestCase("add 1", "add 2", "add 3", "add 4", null, "town", "uprn", "postcode")]
        [TestCase("add 1", "add 2", "add 3", "add 4", "add 5", null, "uprn", "postcode")]
        [TestCase("add 1", "add 2", "add 3", "add 4", "add 5", "town", null, "postcode")]
        [TestCase("add 1", "add 2", "add 3", "add 4", "add 5", "town", "uprn", null)]
        public void ShouldMapStringPropertiesAndUpperCasePostcode(string add1, string add2, string add3, string add4, string add5, string town, string uprn, string postcode)
        {
            //Arrange
            var source = new PostalAddress
            {
                AddressLine1 = add1,
                AddressLine2 = add2,
                AddressLine3 = add3,
                AddressLine4 = add4,
                AddressLine5 = add5,
                Town = town,
                ValidationSourceKeyValue = uprn,
                Postcode = postcode
            };
            AddressViewModel destination = null;

            //Act
            destination = mapper.Map<PostalAddress, AddressViewModel>(source);

            //Assert
            destination.Should().NotBeNull();
            destination.AddressLine1.Should().Be(add1);
            destination.AddressLine2.Should().Be(add2);
            destination.AddressLine3.Should().Be(add3);
            destination.AddressLine4.Should().Be(add4);
            destination.AddressLine5.Should().Be(add5);
            destination.Town.Should().Be(town);
            destination.Uprn.Should().Be(uprn);
            destination.Postcode.Should().Be(postcode?.ToUpperInvariant());
        }

        [Test]
        public void ShouldMapGeoPoint()
        {
            //Arrange
            var source = new PostalAddress
            {
                GeoPoint = new GeoPoint() { Latitude = 0D, Longitude = 360D }
            };

            AddressViewModel destination = null;

            //Act
            destination = mapper.Map<PostalAddress, AddressViewModel>(source);

            //Assert
            destination.Should().NotBeNull();
            destination.GeoPoint.Should().NotBeNull();
            destination.GeoPoint.Latitude.Should().Be(source.GeoPoint.Latitude);
            destination.GeoPoint.Longitude.Should().Be(source.GeoPoint.Longitude);
        }
    }
}
