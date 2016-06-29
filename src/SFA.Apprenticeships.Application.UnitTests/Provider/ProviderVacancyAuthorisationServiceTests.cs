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
    using Ploeh.AutoFixture;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    public class ProviderVacancyAuthorisationServiceTests
    {
        [Test]
        public void ShouldAuthoriseProviderUserForVacancyForOwnProvider()
        {
            // Arrange.
            var ukprn = Convert.ToString(new Random().Next(1, 100));
            var providerSiteId = new Random().Next(101, 200);

            var provider = new Fixture().Create<Provider>();

            var mockCurrentUserService = new Mock<ICurrentUserService>();
            var mockProviderService = new Mock<IProviderService>();

            mockCurrentUserService.Setup(mock =>
                mock.IsInRole(Roles.Faa))
                .Returns(true);

            mockCurrentUserService.Setup(mock =>
                mock.GetClaimValue("ukprn"))
                .Returns(ukprn);

            mockProviderService.Setup(mock => mock
                .GetProvider(ukprn))
                .Returns(provider);

            var service = new ProviderVacancyAuthorisationService(mockCurrentUserService.Object, mockProviderService.Object);

            // Act.
            Action action = () => service.Authorise(provider.ProviderId, providerSiteId);

            // Assert.
            action.ShouldNotThrow();
        }

        [Test]
        public void ShouldNotAuthoriseProviderUserForVacancyForOtherProvider()
        {
            // Arrange.
            var ukprn = Convert.ToString(new Random().Next(1, 100));
            var providerSiteId = new Random().Next(101, 200);
            const string userName = "john.provider@example.com";

            var provider = new Fixture().Create<Provider>();
            var otherProviderId = provider.ProviderId + 1;

            var mockCurrentUserService = new Mock<ICurrentUserService>();
            var mockProviderService = new Mock<IProviderService>();

            mockCurrentUserService.Setup(mock =>
                mock.CurrentUserName)
                .Returns(userName);

            mockCurrentUserService.Setup(mock =>
                mock.IsInRole(Roles.Faa))
                .Returns(true);

            mockCurrentUserService.Setup(mock =>
                mock.GetClaimValue("ukprn"))
                .Returns(ukprn);

            mockProviderService.Setup(mock => mock
                .GetProvider(ukprn))
                .Returns(provider);

            var service = new ProviderVacancyAuthorisationService(mockCurrentUserService.Object, mockProviderService.Object);

            // Act.
            Action action = () => service.Authorise(otherProviderId, providerSiteId);

            // Assert.
            action
                .ShouldThrow<CustomException>()
                .Which.Code.Should().Be(Interfaces.ErrorCodes.ProviderVacancyAuthorisation.Failed);
        }

        [Test]
        public void ShouldAuthoriseProviderUserForVacancyForOwnProviderSite()
        {
            // Arrange.
            var ukprn = Convert.ToString(new Random().Next(1, 50));
            var subContractorProviderId = new Random().Next(51, 100);

            var providerSiteId = new Random().Next(101, 200);

            var provider = new Fixture().Create<Provider>();

            var mockCurrentUserService = new Mock<ICurrentUserService>();
            var mockProviderService = new Mock<IProviderService>();

            mockCurrentUserService.Setup(mock =>
                mock.IsInRole(Roles.Faa))
                .Returns(true);

            mockCurrentUserService.Setup(mock =>
                mock.GetClaimValue("ukprn"))
                .Returns(ukprn);

            mockProviderService.Setup(mock => mock
                .GetProvider(ukprn))
                .Returns(provider);

            mockProviderService.Setup(mock => mock
                .GetProviderSites(ukprn))
                .Returns(new List<ProviderSite>
                {
                    new ProviderSite
                    {
                        ProviderSiteId = providerSiteId
                    }
                });

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
            var ukprn = Convert.ToString(new Random().Next(1, 50));

            var providerSiteId = new Random().Next(101, 200);
            var otherProviderSiteId = providerSiteId + 1;

            var provider = new Fixture().Create<Provider>();
            var otherProviderId = provider.ProviderId + 1;

            var mockCurrentUserService = new Mock<ICurrentUserService>();
            var mockProviderService = new Mock<IProviderService>();

            mockCurrentUserService.Setup(mock =>
                mock.IsInRole(Roles.Faa))
                .Returns(true);

            mockCurrentUserService.Setup(mock =>
                mock.GetClaimValue("ukprn"))
                .Returns(ukprn);

            mockProviderService.Setup(mock => mock
                .GetProvider(ukprn))
                .Returns(provider);

            mockProviderService.Setup(mock => mock
                .GetProviderSites(ukprn))
                .Returns(new List<ProviderSite>
                {
                    new ProviderSite
                    {
                        ProviderSiteId = otherProviderSiteId
                    }
                });

            var service = new ProviderVacancyAuthorisationService(mockCurrentUserService.Object, mockProviderService.Object);

            // Act.
            Action action = () => service.Authorise(otherProviderId, providerSiteId);

            // Assert.
            action
                .ShouldThrow<CustomException>()
                .Which.Code.Should().Be(Interfaces.ErrorCodes.ProviderVacancyAuthorisation.Failed);
        }

        [Test]
        public void ShouldNotAuthoriseProviderWithInvalidUkprn()
        {
            // Arrange.
            var ukprn = Convert.ToString(new Random().Next(1, 50));

            var mockCurrentUserService = new Mock<ICurrentUserService>();
            var mockProviderService = new Mock<IProviderService>();

            mockCurrentUserService.Setup(mock =>
                mock.IsInRole(Roles.Faa))
                .Returns(true);

            mockCurrentUserService.Setup(mock =>
                mock.GetClaimValue("ukprn"))
                .Returns(ukprn);

            mockProviderService.Setup(mock => mock
                .GetProvider(ukprn))
                .Returns(default(Provider));

            var service = new ProviderVacancyAuthorisationService(mockCurrentUserService.Object, mockProviderService.Object);

            // Act.
            Action action = () => service.Authorise(1, 2);

            // Assert.
            action
                .ShouldThrow<CustomException>()
                .Which.Code.Should().Be(Interfaces.ErrorCodes.ProviderVacancyAuthorisation.InvalidUkprn);
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
