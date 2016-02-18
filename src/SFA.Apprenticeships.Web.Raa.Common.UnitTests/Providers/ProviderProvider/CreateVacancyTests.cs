namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.ProviderProvider
{
    using System;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Vacancies;
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
            var apprenticeshipVacancy = new Vacancy();
            var vacancyPartyId = 42;
            var providerSiteId = 1;
            var employerId = 2;
            var providerSiteEmployerLink = new VacancyParty
            {
                VacancyPartyId = vacancyPartyId,
                CreatedDateTime = DateTime.Now,
                UpdatedDateTime = DateTime.Now,
                ProviderSiteId = providerSiteId,
                EmployerId = employerId,
                EmployerDescription = "Description",
                EmployerWebsiteUrl = "Url"
            };

            MockVacancyPostingService.Setup(s => s.GetVacancy(vacancyGuid)).Returns(apprenticeshipVacancy);
            MockProviderService.Setup(s => s.SaveVacancyParty(providerSiteEmployerLink))
                .Returns(providerSiteEmployerLink);
            MockProviderService.Setup(s => s.GetVacancyParty(providerSiteId, employerId))
                .Returns(providerSiteEmployerLink);

            var provider = GetProvider();

            // Act
            provider.ConfirmProviderSiteEmployerLink(new ProviderSiteEmployerLinkViewModel
            {
                ProviderSiteId = providerSiteId,
                Employer = new EmployerViewModel
                {
                    EmployerId = employerId,
                    Address = new AddressViewModel()
                },
                VacancyGuid = vacancyGuid,
                IsEmployerLocationMainApprenticeshipLocation = true,
                NumberOfPositions = 4
            });

            // Assert
            MockVacancyPostingService.Verify(s => s.GetVacancy(vacancyGuid), Times.Once);
            MockProviderService.Verify(s => s.SaveVacancyParty(providerSiteEmployerLink), Times.Once);
            MockVacancyPostingService.Verify(
                s =>
                    s.SaveApprenticeshipVacancy(
                        It.Is<Vacancy>(v => v.OwnerPartyId == vacancyPartyId)), Times.Once);


        }

        [Test]
        public void ShouldNotUpdateVacancyProviderSiteEmployerLinkIfTheVacancyDoesNotExist()
        {
            // Arrange
            var vacancyGuid = Guid.NewGuid();
            Vacancy nullApprenticeshipVacancy = null;
            var vacancyPartyId = 42;
            var providerSiteId = 1;
            var employerId = 2;
            var providerSiteEmployerLink = new VacancyParty
            {
                VacancyPartyId = vacancyPartyId,
                CreatedDateTime = DateTime.Now,
                UpdatedDateTime = DateTime.Now,
                ProviderSiteId = providerSiteId,
                EmployerId = employerId,
                EmployerDescription = "Description",
                EmployerWebsiteUrl = "Url"
            };

            MockVacancyPostingService.Setup(s => s.GetVacancy(vacancyGuid)).Returns(nullApprenticeshipVacancy);
            MockProviderService.Setup(s => s.SaveVacancyParty(providerSiteEmployerLink))
                .Returns(providerSiteEmployerLink);
            MockProviderService.Setup(s => s.GetVacancyParty(providerSiteId, employerId))
                .Returns(providerSiteEmployerLink);

            var provider = GetProvider();

            // Act
            provider.ConfirmProviderSiteEmployerLink(new ProviderSiteEmployerLinkViewModel
            {
                ProviderSiteId = providerSiteId,
                Employer = new EmployerViewModel
                {
                    EmployerId = employerId,
                    Address = new AddressViewModel()
                },
                VacancyGuid = vacancyGuid
            });

            // Assert
            MockVacancyPostingService.Verify(s => s.GetVacancy(vacancyGuid), Times.Once);
            MockProviderService.Verify(s => s.SaveVacancyParty(providerSiteEmployerLink), Times.Once);
            MockVacancyPostingService.Verify(
                s =>
                    s.SaveApprenticeshipVacancy(
                        It.Is<Vacancy>(v => v.OwnerPartyId == vacancyPartyId)), Times.Never);


        }
    }
}
