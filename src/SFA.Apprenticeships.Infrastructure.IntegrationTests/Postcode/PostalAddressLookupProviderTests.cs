namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.Postcode
{
    using System.Linq;
    using Application.Location;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class PostalAddressLookupProviderTests : PostCodeBaseTests
    {
        [Test, Category("Integration")]
        public void ShouldReturnCorrectLocationForPostcode()
        {
            var service = Container.GetInstance<IPostalAddressLookupProvider>();

            var location = service.GetValidatedPostalAddresses("115 Pemberton Road", "N4 1AY");
            location.Single().AddressLine1.Should().Be("115 Pemberton Road");
            location.Single().Postcode.Should().Be("N4 1AY");
        }

        [Test, Category("Integration")]
        public void ShouldReturnNullForInvalidId()
        {
            var service = Container.GetInstance<IPostalAddressLookupProvider>();

            var location = service.GetValidatedPostalAddresses("324sdf", "N4 1AY");
            location.Should().BeNull();
        }
    }
}