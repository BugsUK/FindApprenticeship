namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.GeoCode
{
    using Application.Location;
    using Common.IoC;
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
            var service = _container.GetInstance<IGeoCodeLookupProvider>();

            var geoPoint = service.GetGeoCodingFor("CV1 2WT");

            geoPoint.Latitude.Should().Be(52.401);
            geoPoint.Longitude.Should().Be(-1.5081);
        }

        [Test, Category("Integration")]
        public void ShouldReturnTheGeoCodingForAValidAddress()
        {
            const string validAddress = "31 Clerkenwell Close,London, EC1R 0AT"; // TODO: check this address
            var service = _container.GetInstance<IGeoCodeLookupProvider>();

            var geoPoint = service.GetGeoCodingFor(validAddress);

            geoPoint.Latitude.Should().Be(52.401); //TODO: check this values
            geoPoint.Longitude.Should().Be(-1.5081); //TODO: check this values
        }

        [Test, Category("Integration")]
        public void ShouldReturnNullForAnInvalidAddressOrPostcode()
        {
            const string validAddress = "SW12 ZZZ";
            var service = _container.GetInstance<IGeoCodeLookupProvider>();

            var geoPoint = service.GetGeoCodingFor(validAddress);

            geoPoint.Should().BeNull();
        }
    }
}