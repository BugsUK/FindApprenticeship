namespace SFA.Apprenticeships.Infrastructure.UnitTests.Postcode
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure.Postcode;
    using Infrastructure.Postcode.Configuration;
    using Infrastructure.Postcode.Entities;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using RestSharp;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    public class PostalAddressDetailsServiceTests
    {
        private PostalAddressDetailsService _serviceUnderTest;
        private Mock<IConfigurationService> _mockConfigService;
        private Mock<ILogService> _mockLogger;
        private Mock<IRestClient> _mockRestClient;
        private PostalAddressServiceConfiguration _config;
        private string _nonEmptyStringParam = "salty lassi";

        [SetUp]
        public void Setup()
        {
            _config = new Fixture().Build<PostalAddressServiceConfiguration>().With(x => x.RetrieveByIdEndpoint, "http://www.google.com").Create();
            _mockConfigService = new Mock<IConfigurationService>();
            _mockConfigService.Setup(m => m.Get<PostalAddressServiceConfiguration>()).Returns(_config);
            _mockLogger = new Mock<ILogService>();
            _mockRestClient = new Mock<IRestClient>();
            _serviceUnderTest = new PostalAddressDetailsService(_mockConfigService.Object, _mockLogger.Object);
            _serviceUnderTest.Client = _mockRestClient.Object;
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldExpectAddressId()
        {
            //Arrange
            string nullArgument = null;
            //Act
            _serviceUnderTest.RetrieveAddress(nullArgument);
            //Assert
        }

        [Test]
        public void ShouldReturnFullyMappedPostalAddress()
        {
            //Arrange
            var apiAddress = new Fixture().Build<PcaAddress>().CreateMany(1).ToList();
            var apiResult = new RestResponse<List<PcaAddress>>();
            apiResult.Data = apiAddress;
            apiResult.ResponseStatus = ResponseStatus.Completed;
            _mockRestClient.Setup(m => m.Execute<List<PcaAddress>>(It.IsAny<IRestRequest>())).Returns(apiResult);
            //Act
            var result = _serviceUnderTest.RetrieveAddress(_nonEmptyStringParam);
            //Assert
            Assert.AreEqual(apiAddress.Single().Line1, result.AddressLine1);
            Assert.AreEqual(apiAddress.Single().Line2, result.AddressLine2);
            Assert.AreEqual(apiAddress.Single().Line3, result.AddressLine3);
            Assert.AreEqual(apiAddress.Single().Line4, result.AddressLine4);
            Assert.AreEqual(apiAddress.Single().Line5, result.AddressLine5);
            Assert.AreEqual(apiAddress.Single().Postcode, result.Postcode);
            Assert.AreEqual(apiAddress.Single().PostTown, result.Town);
            Assert.AreEqual(apiAddress.Single().Udprn, result.ValidationSourceKeyValue);
            Assert.AreEqual("PCA", result.ValidationSourceCode);

        }

        [Test]
        public void ShouldReturnNullIfNoResults()
        {
            //Arrange
            var apiAddress = new Fixture().Build<PcaAddress>()
                .With(x => x.Udprn, null)
                .With(x => x.Line1, null)
                .With(x => x.Line2, null)
                .With(x => x.Postcode, null)
                .With(x => x.PostTown, null)
                .CreateMany(1).ToList();

            var apiResult = new RestResponse<List<PcaAddress>>();
            apiResult.Data = apiAddress;
            //I know what you're thinking: surely this should be null.
            //But no: RESTSharp will create a new object, with empty properties. It's mental.
            apiResult.ResponseStatus = ResponseStatus.Completed;
            _mockRestClient.Setup(m => m.Execute<List<PcaAddress>>(It.IsAny<IRestRequest>())).Returns(apiResult);
            //Act
            var result = _serviceUnderTest.RetrieveAddress(_nonEmptyStringParam);
            //Assert
            Assert.IsNull(result);
        }
    }
}
