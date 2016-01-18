namespace SFA.Apprenticeship.Api.AvService.UnitTests.Mediators.Version51.ReferenceDataService
{
    using System;
    using System.Security;
    using AvService.Mediators.Version51;
    using AvService.Providers;
    using Common;
    using FluentAssertions;
    using MessageContracts.Version51;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ReferenceDataServiceMediatorTests
    {
        private Mock<IWebServiceAuthenticationProvider> _mockWebServiceAuthenticationProvider;

        private ReferenceDataServiceMediator _referenceDataServiceMediator;

        [SetUp]
        public void SetUp()
        {
            // Providers.
            _mockWebServiceAuthenticationProvider = new Mock<IWebServiceAuthenticationProvider>();

            _mockWebServiceAuthenticationProvider.Setup(mock => mock
                .Authenticate(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(AuthenticationResult.Authenticated);

            // Mediator.
            _referenceDataServiceMediator = new ReferenceDataServiceMediator(
                _mockWebServiceAuthenticationProvider.Object);
        }

        [Test]
        public void ShouldThrowIfRequestIsNull()
        {
            // Act.
            Action action = () => _referenceDataServiceMediator.GetErrorCodes(default(GetErrorCodesRequest));

            // Assert.
            action.ShouldThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ShouldReflectRequestMessageIdInResponse()
        {
            // Act.
            var request = new GetErrorCodesRequest
            {
                MessageId = Guid.NewGuid()
            };

            var response = _referenceDataServiceMediator.GetErrorCodes(request);

            // Assert.
            response.Should().NotBeNull();
            response.MessageId.Should().Be(request.MessageId);
        }

        [Test]
        public void ShouldGetErrorCodes()
        {
            // Act.
            var request = new GetErrorCodesRequest();

            var response = _referenceDataServiceMediator.GetErrorCodes(request);

            // Assert.
            response.Should().NotBeNull();
            response.ErrorCodes.Should().NotBeNull();
            response.ErrorCodes.ShouldBeEquivalentTo(ApiErrors.ErrorCodes);
        }

        [Test]
        public void ShouldAuthenticateRequest()
        {
            // Arrange.
            _referenceDataServiceMediator = new ReferenceDataServiceMediator(_mockWebServiceAuthenticationProvider.Object);

            var request = new GetErrorCodesRequest
            {
                ExternalSystemId = Guid.NewGuid(),
                PublicKey = "dontletmein"
            };

            _mockWebServiceAuthenticationProvider.Reset();

            _mockWebServiceAuthenticationProvider.Setup(mock => mock
                .Authenticate(request.ExternalSystemId, request.PublicKey))
                .Returns(AuthenticationResult.AuthenticationFailed);

            // Act.
            Action action = () => _referenceDataServiceMediator.GetErrorCodes(request);

            // Assert.
            action.ShouldThrowExactly<SecurityException>();
        }
    }
}
