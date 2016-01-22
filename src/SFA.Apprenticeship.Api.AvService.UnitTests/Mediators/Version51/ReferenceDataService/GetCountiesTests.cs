namespace SFA.Apprenticeship.Api.AvService.UnitTests.Mediators.Version51.ReferenceDataService
{
    using System;
    using System.Linq;
    using System.Security;
    using Apprenticeships.Domain.Entities.WebServices;
    using AvService.Providers;
    using DataContracts.Version51;
    using FluentAssertions;
    using MessageContracts.Version51;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class GetCountiesTests : ReferenceDataServiceMediatorTestsBase
    {
        [Test]
        public void ShouldThrowIfRequestIsNull()
        {
            // Act.
            Action action = () => ReferenceDataServiceMediator.GetCounties(default(GetCountiesRequest));

            // Assert.
            action.ShouldThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void ShouldReflectRequestMessageIdInResponse()
        {
            // Act.
            var request = new GetCountiesRequest
            {
                MessageId = Guid.NewGuid()
            };

            var response = ReferenceDataServiceMediator.GetCounties(request);

            // Assert.
            response.Should().NotBeNull();
            response.MessageId.Should().Be(request.MessageId);
        }

        [Test]
        public void ShouldAuthenticateRequest()
        {
            // Arrange.
            var request = new GetCountiesRequest
            {
                ExternalSystemId = Guid.NewGuid(),
                PublicKey = "dontletmein"
            };

            MockWebServiceAuthenticationProvider.Reset();

            MockWebServiceAuthenticationProvider.Setup(mock => mock
                .Authenticate(request.ExternalSystemId, request.PublicKey, WebServiceCategory.Reference))
                .Returns(WebServiceAuthenticationResult.AuthenticationFailed);

            // Act.
            Action action = () => ReferenceDataServiceMediator.GetCounties(request);

            // Assert.
            action.ShouldThrowExactly<SecurityException>();
        }

        [Test]
        public void ShouldGetCounties()
        {
            // Arrange.
            var counties = new Fixture().CreateMany<CountyData>(3).ToList();
            MockReferenceDataProvider.Setup(p => p.GetCounties()).Returns(counties);

            // Act.
            var request = new GetCountiesRequest();

            var response = ReferenceDataServiceMediator.GetCounties(request);

            // Assert.
            response.Should().NotBeNull();
            response.Counties.Should().NotBeNull();
            response.Counties.ShouldBeEquivalentTo(counties);
        }
    }
}