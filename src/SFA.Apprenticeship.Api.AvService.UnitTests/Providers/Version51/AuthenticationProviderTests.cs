namespace SFA.Apprenticeship.Api.AvService.UnitTests.Providers.Version51
{
    using System;
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

    public enum AuthenticationResult
    {
        Unknown = 0,
        Authenticated = 1,
        InvalidPublicKey = 2,
        AuthenticationFailed = 3,
        InvalidExternalSystemId = 4
    }

    public class WebServiceAuthenticationProvider : IWebServiceAuthenticationProvider
    {
        private readonly IWebServiceConsumerService _webServiceConsumerService;

        public WebServiceAuthenticationProvider(IWebServiceConsumerService webServiceConsumerService)
        {
            _webServiceConsumerService = webServiceConsumerService;
        }

        public AuthenticationResult Authenticate(Guid externalSystemId, string publicKey)
        {
            if (string.IsNullOrWhiteSpace(publicKey))
            {
                return AuthenticationResult.InvalidPublicKey;    
            }

            var webServiceConsumer = _webServiceConsumerService.Get(externalSystemId);

            if (webServiceConsumer == null)
            {
                return AuthenticationResult.InvalidExternalSystemId;
            }

            if (webServiceConsumer.PublicKey == publicKey)
            {
                return AuthenticationResult.Authenticated;
            }

            return AuthenticationResult.AuthenticationFailed;
        }
    }

    public interface IWebServiceConsumerService
    {
        WebServiceConsumer Get(Guid externalSystemId);
    }

    public class WebServiceConsumer
    {
        public Guid ExternalSystemId { get; set; }
        public string PublicKey { get; set; }
    }

    public interface IWebServiceAuthenticationProvider
    {
        AuthenticationResult Authenticate(Guid externalSystemId, string publicKey);
    }
}
