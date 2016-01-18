namespace SFA.Apprenticeship.Api.AvService.UnitTests.Mediators.Version51.ReferenceDataService
{
    using System;
    using System.Security;
    using AvService.Providers;
    using Common;
    using FluentAssertions;
    using MessageContracts.Version51;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class GetErrorCodesTests : ReferenceDataServiceMediatorTestsBase
    {
        [Test]
        public void ShouldThrowIfRequestIsNull()
        {
            // Act.
            Action action = () => ReferenceDataServiceMediator.GetErrorCodes(default(GetErrorCodesRequest));

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

            var response = ReferenceDataServiceMediator.GetErrorCodes(request);

            // Assert.
            response.Should().NotBeNull();
            response.MessageId.Should().Be(request.MessageId);
        }

        [Test]
        public void ShouldAuthenticateRequest()
        {
            // Arrange.
            var request = new GetErrorCodesRequest
            {
                ExternalSystemId = Guid.NewGuid(),
                PublicKey = "dontletmein"
            };

            MockWebServiceAuthenticationProvider.Reset();

            MockWebServiceAuthenticationProvider.Setup(mock => mock
                .Authenticate(request.ExternalSystemId, request.PublicKey))
                .Returns(AuthenticationResult.AuthenticationFailed);

            // Act.
            Action action = () => ReferenceDataServiceMediator.GetErrorCodes(request);

            // Assert.
            action.ShouldThrowExactly<SecurityException>();
        }

        [Test]
        public void ShouldGetErrorCodes()
        {
            // Act.
            var request = new GetErrorCodesRequest();

            var response = ReferenceDataServiceMediator.GetErrorCodes(request);

            // Assert.
            response.Should().NotBeNull();
            response.ErrorCodes.Should().NotBeNull();
            response.ErrorCodes.ShouldBeEquivalentTo(ApiErrors.ErrorCodes);
        }
    }
}
