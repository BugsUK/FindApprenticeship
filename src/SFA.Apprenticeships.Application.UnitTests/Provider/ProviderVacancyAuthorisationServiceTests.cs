namespace SFA.Apprenticeships.Application.UnitTests.Provider
{
    using System;
    using Apprenticeships.Application.Provider;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Raa;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    public class ProviderVacancyAuthorisationServiceTests
    {
        [Test]
        public void ShouldAuthoriseProviderUserForVacancyForOwnProvider()
        {
            // Arrange.
            var providerId = new Random().Next(1, 100);
            var claimValue = Convert.ToString(providerId);

            var mockCurrentUserService = new Mock<ICurrentUserService>();

            mockCurrentUserService.Setup(mock =>
                mock.IsInRole(Roles.Faa))
                .Returns(true);

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
        public void ShouldNotAuthoriseProviderUserForVacancyForOtherProvider()
        {
            // Arrange.
            var providerId = new Random().Next(1, 100);
            const string userName = "john.provider@example.com";
            var signedInProviderId = Convert.ToString(providerId + 1);

            var mockCurrentUserService = new Mock<ICurrentUserService>();

            mockCurrentUserService.Setup(mock =>
                mock.CurrentUserName)
                .Returns(userName);

            mockCurrentUserService.Setup(mock =>
                mock.IsInRole(Roles.Faa))
                .Returns(true);

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

        [Test]
        public void ShouldAuthoriseQaUserForAnyVacancy()
        {
            // Arrange.
            var providerId = new Random().Next(1, 100);

            var mockCurrentUserService = new Mock<ICurrentUserService>();

            mockCurrentUserService.Setup(mock =>
                mock.IsInRole(Roles.Raa))
                .Returns(true);

            var service = new ProviderVacancyAuthorisationService(mockCurrentUserService.Object);

            // Act.
            Action action = () => service.Authorise(providerId);

            // Assert.
            action.ShouldNotThrow();
        }
    }
}
