namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.GeoCodingProvider
{
    using Application.Interfaces.Employers;
    using Application.Interfaces.Locations;
    using Application.Location;
    using Common.Providers;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Parties;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class GeoCodingProviderTests
    {
        [Test]
        public void GivenAValidAddresShouldUpdateTheEmployer()
        {
            const int employerId = 2;
            var employerService = new Mock<IEmployerService>();
            var geoCodeLookupService = new Mock<IGeoCodeLookupService>();
            var postalAddress = new PostalAddress();
            var geoPoint = new GeoPoint
            {
                Latitude = 54.1,
                Longitude = -0.3
            };

            employerService.Setup(es => es.GetEmployer(employerId)).Returns(new Employer {Address = postalAddress});
            geoCodeLookupService.Setup(gs => gs.GetGeoPointFor(postalAddress)).Returns(geoPoint);

            var geoCodingProvider =
                new GeoCodingProviderBuilder().With(employerService).With(geoCodeLookupService).Build();

            var result = geoCodingProvider.GeoCodeAddress(employerId);

            employerService.Verify(es => es.GetEmployer(employerId));
            geoCodeLookupService.Setup(gs => gs.GetGeoPointFor(postalAddress));
            employerService.Verify(es => es.SaveEmployer(It.Is<Employer>(e => e.Address.GeoPoint == geoPoint)));
            result.Should().Be(GeoCodeAddressResult.Ok);
        }

        [Test]
        public void GivenAnInValidAddresShouldNotUpdateTheEmployer()
        {
            const int employerId = 2;
            var employerService = new Mock<IEmployerService>();
            var geoCodeLookupService = new Mock<IGeoCodeLookupService>();
            var postalAddress = new PostalAddress();
            
            employerService.Setup(es => es.GetEmployer(employerId)).Returns(new Employer { Address = postalAddress });
            geoCodeLookupService.Setup(gs => gs.GetGeoPointFor(postalAddress)).Returns(GeoPoint.Invalid);

            var geoCodingProvider =
                new GeoCodingProviderBuilder().With(employerService).With(geoCodeLookupService).Build();

            var result = geoCodingProvider.GeoCodeAddress(employerId);

            employerService.Verify(es => es.GetEmployer(employerId));
            geoCodeLookupService.Setup(gs => gs.GetGeoPointFor(postalAddress));
            employerService.Verify(es => es.SaveEmployer(It.IsAny<Employer>()), Times.Never());
            result.Should().Be(GeoCodeAddressResult.InvalidAddress);
        }
    }

    public class GeoCodingProviderBuilder
    {
        private Mock<IEmployerService> _employerService = new Mock<IEmployerService>();
        private Mock<IGeoCodeLookupService> _geoCodeLookupService = new Mock<IGeoCodeLookupService>();

        public GeoCodingProviderBuilder With(Mock<IEmployerService> employerService)
        {
            _employerService = employerService;
            return this;
        }

        public GeoCodingProviderBuilder With(Mock<IGeoCodeLookupService> geoCodeLookupService)
        {
            _geoCodeLookupService = geoCodeLookupService;
            return this;
        }

        public GeoCodingProvider Build()
        {
            return new GeoCodingProvider(_employerService.Object, _geoCodeLookupService.Object);
        }
    }
}