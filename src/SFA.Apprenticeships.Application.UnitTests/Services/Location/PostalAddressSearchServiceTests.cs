namespace SFA.Apprenticeships.Application.UnitTests.Services.Location
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Location;
    using Domain.Entities.Locations;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
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
            _postalAddressLookupProvider.Setup(m => m.GetPostalAddresses(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(singleResult);
            
            //Act
            var result = _serviceUnderTest.GetAddress(It.IsAny<string>(), It.IsAny<string>());

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void ShouldReturnNullIfNoAddressesFound()
        {
            //Arrange
            List<PostalAddress> findResult = null;
            _postalAddressLookupProvider.Setup(m => m.GetPostalAddresses(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(findResult);

            //Act
            var result = _serviceUnderTest.GetAddress(It.IsAny<string>(), It.IsAny<string>());

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldReturnExceptionIfMultipleAddressesFound()
        {
            //Arrange
            var multipleResults = new Fixture().Build<PostalAddress>()
                .CreateMany().ToList();
            _postalAddressLookupProvider.Setup(m => m.GetPostalAddresses(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(multipleResults);

            //Act
            var result = _serviceUnderTest.GetAddress(It.IsAny<string>(), It.IsAny<string>());

            //Assert
            //expected exception
        }
    }
}
