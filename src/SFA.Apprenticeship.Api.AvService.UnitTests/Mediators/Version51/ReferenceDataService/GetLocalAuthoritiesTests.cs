namespace SFA.Apprenticeship.Api.AvService.UnitTests.Mediators.Version51.ReferenceDataService
{
    using System;
    using System.Linq;
    using System.Security;
    using AvService.Providers;
    using DataContracts.Version51;
    using Domain;
    using FluentAssertions;
    using MessageContracts.Version51;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class GetLocalAuthoritiesTests : ReferenceDataServiceMediatorTestsBase
    {
        [Test]
        public void ShouldThrowIfRequestIsNull()
        {
            // Act.
            Action action = () => ReferenceDataServiceMediator.GetLocalAuthorities(default(GetLocalAuthoritiesRequest));

            // Assert.
            action.ShouldThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ShouldReflectRequestMessageIdInResponse()
        {
            // Act.
            var request = new GetLocalAuthoritiesRequest
            {
                MessageId = Guid.NewGuid()
            };

            var response = ReferenceDataServiceMediator.GetLocalAuthorities(request);

            // Assert.
            response.Should().NotBeNull();
            response.MessageId.Should().Be(request.MessageId);
        }

        [Test]
        public void ShouldAuthenticateRequest()
        {
            // Arrange.
            var request = new GetLocalAuthoritiesRequest
            {
                ExternalSystemId = Guid.NewGuid(),
                PublicKey = "dontletmein"
            };

            MockWebServiceAuthenticationProvider.Reset();

            MockWebServiceAuthenticationProvider.Setup(mock => mock
                .Authenticate(request.ExternalSystemId, request.PublicKey, WebServiceCategory.Reference))
                .Returns(WebServiceAuthenticationResult.AuthenticationFailed);

            // Act.
            Action action = () => ReferenceDataServiceMediator.GetLocalAuthorities(request);

            // Assert.
            action.ShouldThrowExactly<SecurityException>();
        }

        [Test]
        public void ShouldGetLocalAuthorities()
        {
            // Arrange.
            var localAuthorities = new Fixture().CreateMany<LocalAuthorityData>(3).ToList();
            MockReferenceDataProvider.Setup(p => p.GetLocalAuthorities()).Returns(localAuthorities);

            // Act.
            var request = new GetLocalAuthoritiesRequest();

            var response = ReferenceDataServiceMediator.GetLocalAuthorities(request);

            // Assert.
            response.Should().NotBeNull();
            response.LocalAuthorities.Should().NotBeNull();
            response.LocalAuthorities.ShouldBeEquivalentTo(localAuthorities);
        }
    }
}