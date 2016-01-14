namespace SFA.Apprenticeship.Api.AvService.UnitTests.Providers
{
    using System;
    using AvService.Providers;
    using AvService.Services;
    using Domain;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class AuthenticationProviderTests
    {
        private const string ValidPublicKey = "letmein";
        private const string InvalidPublicKey = "dontletmein";

        private const string ValidExternalSystemId = "bbd33d6d-fc94-4be3-9e52-282bf7293356";
        private const string InvalidExternalSystemId = "63c545db-d08f-4a9b-b05b-33727432e987";

        private Mock<IWebServiceConsumerService> _mockWebServiceConsumerService;
        private IWebServiceAuthenticationProvider _webServiceAuthenticationProvider;

        [SetUp]
        public void SetUp()
        {
            // Services.
            _mockWebServiceConsumerService = new Mock<IWebServiceConsumerService>();

            var validWebConsumer = new WebServiceConsumer
            {
                ExternalSystemId = new Guid(ValidExternalSystemId),
                PublicKey = ValidPublicKey
            };

            _mockWebServiceConsumerService.Setup(mock => mock
                .Get(new Guid(ValidExternalSystemId)))
                .Returns(validWebConsumer);

            _mockWebServiceConsumerService.Setup(mock => mock
                .Get(new Guid(InvalidExternalSystemId)))
                .Returns(default(WebServiceConsumer));

            // Provider under test.
            _webServiceAuthenticationProvider = new WebServiceAuthenticationProvider(_mockWebServiceConsumerService.Object);
        }

        [TestCase(ValidExternalSystemId, ValidPublicKey, AuthenticationResult.Authenticated)]
        [TestCase(ValidExternalSystemId, InvalidPublicKey, AuthenticationResult.AuthenticationFailed)]
        [TestCase(ValidExternalSystemId, "", AuthenticationResult.InvalidPublicKey)]
        [TestCase(ValidExternalSystemId, " ", AuthenticationResult.InvalidPublicKey)]
        [TestCase(InvalidExternalSystemId, ValidPublicKey, AuthenticationResult.InvalidExternalSystemId)]
        public void ShouldAuthenticate(string externalSystemId, string publicKey, AuthenticationResult expectedAuthenticationResult)
        {
            // Act.
            var result = _webServiceAuthenticationProvider.Authenticate(new Guid(externalSystemId), publicKey);

            // Assert.
            result.Should().Be(expectedAuthenticationResult);
        }

    }
}
