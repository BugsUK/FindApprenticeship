using System.Collections.Generic;
using FluentAssertions;
using SFA.Apprenticeships.Domain.Entities.Locations;
using SFA.Apprenticeships.Domain.Entities.Organisations;
using SFA.Apprenticeships.Domain.Entities.Providers;
using SFA.Apprenticeships.Web.Common.Configuration;
using SFA.Apprenticeships.Web.Common.ViewModels.Locations;
using SFA.Apprenticeships.Web.Recruit.Constants.ViewModels;
using SFA.Apprenticeships.Web.Recruit.ViewModels.Frameworks;

namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Providers.VacancyPosting
{
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Moq;
    using NUnit.Framework;
    using ViewModels.Provider;
    using ViewModels.Vacancy;

    [TestFixture]
    public class CreateVacancyTests : TestBase
    {
        private NewVacancyViewModel _validNewVacancyViewModelWithReferenceNumber = new NewVacancyViewModel()
        {
            VacancyReferenceNumber = 1,
            ApprenticeshipLevel = ApprenticeshipLevel.Advanced
        };

        private ApprenticeshipVacancy _existingApprenticeshipVacancy = new ApprenticeshipVacancy()
        {
            ProviderSiteEmployerLink = new ProviderSiteEmployerLink()
            {
                Employer = new Employer()
                {
                    Address = new Address()
                }
            }
        };

        private NewVacancyViewModel _validNewVacancyViewModelSansReferenceNumber = new NewVacancyViewModel
        {
            SectorsAndFrameworks = new List<SectorSelectItemViewModel>(),
            ProviderSiteEmployerLink = new ProviderSiteEmployerLinkViewModel()
            {
                Employer = new EmployerViewModel()
                {
                    Address = new AddressViewModel()
                }
            }
        };

        [SetUp]
        public void SetUp()
        {
            //MockConfigurationService
            //    .Setup(mock => mock.Get<CommonWebConfiguration>())
            //    .Returns(_webConfiguration);

            //MockProviderService
            //    .Setup(mock => mock.GetProviderSiteEmployerLink(ProviderSiteUrn, EmployerFilterViewModelMessages.Ern))
            //    .Returns(ProviderSiteEmployerLink);

            //MockReferenceDataService
            //    .Setup(mock => mock.GetCategories())
            //    .Returns(_categories);

            MockVacancyPostingService.Setup(mock => mock.GetVacancy(_validNewVacancyViewModelWithReferenceNumber.VacancyReferenceNumber.Value))
                .Returns(_existingApprenticeshipVacancy);
            MockVacancyPostingService.Setup(mock => mock.SaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()))
                .Returns(_existingApprenticeshipVacancy);
        }

        [Test]
        public void ShouldUpdateIfVacancyReferenceIsPresent()
        {
            // Arrange.
            var provider = GetProvider();

            // Act.
            var viewModel = provider.CreateVacancy(_validNewVacancyViewModelWithReferenceNumber);

            // Assert.
            MockVacancyPostingService.Verify(mock =>
                mock.GetVacancy(_validNewVacancyViewModelWithReferenceNumber.VacancyReferenceNumber.Value), Times.Once);
            MockVacancyPostingService.Verify(mock => mock.GetNextVacancyReferenceNumber(), Times.Never);
            MockVacancyPostingService.Verify(mock =>
                mock.SaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()), Times.Once);

            viewModel.VacancyReferenceNumber.Should().HaveValue();
        }

        [Test]
        public void ShouldCreateNewIfVacancyReferenceIsNotPresent()
        {
            // Arrange.
            var provider = GetProvider();

            // Act.
            var viewModel = provider.CreateVacancy(_validNewVacancyViewModelSansReferenceNumber);

            // Assert.
            MockVacancyPostingService.Verify(mock =>
                mock.GetVacancy(It.IsAny<long>()), Times.Never);
            MockVacancyPostingService.Verify(mock => mock.GetNextVacancyReferenceNumber(), Times.Once);
            MockVacancyPostingService.Verify(mock =>
                mock.SaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()), Times.Once);

            viewModel.VacancyReferenceNumber.Should().HaveValue();
        }

        [Test]
        public void ShouldStoreOfflineApplicationFields()
        {
            MockVacancyPostingService.Setup(s => s.SaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()))
                .Returns(new ApprenticeshipVacancy());

            var provider = GetProvider();

            const bool offlineVacancy = true;
            const string offlineApplicationUrl = "A url";
            const string offlineApplicationInstructions = "Some instructions";

            provider.CreateVacancy(new NewVacancyViewModel
            {
                ProviderSiteEmployerLink = new ProviderSiteEmployerLinkViewModel
                {
                  Employer  = new EmployerViewModel()
                },
                OfflineVacancy = offlineVacancy,
                OfflineApplicationUrl = offlineApplicationUrl,
                OfflineApplicationInstructions = offlineApplicationInstructions,
                ApprenticeshipLevel = ApprenticeshipLevel.Higher
            });

            MockVacancyPostingService.Verify(s => s.SaveApprenticeshipVacancy(It.Is<ApprenticeshipVacancy>(v => v.OfflineVacancy == offlineVacancy 
            && v.OfflineApplicationUrl == offlineApplicationUrl && v.OfflineApplicationInstructions == offlineApplicationInstructions)));
        }
    }
}