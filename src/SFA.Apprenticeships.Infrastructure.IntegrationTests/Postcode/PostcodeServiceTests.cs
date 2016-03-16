namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.Postcode
{
    using Application.Location;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class PostcodeServiceTests : PostCodeBaseTests
    {
        [Test, Category("Integration")]
        public void ShouldReturnCorrectLocationForPartialPostcode()
        {
            var service = Container.GetInstance<IPostcodeLookupProvider>();

            var location = service.GetLocation("CV1");
            location.GeoPoint.Latitude.Should().Be(52.4087386196133);
            location.GeoPoint.Longitude.Should().Be(-1.50533991358462);
        }

        [Test, Category("Integration")]
        public void ShouldReturnCorrectLocationForPostcode()
        {
            var service = Container.GetInstance<IPostcodeLookupProvider>();

            var location = service.GetLocation("CV1 2WT");
            location.GeoPoint.Latitude.Should().Be(52.4009991288043);
            location.GeoPoint.Longitude.Should().Be(-1.50812239495425);
        }

        [Test, Category("Integration")]
        public void ShouldReturnNullForNonExistentPostcode()
        {
            var service = Container.GetInstance<IPostcodeLookupProvider>();

            var location = service.GetLocation("ZZ1 0ZZ");
            location.Should().BeNull();
        }
    }
}