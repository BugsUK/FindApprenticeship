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
    public class GetRegionsTests : ReferenceDataServiceMediatorTestsBase
    {
        [Test]
        public void ShouldThrowIfRequestIsNull()
        {
            // Act.
            Action action = () => ReferenceDataServiceMediator.GetRegions(default(GetRegionRequest));

            // Assert.
            action.ShouldThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ShouldReflectRequestMessageIdInResponse()
        {
            // Act.
            var request = new GetRegionRequest
            {
                MessageId = Guid.NewGuid()
            };

            var response = ReferenceDataServiceMediator.GetRegions(request);

            // Assert.
            response.Should().NotBeNull();
            response.MessageId.Should().Be(request.MessageId);
        }

        [Test]
        public void ShouldAuthenticateRequest()
        {
            // Arrange.
            var request = new GetRegionRequest
            {
                ExternalSystemId = Guid.NewGuid(),
                PublicKey = "dontletmein"
            };

            MockWebServiceAuthenticationProvider.Reset();

            MockWebServiceAuthenticationProvider.Setup(mock => mock
                .Authenticate(request.ExternalSystemId, request.PublicKey, WebServiceCategory.Reference))
                .Returns(WebServiceAuthenticationResult.AuthenticationFailed);

            // Act.
            Action action = () => ReferenceDataServiceMediator.GetRegions(request);

            // Assert.
            action.ShouldThrowExactly<SecurityException>();
        }

        [Test]
        public void ShouldGetRegions()
        {
            // Arrange.
            var regions = new Fixture().CreateMany<RegionData>(3).ToList();
            MockReferenceDataProvider.Setup(p => p.GetRegions()).Returns(regions);

            // Act.
            var request = new GetRegionRequest();

            var response = ReferenceDataServiceMediator.GetRegions(request);

            // Assert.
            response.Should().NotBeNull();
            response.Regions.Should().NotBeNull();
            response.Regions.ShouldBeEquivalentTo(regions);
        }
    }
}