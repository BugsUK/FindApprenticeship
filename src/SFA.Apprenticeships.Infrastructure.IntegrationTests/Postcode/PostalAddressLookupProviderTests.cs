namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.Postcode
{
    using System.Linq;
    using Application.Location;
    using Common.IoC;
    using FluentAssertions;
    using Infrastructure.Postcode;
    using Infrastructure.Postcode.IoC;
    using Logging.IoC;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class PostalAddressLookupProviderTests
    {
        private Container _container;

        [SetUp]
        public void Setup()
        {
            _container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<PostcodeRegistry>();
            });
        }

        [Test, Category("Integration")]
        public void ShouldReturnCorrectLocationForPostcode()
        {
            var service = _container.GetInstance<IPostalAddressLookupProvider>();

            var location = service.GetPostalAddresses("115 Pemberton Road", "N4 1AY");
            location.Single().AddressLine1.Should().Be("115 Pemberton Road");
            location.Single().Postcode.Should().Be("N4 1AY");
        }

        [Test, Category("Integration")]
        public void ShouldReturnNullForInvalidId()
        {
            var service = _container.GetInstance<IPostalAddressLookupProvider>();

            var location = service.GetPostalAddresses("324sdf", "N4 1AY");
            location.Should().BeNull();
        }
    }
}
