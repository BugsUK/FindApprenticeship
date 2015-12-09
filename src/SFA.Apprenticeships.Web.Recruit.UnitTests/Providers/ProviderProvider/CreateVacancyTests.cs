using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.Apprenticeships.Domain.Entities.Locations;
using SFA.Apprenticeships.Domain.Entities.Organisations;
using SFA.Apprenticeships.Domain.Entities.Providers;
using SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
using SFA.Apprenticeships.Web.Common.ViewModels.Locations;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy;

namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Providers.ProviderProvider
{
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
                    Address = new Address()
                },
                Description = "Description",
                EntityId = Guid.NewGuid(),
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
                    Address = new Address()
                },
                Description = "Description",
                EntityId = Guid.NewGuid(),
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
