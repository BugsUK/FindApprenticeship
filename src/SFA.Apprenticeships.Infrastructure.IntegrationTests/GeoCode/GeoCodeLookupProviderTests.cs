namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.GeoCode
{
    using Application.Location;
    using Common.IoC;
    using Domain.Entities.Raa.Locations;
    using FluentAssertions;
    using Infrastructure.Postcode.IoC;
    using Logging.IoC;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class GeoCodeLookupProviderTests
    {
        private Container _container;

        [SetUp]
        public void SetUp()
        {
            _container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<PostcodeRegistry>();
            });
        }

        [Test, Category("Integration")]
        public void ShouldReturnTheGeoCodingForAValidPostCode()
        {
            var provider = _container.GetInstance<IGeoCodeLookupProvider>();
            var postalAddress = new PostalAddress
            {
                Postcode = "CV1 2WT"
            };
            var geoPoint = provider.GetGeoCodingFor(postalAddress);

            geoPoint.Latitude.Should().Be(52.401);
            geoPoint.Longitude.Should().Be(-1.5081);
        }

        [Test, Category("Integration")]
        public void ShouldReturnTheGeoCodingForAValidAddress()
        {
            var provider = _container.GetInstance<IGeoCodeLookupProvider>();
            var postalAddres = new PostalAddress // TODO: check this Address
            {
                AddressLine1 = "Clerkenwell Close",
                AddressLine3 = "London"
            };
            var geoPoint = provider.GetGeoCodingFor(postalAddres); 

            geoPoint.Latitude.Should().Be(52.401); //TODO: check this values
            geoPoint.Longitude.Should().Be(-1.5081); //TODO: check this values
        }

        [Test, Category("Integration")]
        public void ShouldReturnNotSetForAnInvalidAddressOrPostcode()
        {
            var postalAddress = new PostalAddress
            {
                Postcode = "SW12 ZZZ"
            };
            var provider = _container.GetInstance<IGeoCodeLookupProvider>();

            var geoPoint = provider.GetGeoCodingFor(postalAddress);

            geoPoint.Should().Be(GeoPoint.NotSet);
        }
    }
}