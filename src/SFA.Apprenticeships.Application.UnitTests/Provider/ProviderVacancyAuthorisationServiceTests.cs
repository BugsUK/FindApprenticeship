namespace SFA.Apprenticeships.Application.UnitTests.Provider
{
    using System;
    using System.Collections.Generic;
    using Apprenticeships.Application.Provider;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Raa;
    using Domain.Entities.Raa.Parties;
    using FluentAssertions;
    using Interfaces.Providers;
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
            var providerSiteId = new Random().Next(101, 200);
            var claimValue = Convert.ToString(providerId);

            var mockCurrentUserService = new Mock<ICurrentUserService>();
            var mockProviderService = new Mock<IProviderService>();

            mockCurrentUserService.Setup(mock =>
                mock.IsInRole(Roles.Faa))
                .Returns(true);

            mockCurrentUserService.Setup(mock =>
                mock.GetClaimValue("providerId"))
                .Returns(claimValue);

            var service = new ProviderVacancyAuthorisationService(mockCurrentUserService.Object, mockProviderService.Object);

            // Act.
            Action action = () => service.Authorise(providerId, providerSiteId);

            // Assert.
            action.ShouldNotThrow();
        }

        [Test]
        public void ShouldNotAuthoriseProviderUserForVacancyForOtherProvider()
        {
            // Arrange.
            var providerId = new Random().Next(1, 100);
            var providerSiteId = new Random().Next(101, 200);
            const string userName = "john.provider@example.com";
            var signedInProviderId = Convert.ToString(providerId + 1);

            var mockCurrentUserService = new Mock<ICurrentUserService>();
            var mockProviderService = new Mock<IProviderService>();

            mockCurrentUserService.Setup(mock =>
                mock.CurrentUserName)
                .Returns(userName);

            mockCurrentUserService.Setup(mock =>
                mock.IsInRole(Roles.Faa))
                .Returns(true);

            mockCurrentUserService.Setup(mock =>
                mock.GetClaimValue("providerId"))
                .Returns(signedInProviderId);

            var service = new ProviderVacancyAuthorisationService(mockCurrentUserService.Object, mockProviderService.Object);

            // Act.
            Action action = () => service.Authorise(providerId, providerSiteId);

            // Assert.
            action
                .ShouldThrow<CustomException>()
                .Which.Code.Should().Be(Interfaces.ErrorCodes.ProviderVacancyAuthorisationFailed);
        }

        [Test]
        public void ShouldAuthoriseProviderUserForVacancyForOwnProviderSite()
        {
            // Arrange.
            var providerId = new Random().Next(1, 50);
            var subContractorProviderId = new Random().Next(51, 100);
            var providerSiteId = new Random().Next(101, 200);
            var claimValue = Convert.ToString(providerId);

            var mockCurrentUserService = new Mock<ICurrentUserService>();
            var mockProviderService = new Mock<IProviderService>();

            mockCurrentUserService.Setup(mock =>
                mock.IsInRole(Roles.Faa))
                .Returns(true);

            mockCurrentUserService.Setup(mock =>
                mock.GetClaimValue("providerId"))
                .Returns(claimValue);

            mockProviderService.Setup(mock => mock.GetProviderSites(providerId))
                .Returns(new List<ProviderSite> {new ProviderSite {ProviderSiteId = providerSiteId}});

            var service = new ProviderVacancyAuthorisationService(mockCurrentUserService.Object, mockProviderService.Object);

            // Act.
            Action action = () => service.Authorise(subContractorProviderId, providerSiteId);

            // Assert.
            action.ShouldNotThrow();
        }

        [Test]
        public void ShouldNotAuthoriseProviderUserForVacancyForOtherProviderSite()
        {
            // Arrange.
            var providerId = new Random().Next(1, 50);
            var subContractorProviderId = new Random().Next(51, 100);
            var providerSiteId = new Random().Next(101, 200);
            const string userName = "john.provider@example.com";
            var signedInProviderId = Convert.ToString(providerId + 1);

            var mockCurrentUserService = new Mock<ICurrentUserService>();
            var mockProviderService = new Mock<IProviderService>();

            mockCurrentUserService.Setup(mock =>
                mock.CurrentUserName)
                .Returns(userName);

            mockCurrentUserService.Setup(mock =>
                mock.IsInRole(Roles.Faa))
                .Returns(true);

            mockCurrentUserService.Setup(mock =>
                mock.GetClaimValue("providerId"))
                .Returns(signedInProviderId);

            var service = new ProviderVacancyAuthorisationService(mockCurrentUserService.Object, mockProviderService.Object);

            // Act.
            Action action = () => service.Authorise(subContractorProviderId, providerSiteId);

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
            var providerSiteId = new Random().Next(101, 200);

            var mockCurrentUserService = new Mock<ICurrentUserService>();
            var mockProviderService = new Mock<IProviderService>();

            mockCurrentUserService.Setup(mock =>
                mock.IsInRole(Roles.Raa))
                .Returns(true);

            var service = new ProviderVacancyAuthorisationService(mockCurrentUserService.Object, mockProviderService.Object);

            // Act.
            Action action = () => service.Authorise(providerId, providerSiteId);

            // Assert.
            action.ShouldNotThrow();
        }
    }
}
