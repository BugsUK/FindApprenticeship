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
        private PostalAddressSearchService _serviceUnderTest;
        private Mock<IPostalAddressLookupProvider> _postalAddressLookupProvider;

        [SetUp]
        public void Setup()
        {
            _postalAddressLookupProvider = new Mock<IPostalAddressLookupProvider>();
            _serviceUnderTest = new PostalAddressSearchService(_postalAddressLookupProvider.Object);
        }


        [Test]
        public void ShouldReturnSinglePostalAddressIfSingleAddressFound()
        {
            //Arrange
            var singleResult = new List<PostalAddress> {new Fixture().Build<PostalAddress>().Create()};
            _postalAddressLookupProvider.Setup(m => m.GetValidatedPostalAddresses(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(singleResult);
            
            //Act
            var result = _serviceUnderTest.GetValidatedAddress(It.IsAny<string>(), It.IsAny<string>());

            //Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void ShouldReturnNullIfNoAddressFound()
        {
            //Arrange
            List<PostalAddress> findResult = null;
            _postalAddressLookupProvider.Setup(m => m.GetValidatedPostalAddresses(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(findResult);

            //Act
            var result = _serviceUnderTest.GetValidatedAddress(It.IsAny<string>(), It.IsAny<string>());

            //Assert
            result.Should().BeNull();
        }

        [Test]
        public void ShouldReturnNullIfMultipleAddressesFound()
        {
            //Arrange
            var multipleResults = new Fixture().Build<PostalAddress>()
                .CreateMany().ToList();
            _postalAddressLookupProvider.Setup(m => m.GetValidatedPostalAddresses(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(multipleResults);

            //Act
            var result = _serviceUnderTest.GetValidatedAddress(It.IsAny<string>(), It.IsAny<string>());

            //Asset
            result.Should().BeNull();
        }

        [Test]
        public void ShouldReturnMultipleAddresses()
        {
            //Arrange
            var multipleResults = new Fixture().Build<PostalAddress>()
                .CreateMany().ToList();
            _postalAddressLookupProvider.Setup(m => m.GetValidatedPostalAddresses(It.IsAny<string>()))
                .Returns(multipleResults);

            //Act
            var result = _serviceUnderTest.GetValidatedAddresses(It.IsAny<string>());

            //Assert
            Assert.AreEqual(result.Count(), multipleResults.Count);
        }

        [Test]
        public void ShouldReturnNullIfNoAddressesFound()
        {
            //Arrange
            List<PostalAddress> findResult = null;
            _postalAddressLookupProvider.Setup(m => m.GetValidatedPostalAddresses(It.IsAny<string>()))
                .Returns(findResult);

            //Act
            var result = _serviceUnderTest.GetValidatedAddresses(It.IsAny<string>());

            //Assert
            result.Should().BeNull();
        }
    }
}
