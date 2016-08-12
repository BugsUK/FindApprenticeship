namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Mappers
{
    using SFA.Infrastructure.Interfaces;
    using Common.Mappers;
    using Domain.Entities.Raa.Locations;
    using FluentAssertions;
    using NUnit.Framework;
    using Web.Common.ViewModels.Locations;

    [TestFixture]
    [Parallelizable]
    public class GeoPointToGeoPointViewModelTests
    {
        private IMapper mapper;

        [OneTimeSetUp]
        public void Setup()
        {
            mapper = new RaaCommonWebMappers();
        }

        [TestCase(0D, 0D)]
        [TestCase(0D, 360D)]
        [TestCase(0D, -360D)]
        [TestCase(360D, 0D)]
        [TestCase(-360D, 0D)]
        [TestCase(180D, 180D)]
        [TestCase(-180D, 180D)]
        [TestCase(-180D, -180D)]
        [TestCase(180D, -180D)]
        public void ShouldMapLatLong(double latitude, double longitude)
        {
            //Arrange
            GeoPoint source = new GeoPoint() { Latitude = latitude, Longitude = longitude };
            GeoPointViewModel destination = null;

            //Act
            destination = mapper.Map<GeoPoint, GeoPointViewModel>(source);

            //Assert
            destination.Should().NotBeNull();
            destination.Latitude.Should().Be(latitude);
            destination.Longitude.Should().Be(longitude);
        }
    }
}
