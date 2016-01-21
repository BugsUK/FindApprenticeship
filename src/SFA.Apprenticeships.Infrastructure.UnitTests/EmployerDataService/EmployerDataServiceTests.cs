namespace SFA.Apprenticeships.Infrastructure.UnitTests.EmployerDataService
{
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

    public class EmployerDataServiceTests
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

        [Test]
        public void TestAddressIsValidatedUsingPostcodeAndLine1()
        {
            //Arrange
            var input = new Fixture().Build<ConciseEmployerStructure>().Create();
            var foundAddress = new Fixture().Build<PostalAddress>()
                .With(x => x.ValidationSourceCode, "PCA").Create();
            _mockPostalAddressSearchService.Setup(
                m => m.GetValidatedAddress(It.IsNotNull<string>(), It.IsNotNull<string>())).Returns(foundAddress);
            //Act
            var result = _serviceUnderTest.ValidatedAddress(input);
            //Assert
            Assert.AreEqual(foundAddress.ValidationSourceKeyValue, result.Address.ValidationSourceKeyValue);
            Assert.AreEqual("PCA", result.Address.ValidationSourceCode);
        }

        [Test]
        public void TestAddressIsValidatedUsingPostcodeAlone()
        {
            //Arrange
            var input = new Fixture().Build<ConciseEmployerStructure>().Create();
            var foundAddressList = new Fixture().Build<PostalAddress>()
                .With(x => x.ValidationSourceCode, "PCA")
                .CreateMany();
            _mockPostalAddressSearchService.Setup(
                m => m.GetValidatedAddresses(It.IsNotNull<string>())).Returns(foundAddressList);
            //Act
            var result = _serviceUnderTest.ValidatedAddress(input);
            //Assert
            Assert.AreEqual(foundAddressList.First().ValidationSourceKeyValue, result.Address.ValidationSourceKeyValue);
            Assert.AreEqual("PCA", result.Address.ValidationSourceCode);
        }

        [Test]
        public void TestAddressIsNotValidatedButAssignedFromServiceResult()
        {
            var input = new Fixture().Build<ConciseEmployerStructure>().Create();
            var foundAddressList = new Fixture().Build<PostalAddress>().CreateMany(0);
            _mockPostalAddressSearchService.Setup(
                m => m.GetValidatedAddresses(It.IsNotNull<string>())).Returns(foundAddressList);
            //Act
            var result = _serviceUnderTest.ValidatedAddress(input);
            //Assert
            Assert.IsNull(result.Address.ValidationSourceKeyValue);
            Assert.IsNull(result.Address.ValidationSourceCode);
        }
    }
}
