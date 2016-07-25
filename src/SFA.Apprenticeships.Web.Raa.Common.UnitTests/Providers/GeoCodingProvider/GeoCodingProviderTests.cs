using SFA.Apprenticeships.Application.Interfaces.Locations;

namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.GeoCodingProvider
{
    using Application.Interfaces.Employers;
    using Application.Interfaces.Locations;
    using Common.Providers;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Parties;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class GeoCodingProviderTests
    {
        [Test]
        public void GivenAnAlreadyGeoCodedAddressShouldNotCallGeoCodingService()
        {
            const int employerId = 2;
            var employerService = new Mock<IEmployerService>();
            var geoCodeLookupService = new Mock<IGeoCodeLookupService>();

            var postalAddress = new Fixture().Create<PostalAddress>();

            employerService.Setup(es => es.GetEmployer(employerId)).Returns(new Employer {Address = postalAddress});

            var geoCodingProvider =
                new GeoCodingProviderBuilder().With(employerService).With(geoCodeLookupService).Build();

            var result = geoCodingProvider.EmployerHasAValidAddress(employerId);

            geoCodeLookupService.Verify(gs => gs.GetGeoPointFor(postalAddress), Times.Never);
            result.Should().Be(GeoCodeAddressResult.Ok);
        }

        [Test]
        public void GivenANonGeoCodedAddressShouldCallGeoCodingService()
        {
            const int employerId = 2;
            var employerService = new Mock<IEmployerService>();
            var geoCodeLookupService = new Mock<IGeoCodeLookupService>();
            var postalAddress = new PostalAddress {County = "something"};
            geoCodeLookupService.Setup(gs => gs.GetGeoPointFor(postalAddress)).Returns(GeoPoint.NotSet);

            employerService.Setup(es => es.GetEmployer(employerId)).Returns(new Employer { Address = postalAddress });

            var geoCodingProvider =
                new GeoCodingProviderBuilder().With(employerService).With(geoCodeLookupService).Build();

            geoCodingProvider.EmployerHasAValidAddress(employerId);

            employerService.Verify(es => es.GetEmployer(employerId));
            geoCodeLookupService.Verify(gs => gs.GetGeoPointFor(postalAddress), Times.Once());
        }

        [Test]
        public void GivenAValidAddressShouldReturnOk()
        {
            const int employerId = 2;
            var employerService = new Mock<IEmployerService>();
            var geoCodeLookupService = new Mock<IGeoCodeLookupService>();
            var postalAddress = new PostalAddress {County = "something"};

            employerService.Setup(es => es.GetEmployer(employerId)).Returns(new Employer { Address = postalAddress });
            geoCodeLookupService.Setup(gs => gs.GetGeoPointFor(postalAddress)).Returns(new Fixture().Create<GeoPoint>());

            var geoCodingProvider =
                new GeoCodingProviderBuilder().With(employerService).With(geoCodeLookupService).Build();

            var result = geoCodingProvider.EmployerHasAValidAddress(employerId);

            result.Should().Be(GeoCodeAddressResult.Ok);
        }

        [Test]
        public void GivenAInvalidAddressShouldReturnInvalidAddress()
        {
            const int employerId = 2;
            var employerService = new Mock<IEmployerService>();
            var geoCodeLookupService = new Mock<IGeoCodeLookupService>();
            var postalAddress = new PostalAddress();

            employerService.Setup(es => es.GetEmployer(employerId)).Returns(new Employer { Address = postalAddress });
            geoCodeLookupService.Setup(gs => gs.GetGeoPointFor(postalAddress)).Returns(GeoPoint.NotSet);

            var geoCodingProvider =
                new GeoCodingProviderBuilder().With(employerService).With(geoCodeLookupService).Build();

            var result = geoCodingProvider.EmployerHasAValidAddress(employerId);

            result.Should().Be(GeoCodeAddressResult.InvalidAddress);
        }

        [Test]
        public void GivenAInvalidCountyShouldReturnInvalidAddress()
        {
            const int employerId = 2;
            var employerService = new Mock<IEmployerService>();
            var geoCodeLookupService = new Mock<IGeoCodeLookupService>();
            var postalAddress = new PostalAddress { County = "" };

            employerService.Setup(es => es.GetEmployer(employerId)).Returns(new Employer { Address = postalAddress });
            geoCodeLookupService.Setup(gs => gs.GetGeoPointFor(postalAddress)).Returns(new Fixture().Create<GeoPoint>());

            var geoCodingProvider =
                new GeoCodingProviderBuilder().With(employerService).With(geoCodeLookupService).Build();

            var result = geoCodingProvider.EmployerHasAValidAddress(employerId);

            result.Should().Be(GeoCodeAddressResult.Ok);
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