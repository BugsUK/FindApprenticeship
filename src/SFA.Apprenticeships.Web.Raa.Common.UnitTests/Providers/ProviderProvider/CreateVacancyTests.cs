namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.ProviderProvider
{
    using System;
    using Domain.Entities.Locations;
    using Domain.Entities.Organisations;
    using Domain.Entities.Providers;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Moq;
    using NUnit.Framework;
    using ViewModels.Provider;
    using ViewModels.Vacancy;
    using Web.Common.ViewModels.Locations;

    [TestFixture]
    public class CreateVacancyTests : TestBase
    {
        [Test]
        public void ShouldUpdateVacancyProviderSiteEmployerLinkIfTheVacancyAlreadyExists()
        {
            // Arrange
            var vacancyGuid = Guid.NewGuid();
            var apprenticeshipVacancy = new ApprenticeshipVacancy();
            var providerSiteErn = "providerSiteErn";
            var ern = "ern";
            var providerSiteEmployerLink = new ProviderSiteEmployerLink
            {
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Employer = new Employer
                {
                    Ern = ern,
                    Address = new PostalAddress()
                },
                Description = "Description",
                EntityId = 1,
                ProviderSiteErn = providerSiteErn,
                WebsiteUrl = "Url"
            };

            MockVacancyPostingService.Setup(s => s.GetVacancy(vacancyGuid)).Returns(apprenticeshipVacancy);
            MockProviderService.Setup(s => s.SaveProviderSiteEmployerLink(providerSiteEmployerLink))
                .Returns(providerSiteEmployerLink);
            MockProviderService.Setup(s => s.GetProviderSiteEmployerLink(providerSiteErn, ern))
                .Returns(providerSiteEmployerLink);

            var provider = GetProvider();

            // Act
            provider.ConfirmProviderSiteEmployerLink(new ProviderSiteEmployerLinkViewModel
            {
                ProviderSiteErn = providerSiteErn,
                Employer = new EmployerViewModel
                {
                    Ern = ern,
                    Address = new AddressViewModel()
                },
                VacancyGuid = vacancyGuid,
                IsEmployerLocationMainApprenticeshipLocation = true,
                NumberOfPositions = 4
            });

            // Assert
            MockVacancyPostingService.Verify(s => s.GetVacancy(vacancyGuid), Times.Once);
            MockProviderService.Verify(s => s.SaveProviderSiteEmployerLink(providerSiteEmployerLink), Times.Once);
            MockVacancyPostingService.Verify(
                s =>
                    s.SaveApprenticeshipVacancy(
                        It.Is<ApprenticeshipVacancy>(v => v.ProviderSiteEmployerLink == providerSiteEmployerLink)), Times.Once);


        }

        [Test]
        public void ShouldNotUpdateVacancyProviderSiteEmployerLinkIfTheVacancyDoesNotExist()
        {
            // Arrange
            var vacancyGuid = Guid.NewGuid();
            ApprenticeshipVacancy nullApprenticeshipVacancy = null;
            var providerSiteErn = "providerSiteErn";
            var ern = "ern";
            var providerSiteEmployerLink = new ProviderSiteEmployerLink
            {
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Employer = new Employer
                {
                    Ern = ern,
                    Address = new PostalAddress()
                },
                Description = "Description",
                EntityId = 1,
                ProviderSiteErn = providerSiteErn,
                WebsiteUrl = "Url"
            };

            MockVacancyPostingService.Setup(s => s.GetVacancy(vacancyGuid)).Returns(nullApprenticeshipVacancy);
            MockProviderService.Setup(s => s.SaveProviderSiteEmployerLink(providerSiteEmployerLink))
                .Returns(providerSiteEmployerLink);
            MockProviderService.Setup(s => s.GetProviderSiteEmployerLink(providerSiteErn, ern))
                .Returns(providerSiteEmployerLink);

            var provider = GetProvider();

            // Act
            provider.ConfirmProviderSiteEmployerLink(new ProviderSiteEmployerLinkViewModel
            {
                ProviderSiteErn = providerSiteErn,
                Employer = new EmployerViewModel
                {
                    Ern = ern,
                    Address = new AddressViewModel()
                },
                VacancyGuid = vacancyGuid
            });

            // Assert
            MockVacancyPostingService.Verify(s => s.GetVacancy(vacancyGuid), Times.Once);
            MockProviderService.Verify(s => s.SaveProviderSiteEmployerLink(providerSiteEmployerLink), Times.Once);
            MockVacancyPostingService.Verify(
                s =>
                    s.SaveApprenticeshipVacancy(
                        It.Is<ApprenticeshipVacancy>(v => v.ProviderSiteEmployerLink == providerSiteEmployerLink)), Times.Never);


        }
    }
}
