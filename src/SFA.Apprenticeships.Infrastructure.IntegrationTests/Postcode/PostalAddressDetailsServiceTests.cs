namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.Postcode
{
    using Common.IoC;
    using FluentAssertions;
    using Infrastructure.Postcode;
    using Infrastructure.Postcode.IoC;
    using Logging.IoC;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class PostalAddressDetailsServiceTests
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
            //Arrange
            var service = _container.GetInstance<IPostalAddressDetailsService>();

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
            var service = _container.GetInstance<IPostalAddressDetailsService>();

            //Act
            var location = service.RetrieveValidatedAddress("0wronginvalid99");

            //Assert
            location.Should().BeNull();
        }
    }
}
