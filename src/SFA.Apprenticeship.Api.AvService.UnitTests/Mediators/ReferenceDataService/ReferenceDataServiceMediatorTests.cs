namespace SFA.Apprenticeship.Api.AvService.UnitTests.Mediators.ReferenceDataService
{
    using System;
    using AvService.Mediators.Version51;
    using Common;
    using FluentAssertions;
    using MessageContracts.Version51;
    using NUnit.Framework;

    [TestFixture]
    public class ReferenceDataServiceMediatorTests
    {
        [Test]
        public void ShouldReflectRequestMessageIdInResponse()
        {
            // Arrange.
            var referenceDataServiceMediator = new ReferenceDataServiceMediator();

            // Act.
            var request = new GetErrorCodesRequest
            {
                MessageId = Guid.NewGuid()
            };

            var response = referenceDataServiceMediator.GetErrorCodes(request);

            // Assert.
            response.Should().NotBeNull();
            response.MessageId.Should().Be(request.MessageId);
        }

        [Test]
        public void ShouldGetErrorCodes()
        {
            // Arrange.
            var referenceDataServiceMediator = new ReferenceDataServiceMediator();

            // Act.
            var request = new GetErrorCodesRequest();

            var response = referenceDataServiceMediator.GetErrorCodes(request);

            // Assert.
            response.Should().NotBeNull();
            response.ErrorCodes.Should().NotBeNull();
            response.ErrorCodes.ShouldBeEquivalentTo(ApiErrors.ErrorCodes);
        }
    }
}
