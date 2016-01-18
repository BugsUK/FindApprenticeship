namespace SFA.Apprenticeship.Api.AvService.UnitTests.Services
{
    using System;
    using AvService.Services;
    using Domain;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Repositories;

    [TestFixture]
    public class WebServiceConsumerServiceTests
    {
        [Test]
        public void ShouldGetWebServiceConsumer()
        {
            // Arrange.
            var externalSystemId = Guid.NewGuid();
            var expectedWebServiceConsumer = new WebServiceConsumer();

            var mockWebServiceConsumerReadRepository = new Mock<IWebServiceConsumerReadRepository>();

            mockWebServiceConsumerReadRepository.Setup(mock => mock
                .Get(externalSystemId))
                .Returns(expectedWebServiceConsumer);

            var service = new WebServiceConsumerService(mockWebServiceConsumerReadRepository.Object);

            // Act.
            var webServiceConsumer = service.Get(externalSystemId);

            // Assert.
            webServiceConsumer.Should().Be(expectedWebServiceConsumer);
        }
    }
}
