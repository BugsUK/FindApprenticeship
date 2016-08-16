namespace SFA.Apprenticeships.Application.UnitTests.Services.Location
{
    using System.Collections.Generic;
    using System.Linq;
    using Apprenticeships.Application.Location;
    using Domain.Entities.Raa.Locations;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    [Parallelizable]
    public class PostalAddressSearchServiceTests
    {
        [Test]
        public void ShouldReturnSinglePostalAddressIfSingleAddressFound()
        {
            //Arrange
            var postalAddressLookupProvider = new Mock<IPostalAddressLookupProvider>();
            var serviceUnderTest = new PostalAddressSearchService(postalAddressLookupProvider.Object);

            var singleResult = new List<PostalAddress> {new Fixture().Build<PostalAddress>().Create()};
            postalAddressLookupProvider.Setup(m => m.GetValidatedPostalAddresses(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(singleResult);
            
            //Act
            var result = serviceUnderTest.GetValidatedAddress(It.IsAny<string>(), It.IsAny<string>());

            //Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void ShouldReturnNullIfNoAddressFound()
        {
            //Arrange
            var postalAddressLookupProvider = new Mock<IPostalAddressLookupProvider>();
            var serviceUnderTest = new PostalAddressSearchService(postalAddressLookupProvider.Object);

            List<PostalAddress> findResult = null;
            postalAddressLookupProvider.Setup(m => m.GetValidatedPostalAddresses(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(findResult);

            //Act
            var result = serviceUnderTest.GetValidatedAddress(It.IsAny<string>(), It.IsAny<string>());

            //Assert
            result.Should().BeNull();
        }

        [Test]
        public void ShouldReturnMultipleAddresses()
        {
            //Arrange
            var multipleResults = new Fixture().Build<PostalAddress>()
                .CreateMany().ToList();
            var postalAddressLookupProvider = new Mock<IPostalAddressLookupProvider>();
            var serviceUnderTest = new PostalAddressSearchService(postalAddressLookupProvider.Object);

            postalAddressLookupProvider.Setup(m => m.GetValidatedPostalAddresses(It.IsAny<string>()))
                .Returns(multipleResults);

            //Act
            var result = serviceUnderTest.GetValidatedAddresses(It.IsAny<string>());

            //Assert
            Assert.AreEqual(result.Count(), multipleResults.Count);
        }

        [Test]
        public void ShouldReturnNullIfNoAddressesFound()
        {
            //Arrange
            List<PostalAddress> findResult = null;
            var postalAddressLookupProvider = new Mock<IPostalAddressLookupProvider>();
            var serviceUnderTest = new PostalAddressSearchService(postalAddressLookupProvider.Object);

            postalAddressLookupProvider.Setup(m => m.GetValidatedPostalAddresses(It.IsAny<string>()))
                .Returns(findResult);

            //Act
            var result = serviceUnderTest.GetValidatedAddresses(It.IsAny<string>());

            //Assert
            result.Should().BeNull();
        }
    }
}
