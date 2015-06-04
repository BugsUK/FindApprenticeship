namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.Address
{
    using System.Linq;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class OrdnanceSurveyAddressSearchProviderTests
    {
        [Test]
        public void PostCodeSearchHouseNumber()
        {
            const string postCode = "kt17 1ne";
            var provider = new OrdnanceSurveyAddressSearchProviderBuilder().Build();

            var addresses = provider.FindAddress(postCode).ToList();

            addresses.Count.Should().BeGreaterThan(0);
            addresses.Should().Contain(a => a.AddressLine1 == "30 West Gardens");
            addresses.First().GeoPoint.Should().NotBeNull();
        }

        [Test]
        public void PostCodeSearchShouldBeInAlphabeticOrder()
        {
            const string postCode = "D E 6     5 J A";
            var provider = new OrdnanceSurveyAddressSearchProviderBuilder().Build();

            var addresses = provider.FindAddress(postCode).ToList();

            addresses.Should().BeInAscendingOrder(a => a.AddressLine1);
        }

        [Test]
        public void PostCodeSearchShouldBeInNumericalOrder()
        {
            const string postCode = "ws149UN";
            var provider = new OrdnanceSurveyAddressSearchProviderBuilder().Build();

            var addresses = provider.FindAddress(postCode).ToList();

            addresses.Should().BeInAscendingOrder(a => a.AddressLine1);
        }

        [Test]
        public void PostCodeSearchShouldnotIncludeCommercial()
        {
            const string postCode = "EC2R 7HG";
            var provider = new OrdnanceSurveyAddressSearchProviderBuilder().Build();

            var addresses = provider.FindAddress(postCode).ToList();

            addresses.Should().BeEmpty();
        }

        [Test]
        public void PostCodeSearchFlats()
        {
            const string postCode = "SW2 4NT";
            var provider = new OrdnanceSurveyAddressSearchProviderBuilder().Build();

            var addresses = provider.FindAddress(postCode).ToList();

            addresses.Count.Should().BeGreaterThan(0);
            addresses.Should().Contain(a => a.AddressLine1 == "12A Killieser Avenue");
        }
    }
}