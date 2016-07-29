namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.ProviderProvider
{
    using System;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Vacancies;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
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
            var edsErn = "232";
            var vacancyParty = new VacancyParty
            {
                VacancyPartyId = vacancyPartyId,
                ProviderSiteId = providerSiteId,
                EmployerId = employerId,
                EmployerDescription = "Description",
                EmployerWebsiteUrl = "Url"
            };

            MockVacancyPostingService.Setup(s => s.GetVacancy(vacancyGuid)).Returns(apprenticeshipVacancy);
            MockProviderService.Setup(s => s.SaveVacancyParty(vacancyParty))
                .Returns(vacancyParty);
            MockProviderService.Setup(s => s.GetVacancyParty(providerSiteId, edsErn))
                .Returns(vacancyParty);
            MockEmployerService.Setup(s => s.GetEmployer(employerId))
                .Returns(new Fixture().Build<Employer>().With(e => e.EmployerId, employerId).Create());

            var provider = GetProviderProvider();

            // Act
            provider.ConfirmVacancyParty(new VacancyPartyViewModel
            {
                ProviderSiteId = providerSiteId,
                Employer = new EmployerViewModel
                {
                    EmployerId = employerId,
                    EdsUrn = edsErn,
                    Address = new AddressViewModel()
                },
                VacancyGuid = vacancyGuid,
                IsEmployerLocationMainApprenticeshipLocation = true,
                NumberOfPositions = 4
            });

            // Assert
            MockVacancyPostingService.Verify(s => s.GetVacancy(vacancyGuid), Times.Once);
            MockProviderService.Verify(s => s.SaveVacancyParty(vacancyParty), Times.Once);
            MockVacancyPostingService.Verify(
                s =>
                    s.UpdateVacancy(
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
            var edsErn = "232";
            var providerSiteEmployerLink = new VacancyParty
            {
                VacancyPartyId = vacancyPartyId,
                ProviderSiteId = providerSiteId,
                EmployerId = employerId,
                EmployerDescription = "Description",
                EmployerWebsiteUrl = "Url"
            };

            MockVacancyPostingService.Setup(s => s.GetVacancy(vacancyGuid)).Returns(nullApprenticeshipVacancy);
            MockProviderService.Setup(s => s.SaveVacancyParty(providerSiteEmployerLink))
                .Returns(providerSiteEmployerLink);
            MockProviderService.Setup(s => s.GetVacancyParty(providerSiteId, edsErn))
                .Returns(providerSiteEmployerLink);
            MockEmployerService.Setup(s => s.GetEmployer(employerId))
                .Returns(new Fixture().Build<Employer>().With(e => e.EmployerId, employerId).Create());

            var provider = GetProviderProvider();

            // Act
            provider.ConfirmVacancyParty(new VacancyPartyViewModel
            {
                ProviderSiteId = providerSiteId,
                Employer = new EmployerViewModel
                {
                    EmployerId = employerId,
                    EdsUrn = edsErn,
                    Address = new AddressViewModel()
                },
                VacancyGuid = vacancyGuid
            });

            // Assert
            MockVacancyPostingService.Verify(s => s.GetVacancy(vacancyGuid), Times.Once);
            MockProviderService.Verify(s => s.SaveVacancyParty(providerSiteEmployerLink), Times.Once);
            MockVacancyPostingService.Verify(
                s =>
                    s.CreateVacancy(
                        It.Is<Vacancy>(v => v.OwnerPartyId == vacancyPartyId)), Times.Never);
        }
    }
}
