using System;

namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Providers.VacancyPosting
{
    using System.Web.Mvc;
    using Common.ViewModels.Locations;
    using Domain.Entities.Locations;
    using Domain.Entities.Organisations;
    using Domain.Entities.Providers;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using ViewModels.Provider;
    using ViewModels.Vacancy;
    using System.Collections.Generic;

    [TestFixture]
    public class CreateVacancyTests : TestBase
    {
        private readonly NewVacancyViewModel _validNewVacancyViewModelWithReferenceNumber = new NewVacancyViewModel()
        {
            VacancyReferenceNumber = 1,
            ApprenticeshipLevel = ApprenticeshipLevel.Advanced
        };

        private readonly ApprenticeshipVacancy _existingApprenticeshipVacancy = new ApprenticeshipVacancy()
        {
            ProviderSiteEmployerLink = new ProviderSiteEmployerLink()
            {
                Employer = new Employer()
                {
                    Address = new Address()
                }
            }
        };

        private readonly NewVacancyViewModel _validNewVacancyViewModelSansReferenceNumber = new NewVacancyViewModel
        {
            SectorsAndFrameworks = new List<SelectListItem>(),
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
            var provider = GetVacancyPostingProvider();

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
            var provider = GetVacancyPostingProvider();

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

            var provider = GetVacancyPostingProvider();

            const bool offlineVacancy = true;
            const string offlineApplicationUrl = "a_url.com";
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
            && v.OfflineApplicationUrl.StartsWith("http://") && v.OfflineApplicationInstructions == offlineApplicationInstructions)));
        }

        [Test]
        public void ShouldUseVacancyGuidExistingInTheViewModel()
        {
            var vacancyGuid = Guid.NewGuid();
            MockVacancyPostingService.Setup(s => s.GetNextVacancyReferenceNumber()).Returns(1);

            var provider = GetVacancyPostingProvider();

            provider.CreateVacancy(new NewVacancyViewModel
            {
                VacancyGuid = vacancyGuid,
                ProviderSiteEmployerLink = new ProviderSiteEmployerLinkViewModel {Employer = new EmployerViewModel()}
            });

            MockVacancyPostingService.Verify(s => s.SaveApprenticeshipVacancy(It.Is<ApprenticeshipVacancy>(v => v.EntityId == vacancyGuid)));
        }

        [Test]
        public void ShouldReturnAnExistingVacancyIfVacancyGuidExists()
        {
            // Arrange
            var vacancyGuid = Guid.NewGuid();
            var ern = "ern";
            var ukprn = "ukprn";
            var providerSiteErn = "providerSiteErn";
            var av = new ApprenticeshipVacancy
            {
                Title = "Title",
                ShortDescription = "shorts",
                FrameworkCodeName = "fwcn",
                StandardId = 1234,
                OfflineVacancy = true,
                OfflineApplicationUrl = "http://www.google.com",
                OfflineApplicationInstructions = "optional",
                ApprenticeshipLevel = ApprenticeshipLevel.Advanced,
                ProviderSiteEmployerLink = new ProviderSiteEmployerLink
                {
                    Employer = new Employer
                    {
                        Address = new Address()
                    }
                }
            };

            MockVacancyPostingService.Setup(s => s.GetVacancy(vacancyGuid)).Returns(av);
            var provider = GetVacancyPostingProvider();

            // Act
            var result = provider.GetNewVacancyViewModel(ukprn, providerSiteErn, ern, vacancyGuid);

            // Assert
            MockVacancyPostingService.Verify(s => s.GetVacancy(vacancyGuid), Times.Once);
            MockProviderService.Verify(s => s.GetProviderSiteEmployerLink(providerSiteErn, ern), Times.Never);
            result.Should()
                .Match<NewVacancyViewModel>(
                    r =>
                        r.Title == av.Title && r.ShortDescription == av.ShortDescription &&
                        r.FrameworkCodeName == av.FrameworkCodeName && r.StandardId == av.StandardId && r.OfflineVacancy == av.OfflineVacancy &&
                        r.OfflineApplicationInstructions == av.OfflineApplicationInstructions &&
                        r.OfflineApplicationUrl == av.OfflineApplicationUrl && r.ApprenticeshipLevel == av.ApprenticeshipLevel);
        }

        [Test]
        public void ShouldReturnANewVacancyIfVacancyGuidDoesNotExists()
        {
            // Arrange
            var vacancyGuid = Guid.NewGuid();
            var ern = "ern";
            var ukprn = "ukprn";
            var providerSiteErn = "providerSiteErn";
            ApprenticeshipVacancy apprenticeshipVacancy = null;
            var providerSiteEmployerLink = new ProviderSiteEmployerLink
            {
                ProviderSiteErn = providerSiteErn,
                Description = "description",
                Employer = new Employer
                {
                    Address = new Address()
                }
            };

            MockVacancyPostingService.Setup(s => s.GetVacancy(vacancyGuid)).Returns(apprenticeshipVacancy);
            var provider = GetVacancyPostingProvider();
            MockProviderService.Setup(s => s.GetProviderSiteEmployerLink(providerSiteErn, ern))
                .Returns(providerSiteEmployerLink);

            // Act
            var result = provider.GetNewVacancyViewModel(ukprn, providerSiteErn, ern, vacancyGuid);

            // Assert
            MockVacancyPostingService.Verify(s => s.GetVacancy(vacancyGuid), Times.Once);
            MockProviderService.Verify(s => s.GetProviderSiteEmployerLink(providerSiteErn, ern), Times.Once);
            result.Should()
                .Match<NewVacancyViewModel>(
                    r =>
                        r.Ukprn == ukprn && r.ApprenticeshipLevel == ApprenticeshipLevel.Unknown &&
                        r.ProviderSiteEmployerLink.Description == providerSiteEmployerLink.Description &&
                        r.ProviderSiteEmployerLink.ProviderSiteErn == providerSiteErn);
        }
    }
}