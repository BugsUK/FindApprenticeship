namespace SFA.Apprenticeships.Application.UnitTests.Services.Location
{
    using Apprenticeships.Application.Location;
    using Domain.Entities.Raa.Locations;
    using Interfaces.Locations;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class GeoCodingServiceTests
    {
        [Test]
        public void GetGeoPointForShouldCallLookupProvider()
        {
            var provider = new Mock<IGeoCodeLookupProvider>();
            var postalAddress = new PostalAddress();
            IGeoCodeLookupService service = new GeoCodeLookupService(provider.Object);
            service.GetGeoPointFor(postalAddress);

            provider.Verify(p => p.GetGeoCodingFor(postalAddress));
        }
    }
}