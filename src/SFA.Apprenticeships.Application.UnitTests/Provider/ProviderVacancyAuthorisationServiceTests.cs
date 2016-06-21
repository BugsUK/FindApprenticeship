namespace SFA.Apprenticeships.Application.UnitTests.Provider
{
    using System;
    using Apprenticeships.Application.Provider;
    using Domain.Entities.Exceptions;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    public class ProviderVacancyAuthorisationServiceTests
    {
        [Test]
        public void ShouldAuthoriseVacancyForSignedInProviderUser()
        {
            // Arrange.
            var providerId = new Random().Next(1, 100);
            var claimValue = Convert.ToString(providerId);

            var mockCurrentUserService = new Mock<ICurrentUserService>();

            mockCurrentUserService.Setup(mock =>
                mock.GetClaimValue("providerId"))
                .Returns(claimValue);

            var service = new ProviderVacancyAuthorisationService(mockCurrentUserService.Object);

            // Act.
            Action action = () => service.Authorise(providerId);

            // Assert.
            action.ShouldNotThrow();
        }

        [Test]
        public void ShouldNotAuthoriseVacancyForAnotherProvider()
        {
            // Arrange.
            var providerId = new Random().Next(1, 100);
            const string userName = "john.doe@example.com";
            var signedInProviderId = Convert.ToString(providerId + 1);

            var mockCurrentUserService = new Mock<ICurrentUserService>();

            mockCurrentUserService.Setup(mock =>
                mock.CurrentUserName)
                .Returns(userName);

            mockCurrentUserService.Setup(mock =>
                mock.GetClaimValue("providerId"))
                .Returns(signedInProviderId);

            var service = new ProviderVacancyAuthorisationService(mockCurrentUserService.Object);

            // Act.
            Action action = () => service.Authorise(providerId);

            // Assert.
            action
                .ShouldThrow<CustomException>()
                .Which.Code.Should().Be(Interfaces.ErrorCodes.ProviderVacancyAuthorisationFailed);
        }
    }
}
