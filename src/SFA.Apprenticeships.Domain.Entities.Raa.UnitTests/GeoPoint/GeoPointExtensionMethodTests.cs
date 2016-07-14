namespace SFA.Apprenticeships.Domain.Entities.Raa.UnitTests.GeoPoint
{
    using FluentAssertions;
    using NUnit.Framework;
    using Locations;

    [TestFixture]
    public class GeoPointExtensionMethodTests
    {
        [Test]
        public void ANullGeoPointIsNotValid()
        {
            GeoPoint geoPoint = null;

            geoPoint.IsValid().Should().BeFalse();
        }

        [Test]
        public void AGeoPointFilledWithZerosShouldBeNotValid()
        {
            var geoPoint = new GeoPoint {Easting = 0, Northing = 0, Latitude = 0.0, Longitude = 0.0};
            
            geoPoint.IsValid().Should().BeFalse();
        }

        [Test]
        public void AnyOtherGeoPointIsValid()
        {
            var geoPoint = new GeoPoint { Easting = 34523, Northing = 452342, Latitude = 52.9238, Longitude = -1.340 };

            geoPoint.IsValid().Should().BeTrue();
        }
         
    }
}