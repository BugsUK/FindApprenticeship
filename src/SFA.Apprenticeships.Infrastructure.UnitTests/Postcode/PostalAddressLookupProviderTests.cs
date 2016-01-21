namespace SFA.Apprenticeships.Infrastructure.UnitTests.Postcode
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Locations;
    using Infrastructure.Postcode;
    using Infrastructure.Postcode.Configuration;
    using Infrastructure.Postcode.Entities;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using RestSharp;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    public class PostalAddressLookupProviderTests
    {
        private PostalAddressLookupProvider _palp;
        private Mock<IConfigurationService> _mockConfigurationService;
        private Mock<ILogService> _mockLogger;
        private Mock<IPostalAddressDetailsService> _mockRetrieveAddressService;
        private Mock<IRestClient> _mockClient;
        private PostalAddressServiceConfiguration addressConfig = new Fixture().Build<PostalAddressServiceConfiguration>()
            .With(x => x.FindByPartsEndpoint, "http://www.google.com")
            .Create();
        private const string _jsonSingleFindResult = "[{\"Id\":\"15499581.00\",\"StreetAddress\":\"115 Pemberton Road\",\"Place\":\"London N4\"}]";

        private const string _jsonMultiFindResult =
            @"[{""Id"":""15499584.00"", ""StreetAddress"":""81 Pemberton Road"", ""Place"":""London N4""},
{""Id"":""15499585.00"",""StreetAddress"":""83 Pemberton Road"",""Place"":""London N4""},
{""Id"":""15499586.00"",""StreetAddress"":""85 Pemberton Road"",""Place"":""London N4""}]";

        private string _nonEmptyStringParam = "salty lassi";

        //private string _ErrorResult =
        //    @"[{""Error"":""2"",""Description"":""Unknown key"",""Cause"":""The key you are using to access the service was not found."",""Resolution"":""Please check that the key is correct.It should be in the form AA11-AA11-AA11-AA11.""}]";


        [SetUp]
        public void Setup()
        {
            _mockConfigurationService = new Mock<IConfigurationService>();
            _mockLogger = new Mock<ILogService>();
            _mockRetrieveAddressService = new Mock<IPostalAddressDetailsService>();
            _mockConfigurationService.Setup(m => m.Get<PostalAddressServiceConfiguration>()).Returns(addressConfig);
            _mockClient = new Mock<IRestClient>();

            _palp = new PostalAddressLookupProvider(_mockConfigurationService.Object, _mockLogger.Object, _mockRetrieveAddressService.Object);
            _palp.Client = _mockClient.Object;
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ShouldRequireLine1()
        {
            //Arrange
            //Act
            _palp.GetValidatedPostalAddresses(null, "postcode");
            //Assert
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ShouldRequirePostcode()
        {
            //Arrange
            //Act
            _palp.GetValidatedPostalAddresses("Line 1", null);
            //Assert
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldRequirePostcodeOnly()
        {
            //Arrange
            //Act
            _palp.GetValidatedPostalAddresses(null);
            //Assert
        }

        [TestCase(1, _jsonSingleFindResult)]
        [TestCase(3, _jsonMultiFindResult)]
        public void GetPostalAddresses_ReturnsMultipleAddresses(int count, string json)
        {
            //Arrange
            var findResult = new RestResponse<List<PcaServiceFindResult>>();
            //findResult.Content = json;
            findResult.ResponseStatus = ResponseStatus.Completed;
            findResult.Data = SimpleJson.DeserializeObject<List<PcaServiceFindResult>>(json);
            _mockClient.Setup(m => m.Execute<List<PcaServiceFindResult>>(It.IsAny<IRestRequest>())).Returns(findResult);
            _mockRetrieveAddressService.Setup(m => m.RetrieveValidatedAddress(It.IsAny<string>())).Returns(new PostalAddress());

            //Act
            var result = _palp.GetValidatedPostalAddresses(_nonEmptyStringParam, _nonEmptyStringParam);

            //Assert
            Assert.AreEqual(count, result.Count());
        }

        [TestCase(1, _jsonSingleFindResult)]
        [TestCase(3, _jsonMultiFindResult)]
        public void GetPostalAddresses_PostcodeOnly_ReturnsMultipleAddresses(int count, string json)
        {
            //Arrange
            var findResult = new RestResponse<List<PcaServiceFindResult>>();
            findResult.ResponseStatus = ResponseStatus.Completed;
            findResult.Data = SimpleJson.DeserializeObject<List<PcaServiceFindResult>>(json);
            _mockClient.Setup(m => m.Execute<List<PcaServiceFindResult>>(It.IsAny<IRestRequest>())).Returns(findResult);
            _mockRetrieveAddressService.Setup(m => m.RetrieveValidatedAddress(It.IsAny<string>())).Returns(new PostalAddress());

            //Act
            var result = _palp.GetValidatedPostalAddresses(_nonEmptyStringParam);

            //Assert
            Assert.AreEqual(count, result.Count());
        }

        [Test]
        public void GetPostalAddresses_ReturnsNull()
        {
            //Arrange
            var findResult = new RestResponse<List<PcaServiceFindResult>>();
            //findResult.Content = _ErrorResult;
            findResult.ResponseStatus = ResponseStatus.Completed;
            findResult.Data = new Fixture().Build<PcaServiceFindResult>()
                .With(x => x.Id, null)
                .CreateMany(1).ToList();
            _mockClient.Setup(m => m.Execute<List<PcaServiceFindResult>>(It.IsAny<IRestRequest>())).Returns(findResult);
            _mockRetrieveAddressService.Setup(m => m.RetrieveValidatedAddress(It.IsAny<string>())).Returns(new PostalAddress());

            //Act
            var result = _palp.GetValidatedPostalAddresses(_nonEmptyStringParam, _nonEmptyStringParam);

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetPostalAddresses_PostcodeOnly_ReturnsNull()
        {
            //Arrange
            var findResult = new RestResponse<List<PcaServiceFindResult>>();
            //findResult.Content = _ErrorResult;
            findResult.ResponseStatus = ResponseStatus.Completed;
            findResult.Data = new Fixture().Build<PcaServiceFindResult>()
                .With(x => x.Id, null)
                .CreateMany(1).ToList();
            _mockClient.Setup(m => m.Execute<List<PcaServiceFindResult>>(It.IsAny<IRestRequest>())).Returns(findResult);
            _mockRetrieveAddressService.Setup(m => m.RetrieveValidatedAddress(It.IsAny<string>())).Returns(new PostalAddress());

            //Act
            var result = _palp.GetValidatedPostalAddresses(_nonEmptyStringParam);

            //Assert
            Assert.IsNull(result);
        }
    }
}
