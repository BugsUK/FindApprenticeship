namespace SFA.Apprenticeships.Infrastructure.UnitTests.EmployerDataService
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Locations;
    using Domain.Entities.Locations;
    using Infrastructure.EmployerDataService.Configuration;
    using Infrastructure.EmployerDataService.EmployerDataService;
    using Infrastructure.EmployerDataService.Providers;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using SFA.Infrastructure.Interfaces;
    using WebServices.Wcf;

    public class EmployerAddressValidationTests
    {
        private Mock<ILogService> _mockLogService;
        private Mock<IConfigurationService> _mockConfigService;
        private Mock<IWcfService<EmployerLookupSoap>> _mockWcfService;
        private Mock<IPostalAddressSearchService> _mockPostalAddressSearchService;
        private EmployerDataProvider _serviceUnderTest;

        [SetUp]
        public void Setup()
        {
            _mockLogService = new Mock<ILogService>();
            _mockConfigService = new Mock<IConfigurationService>();
            var config = new Fixture().Build<EmployerDataServiceConfiguration>().Create();
            _mockConfigService.Setup(m => m.Get<EmployerDataServiceConfiguration>()).Returns(config);
            _mockWcfService = new Mock<IWcfService<EmployerLookupSoap>>();
            _mockPostalAddressSearchService = new Mock<IPostalAddressSearchService>();

            _serviceUnderTest = new EmployerDataProvider(
                _mockLogService.Object,
                _mockConfigService.Object,
                _mockWcfService.Object,
                _mockPostalAddressSearchService.Object);
        }

        //INPUT HAS Address.PAON.Items(Count > 0)
        //PostcodeAndLine1SingleResult_PASS
        //PostcodeAndLine1ManyResults_FAIL_and_PostcodeAloneSingleResult_PASS
        //PostcodeAndLine1ManyResults_FAIL_and_PostcodeAloneManyResults_PASS
        //PostcodeAndLine1ManyResults_FAIL_and_PostcodeAloneNoResults_FAIL_and_NaiveMapping
        //PostcodeAndLine1NoResults_FAIL_and_PostcodeAloneSingleResult_PASS
        //PostcodeAndLine1NoResults_FAIL_and_PostcodeAloneManyResults_PASS
        //PostcodeAndLine1NoResults_FAIL_and_PostcodeAloneNoResults_FAIL_and_NaiveMapping

        //INPUT DOES NOT HAVE Address.PAON/Address.PAON.Items
        //PostcodeAloneSingleResult_PASS
        //PostcodeAloneManyResults_PASS
        //PostcodeAloneNoResults_FAIL_and_NaiveMapping


        [Test]
        public void PostcodeAndLine1SingleResult_PASS()
        {
            //Arrange
            var input = new Fixture().Build<ConciseEmployerStructure>().Create();
            var foundAddressList = new Fixture().Build<PostalAddress>()
                .With(x => x.ValidationSourceCode, "PCA").CreateMany(1); //the single result
            _mockPostalAddressSearchService.Setup(
                m => m.GetValidatedAddress(It.IsNotNull<string>(), It.IsNotNull<string>())).Returns(foundAddressList);
            //Act
            var result = _serviceUnderTest.ValidatedAddress(input);
            //Assert
            Assert.AreEqual(foundAddressList.First().ValidationSourceKeyValue, result.Address.ValidationSourceKeyValue);
            Assert.AreEqual("PCA", result.Address.ValidationSourceCode);
        }

        [TestCase(2)]
        [TestCase(20)]
        public void PostcodeAndLine1ManyResults_FAIL_and_PostcodeAloneSingleResult_PASS(int findCount)
        {
            //Arrange
            var input = new Fixture().Build<ConciseEmployerStructure>().Create();
            var foundAddressList = new Fixture().Build<PostalAddress>()
                .With(x => x.ValidationSourceCode, "PCA").CreateMany(findCount).ToList();
            _mockPostalAddressSearchService.Setup(
                m => m.GetValidatedAddress(It.IsNotNull<string>(), It.IsNotNull<string>())).Returns(foundAddressList);

            var foundAddressList2 = new Fixture().Build<PostalAddress>()
                .With(x => x.ValidationSourceCode, "PCA").CreateMany(1).ToList();
            _mockPostalAddressSearchService.Setup(
                m => m.GetValidatedAddresses(It.IsNotNull<string>())).Returns(foundAddressList2);
            //Act
            var result = _serviceUnderTest.ValidatedAddress(input);
            //Assert
            Assert.AreEqual(foundAddressList2.First().ValidationSourceKeyValue, result.Address.ValidationSourceKeyValue);
            Assert.AreEqual("PCA", result.Address.ValidationSourceCode);
        }

        [TestCase(2)]
        [TestCase(23)]
        public void PostcodeAndLine1ManyResults_FAIL_and_PostcodeAloneManyResults_PASS(int findCount)
        {
            //Arrange
            var input = new Fixture().Build<ConciseEmployerStructure>().Create();
            var foundAddressList = new Fixture().Build<PostalAddress>()
                .With(x => x.ValidationSourceCode, "PCA").CreateMany(findCount).ToList();
            _mockPostalAddressSearchService.Setup(
                m => m.GetValidatedAddress(It.IsNotNull<string>(), It.IsNotNull<string>())).Returns(foundAddressList);

            var foundAddressList2 = new Fixture().Build<PostalAddress>()
                .With(x => x.ValidationSourceCode, "PCA").CreateMany(findCount).ToList();
            _mockPostalAddressSearchService.Setup(
                m => m.GetValidatedAddresses(It.IsNotNull<string>())).Returns(foundAddressList2);
            //Act
            var result = _serviceUnderTest.ValidatedAddress(input);
            //Assert
            Assert.AreEqual(foundAddressList2.First().ValidationSourceKeyValue, result.Address.ValidationSourceKeyValue);
            Assert.AreEqual("PCA", result.Address.ValidationSourceCode);
        }

        [TestCase(2)]
        [TestCase(23)]
        public void PostcodeAndLine1ManyResults_FAIL_and_PostcodeAloneNoResults_FAIL_and_NaiveMapping(int findCount)
        {
            //Arrange
            var input = new Fixture().Build<ConciseEmployerStructure>().Create();
            var foundAddressList = new Fixture().Build<PostalAddress>()
                .With(x => x.ValidationSourceCode, "PCA").CreateMany(findCount).ToList();
            _mockPostalAddressSearchService.Setup(
                m => m.GetValidatedAddress(It.IsNotNull<string>(), It.IsNotNull<string>())).Returns(foundAddressList);

            var foundAddressList2 = new List<PostalAddress>();
            _mockPostalAddressSearchService.Setup(
                m => m.GetValidatedAddresses(It.IsNotNull<string>())).Returns(foundAddressList2);
            //Act
            var result = _serviceUnderTest.ValidatedAddress(input);

            //Assert. Naive mapping indicates that it is not validated
            Assert.AreEqual(null, result.Address.ValidationSourceKeyValue);
            Assert.AreEqual(null, result.Address.ValidationSourceCode);
            Assert.AreEqual(input.Address.PAON.Items.First().ToString(), result.Address.AddressLine1);
            Assert.AreEqual(input.Address.PostCode, result.Address.Postcode);
        }

        [Test]
        public void PostcodeAndLine1NoResults_FAIL_and_PostcodeAloneSingleResult_PASS()
        {
            //Arrange
            var input = new Fixture().Build<ConciseEmployerStructure>().Create();
            var foundAddressList = new List<PostalAddress>();
            _mockPostalAddressSearchService.Setup(
                m => m.GetValidatedAddress(It.IsNotNull<string>(), It.IsNotNull<string>())).Returns(foundAddressList);

            var foundAddressList2 = new Fixture().Build<PostalAddress>()
                .With(x => x.ValidationSourceCode, "PCA").CreateMany(1).ToList();
            _mockPostalAddressSearchService.Setup(
                m => m.GetValidatedAddresses(It.IsNotNull<string>())).Returns(foundAddressList2);
            //Act
            var result = _serviceUnderTest.ValidatedAddress(input);
            //Assert
            Assert.AreEqual(foundAddressList2.First().ValidationSourceKeyValue, result.Address.ValidationSourceKeyValue);
            Assert.AreEqual("PCA", result.Address.ValidationSourceCode);
        }

        [TestCase(2)]
        [TestCase(23)]
        public void PostcodeAndLine1NoResults_FAIL_and_PostcodeAloneManyResults_PASS(int findCount)
        {
            //Arrange
            var input = new Fixture().Build<ConciseEmployerStructure>().Create();
            var foundAddressList = new List<PostalAddress>();
            _mockPostalAddressSearchService.Setup(
                m => m.GetValidatedAddress(It.IsNotNull<string>(), It.IsNotNull<string>())).Returns(foundAddressList);

            var foundAddressList2 = new Fixture().Build<PostalAddress>()
                .With(x => x.ValidationSourceCode, "PCA").CreateMany(findCount).ToList();
            _mockPostalAddressSearchService.Setup(
                m => m.GetValidatedAddresses(It.IsNotNull<string>())).Returns(foundAddressList2);
            //Act
            var result = _serviceUnderTest.ValidatedAddress(input);
            //Assert
            Assert.AreEqual(foundAddressList2.First().ValidationSourceKeyValue, result.Address.ValidationSourceKeyValue);
            Assert.AreEqual("PCA", result.Address.ValidationSourceCode);
        }

        [TestCase(2)]
        [TestCase(23)]
        public void PostcodeAndLine1NoResults_FAIL_and_PostcodeAloneNoResults_FAIL_and_NaiveMapping(int findCount)
        {
            //Arrange
            var input = new Fixture().Build<ConciseEmployerStructure>().Create();
            var foundAddressList = new List<PostalAddress>();
            _mockPostalAddressSearchService.Setup(
                m => m.GetValidatedAddress(It.IsNotNull<string>(), It.IsNotNull<string>())).Returns(foundAddressList);

            var foundAddressList2 = new List<PostalAddress>();
            _mockPostalAddressSearchService.Setup(
                m => m.GetValidatedAddresses(It.IsNotNull<string>())).Returns(foundAddressList2);
            //Act
            var result = _serviceUnderTest.ValidatedAddress(input);

            //Assert. Naive mapping indicates that it is not validated
            Assert.AreEqual(null, result.Address.ValidationSourceKeyValue);
            Assert.AreEqual(null, result.Address.ValidationSourceCode);
            Assert.AreEqual(input.Address.PAON.Items.First().ToString(), result.Address.AddressLine1);
            Assert.AreEqual(input.Address.PostCode, result.Address.Postcode);
        }

        [Test]
        public void PostcodeAloneSingleResult_PASS()
        {
            //Arrange
            var input = new Fixture().Build<ConciseEmployerStructure>()
                .With(x => x.Address, new BSaddressStructure() {PostCode = "FAKE", PAON = null}).Create(); //not providing addressLine1, so should not try to obtain address with this

            var foundAddressList = new Fixture().Build<PostalAddress>()
                .With(x => x.ValidationSourceCode, "PCA").CreateMany(1).ToList();
            _mockPostalAddressSearchService.Setup(
                m => m.GetValidatedAddresses(It.IsNotNull<string>())).Returns(foundAddressList);
            //Act
            var result = _serviceUnderTest.ValidatedAddress(input);
            //Assert
            _mockPostalAddressSearchService.Verify(m => m.GetValidatedAddress(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            Assert.AreEqual(foundAddressList.First().ValidationSourceKeyValue, result.Address.ValidationSourceKeyValue);
            Assert.AreEqual("PCA", result.Address.ValidationSourceCode);
        }

        [TestCase(2)]
        [TestCase(23)]
        public void PostcodeAloneManyResults_PASS(int findCount)
        {
            //Arrange
            var input = new Fixture().Build<ConciseEmployerStructure>()
                .With(x => x.Address, new BSaddressStructure() {PostCode = "FAKE", PAON = null}).Create(); //not providing addressLine1, so should not try to obtain address with this
                
            var foundAddressList = new Fixture().Build<PostalAddress>()
                .With(x => x.ValidationSourceCode, "PCA").CreateMany(findCount).ToList();
            _mockPostalAddressSearchService.Setup(
                m => m.GetValidatedAddresses(It.IsNotNull<string>())).Returns(foundAddressList);
            //Act
            var result = _serviceUnderTest.ValidatedAddress(input);
            //Assert
            _mockPostalAddressSearchService.Verify(m => m.GetValidatedAddress(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            Assert.AreEqual(foundAddressList.First().ValidationSourceKeyValue, result.Address.ValidationSourceKeyValue);
            Assert.AreEqual("PCA", result.Address.ValidationSourceCode);
        }

        [TestCase(2)]
        [TestCase(23)]
        public void PostcodeAloneNoResults_FAIL_and_NaiveMapping(int findCount)
        {
            //Arrange
            var input = new Fixture().Build<ConciseEmployerStructure>()
                .With(x => x.Address, new BSaddressStructure() {PostCode = "FAKE", PAON = null}).Create(); //not providing addressLine1, so should not try to obtain address with this
                
            var foundAddressList = new List<PostalAddress>();
            _mockPostalAddressSearchService.Setup(
                m => m.GetValidatedAddresses(It.IsNotNull<string>())).Returns(foundAddressList);
            //Act
            var result = _serviceUnderTest.ValidatedAddress(input);

            //Assert. Naive mapping indicates that it is not validated
            _mockPostalAddressSearchService.Verify(m => m.GetValidatedAddress(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            Assert.AreEqual(null, result.Address.ValidationSourceKeyValue);
            Assert.AreEqual(null, result.Address.ValidationSourceCode);
            Assert.AreEqual(input.Address.PostCode, result.Address.Postcode);
        }

    }
}
