namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.ProviderProvider
{
    using System;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Vacancies;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using ViewModels.Employer;
    using ViewModels.Provider;
    using ViewModels.Vacancy;
    using Web.Common.ViewModels.Locations;

    [TestFixture]
    [Parallelizable]
    public class CreateVacancyTests : TestBase
    {
        [Test]
        public void ShouldUpdateVacancyProviderSiteEmployerLinkIfTheVacancyAlreadyExists()
        {
            // Arrange
            var vacancyGuid = Guid.NewGuid();
            var apprenticeshipVacancy = new Vacancy();
            var vacancyOwnerRelationshipId = 42;
            var providerSiteId = 1;
            var employerId = 2;
            var edsErn = "232";
            var vacancyOwnerRelationship = new VacancyOwnerRelationship
            {
                VacancyOwnerRelationshipId = vacancyOwnerRelationshipId,
                ProviderSiteId = providerSiteId,
                EmployerId = employerId,
                EmployerDescription = "Description",
                EmployerWebsiteUrl = "Url"
            };

            MockVacancyPostingService.Setup(s => s.GetVacancy(vacancyGuid)).Returns(apprenticeshipVacancy);
            MockProviderService.Setup(s => s.SaveVacancyOwnerRelationship(vacancyOwnerRelationship))
                .Returns(vacancyOwnerRelationship);
            MockProviderService.Setup(s => s.GetVacancyOwnerRelationship(providerSiteId, edsErn))
                .Returns(vacancyOwnerRelationship);
            MockEmployerService.Setup(s => s.GetEmployer(employerId, It.IsAny<bool>()))
                .Returns(new Fixture().Build<Employer>().With(e => e.EmployerId, employerId).Create());

            var provider = GetProviderProvider();

            // Act
            provider.ConfirmVacancyOwnerRelationship(new VacancyOwnerRelationshipViewModel
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
            MockProviderService.Verify(s => s.SaveVacancyOwnerRelationship(vacancyOwnerRelationship), Times.Once);
            MockVacancyPostingService.Verify(
                s =>
                    s.UpdateVacancy(
                        It.Is<Vacancy>(v => v.VacancyOwnerRelationshipId == vacancyOwnerRelationshipId)), Times.Once);


        }

        [Test]
        public void ShouldNotUpdateVacancyProviderSiteEmployerLinkIfTheVacancyDoesNotExist()
        {
            // Arrange
            var vacancyGuid = Guid.NewGuid();
            Vacancy nullApprenticeshipVacancy = null;
            var vacancyOwnerRelationshipId = 42;
            var providerSiteId = 1;
            var employerId = 2;
            var edsErn = "232";
            var providerSiteEmployerLink = new VacancyOwnerRelationship
            {
                VacancyOwnerRelationshipId = vacancyOwnerRelationshipId,
                ProviderSiteId = providerSiteId,
                EmployerId = employerId,
                EmployerDescription = "Description",
                EmployerWebsiteUrl = "Url"
            };

            MockVacancyPostingService.Setup(s => s.GetVacancy(vacancyGuid)).Returns(nullApprenticeshipVacancy);
            MockProviderService.Setup(s => s.SaveVacancyOwnerRelationship(providerSiteEmployerLink))
                .Returns(providerSiteEmployerLink);
            MockProviderService.Setup(s => s.GetVacancyOwnerRelationship(providerSiteId, edsErn))
                .Returns(providerSiteEmployerLink);
            MockEmployerService.Setup(s => s.GetEmployer(employerId, It.IsAny<bool>()))
                .Returns(new Fixture().Build<Employer>().With(e => e.EmployerId, employerId).Create());

            var provider = GetProviderProvider();

            // Act
            provider.ConfirmVacancyOwnerRelationship(new VacancyOwnerRelationshipViewModel
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
            MockProviderService.Verify(s => s.SaveVacancyOwnerRelationship(providerSiteEmployerLink), Times.Once);
            MockVacancyPostingService.Verify(
                s =>
                    s.CreateVacancy(
                        It.Is<Vacancy>(v => v.VacancyOwnerRelationshipId == vacancyOwnerRelationshipId)), Times.Never);
        }

        [Test]
        public void ShouldSetVacancyOwnerRelationshipToLiveIfItsDeleted()
        {
            // Arrange
            var vacancyGuid = Guid.NewGuid();
            var apprenticeshipVacancy = new Vacancy();
            var vacancyOwnerRelationshipId = 42;
            var providerSiteId = 1;
            var employerId = 2;
            var edsErn = "232";
            var vacancyOwnerRelationship = new VacancyOwnerRelationship
            {
                VacancyOwnerRelationshipId = vacancyOwnerRelationshipId,
                ProviderSiteId = providerSiteId,
                EmployerId = employerId,
                EmployerDescription = "Description",
                EmployerWebsiteUrl = "Url"
            };

            MockVacancyPostingService.Setup(s => s.GetVacancy(vacancyGuid)).Returns(apprenticeshipVacancy);
            MockProviderService.Setup(s => s.SaveVacancyOwnerRelationship(vacancyOwnerRelationship))
                .Returns(vacancyOwnerRelationship);
            MockProviderService.Setup(s => s.GetVacancyOwnerRelationship(providerSiteId, edsErn))
                .Returns(vacancyOwnerRelationship);
            MockEmployerService.Setup(s => s.GetEmployer(employerId, It.IsAny<bool>()))
                .Returns(new Fixture().Build<Employer>().With(e => e.EmployerId, employerId).Create());
            MockProviderService.Setup(s => s.IsADeletedVacancyOwnerRelationship(providerSiteId, edsErn)).Returns(true);

            var provider = GetProviderProvider();

            // Act
            provider.ConfirmVacancyOwnerRelationship(new VacancyOwnerRelationshipViewModel
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
            MockProviderService.Verify(s => s.ResurrectVacancyOwnerRelationship(providerSiteId, edsErn));


        }
    }
}
