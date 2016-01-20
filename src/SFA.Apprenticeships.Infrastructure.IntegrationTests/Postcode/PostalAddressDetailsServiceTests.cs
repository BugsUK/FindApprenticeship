namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.Postcode
{
    using FluentAssertions;
    using Infrastructure.Postcode;
    using NUnit.Framework;

    [TestFixture]
    public class PostalAddressDetailsServiceTests : PostCodeBaseTests
    {
        [Test, Category("Integration")]
        public void ShouldReturnCorrectLocationForPostcode()
        {
            //Arrange
            var service = Container.GetInstance<IPostalAddressDetailsService>();

            //Act
            var location = service.RetrieveValidatedAddress("15499581");

            //Assert
            location.AddressLine1.Should().Be("115 Pemberton Road");
            location.Postcode.Should().Be("N4 1AY");
        }

        [Test, Category("Integration")]
        public void ShouldReturnNullForInvalidId()
        {
            //Arrange
            var service = Container.GetInstance<IPostalAddressDetailsService>();

            //Act
            var location = service.RetrieveValidatedAddress("0wronginvalid99");

            //Assert
            location.Should().BeNull();
        }
    }
}