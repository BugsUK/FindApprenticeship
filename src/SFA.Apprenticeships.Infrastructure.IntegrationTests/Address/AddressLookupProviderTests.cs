namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.Address
{
    using System.Collections.Generic;
    using Application.Location;
    using Common.IoC;
    using Domain.Entities.Locations;
    using FluentAssertions;
    using Infrastructure.Postcode;
    using Infrastructure.Postcode.IoC;
    using Logging.IoC;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class AddressLookupProviderTests
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
        public void ShouldReturnAListOfPossibleLocations()
        {
            var service = _container.GetInstance<IAddressLookupProvider>();

            IEnumerable<Address> addresses = service.GetPossibleAddresses("SW12 9SX");

            addresses.Should().HaveCount(49);
        }
    }
}