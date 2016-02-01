namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyPosting
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Locations;
    using Domain.Entities.Organisations;
    using Domain.Entities.Providers;
    using Domain.Entities.ReferenceData;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using ViewModels.Provider;
    using ViewModels.Vacancy;
    using ViewModels.VacancyPosting;
    using Web.Common.ViewModels.Locations;

    [TestFixture]
    public class CreateVacancyTests : TestBase
    {
        private const string Ern = "ern";
        private const string Ukprn = "ukprn";
        private const string ProviderSiteErn = "providerSiteErn";

        private NewVacancyViewModel _validNewVacancyViewModelWithReferenceNumber;
        private NewVacancyViewModel _validNewVacancyViewModelSansReferenceNumber;

        private readonly ApprenticeshipVacancy _existingApprenticeshipVacancy = new ApprenticeshipVacancy()
        {
            ProviderSiteEmployerLink = new ProviderSiteEmployerLink()
            {
                Employer = new Employer()
                {
                    Address = new PostalAddress()
                }
            }
        };

        private readonly ProviderSiteEmployerLink _providerSiteEmployerLink = new ProviderSiteEmployerLink
        {
            ProviderSiteErn = ProviderSiteErn,
            Description = "description",
            Employer = new Employer
            {
                Address = new PostalAddress()
            }
        };

        [SetUp]
        public void SetUp()
        {
            _validNewVacancyViewModelWithReferenceNumber = new NewVacancyViewModel()
            {
                VacancyReferenceNumber = 1,
                OfflineVacancy = false,
            };

            _validNewVacancyViewModelSansReferenceNumber = new NewVacancyViewModel
            {
                ProviderSiteEmployerLink = new ProviderSiteEmployerLinkViewModel()
                {
                    ProviderSiteErn = ProviderSiteErn,
                    Employer = new EmployerViewModel
                    {
                        Ern = Ern,
                        Address = new AddressViewModel()
                    }
                },
                OfflineVacancy = false,
            };

            MockVacancyPostingService.Setup(mock => mock.GetVacancy(_validNewVacancyViewModelWithReferenceNumber.VacancyReferenceNumber.Value))
                .Returns(_existingApprenticeshipVacancy);
            MockVacancyPostingService.Setup(mock => mock.CreateApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()))
                .Returns<ApprenticeshipVacancy>(v => v);
            MockVacancyPostingService.Setup(mock => mock.SaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()))
                .Returns<ApprenticeshipVacancy>(v => v);
            MockVacancyPostingService.Setup(mock => mock.ShallowSaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()))
                .Returns<ApprenticeshipVacancy>(v => v);
            MockReferenceDataService.Setup(mock => mock.GetSectors())
                .Returns(new List<Sector>
                {
                    new Sector
                    {
                        Id = 1,
                        Standards =
                            new List<Standard>
                            {
                                new Standard {Id = 1, ApprenticeshipSectorId = 1, ApprenticeshipLevel = ApprenticeshipLevel.Intermediate},
                                new Standard {Id = 2, ApprenticeshipSectorId = 1, ApprenticeshipLevel = ApprenticeshipLevel.Advanced},
                                new Standard {Id = 3, ApprenticeshipSectorId = 1, ApprenticeshipLevel = ApprenticeshipLevel.Higher},
                                new Standard {Id = 4, ApprenticeshipSectorId = 1, ApprenticeshipLevel = ApprenticeshipLevel.FoundationDegree},
                                new Standard {Id = 5, ApprenticeshipSectorId = 1, ApprenticeshipLevel = ApprenticeshipLevel.Degree},
                                new Standard {Id = 6, ApprenticeshipSectorId = 1, ApprenticeshipLevel = ApprenticeshipLevel.Masters}
                            }
                    }
                });
            MockProviderService.Setup(s => s.GetProviderSiteEmployerLink(ProviderSiteErn, Ern))
                .Returns(_providerSiteEmployerLink);
        }

        [Test]
        public void ShouldUpdateIfVacancyReferenceIsPresent()
        {
            // Arrange.
            var vvm = new Fixture().Build<NewVacancyViewModel>().Create();
            MockMapper.Setup(m => m.Map<ApprenticeshipVacancy, NewVacancyViewModel>(It.IsAny<ApprenticeshipVacancy>())).Returns(vvm);
            var provider = GetVacancyPostingProvider();

            // Act.
            var viewModel = provider.CreateVacancy(_validNewVacancyViewModelWithReferenceNumber);

            // Assert.
            MockVacancyPostingService.Verify(mock =>
                mock.GetVacancy(_validNewVacancyViewModelWithReferenceNumber.VacancyReferenceNumber.Value), Times.Once);
            MockVacancyPostingService.Verify(mock => mock.GetNextVacancyReferenceNumber(), Times.Never);
            MockVacancyPostingService.Verify(mock =>
                mock.ShallowSaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()), Times.Once);

            viewModel.VacancyReferenceNumber.Should().HaveValue();
        }

        [Test]
        public void ShouldCreateNewIfVacancyReferenceIsNotPresent()
        {
            // Arrange.
            var vvm = new Fixture().Build<NewVacancyViewModel>().Create();
            MockMapper.Setup(m => m.Map<ApprenticeshipVacancy, NewVacancyViewModel>(It.IsAny<ApprenticeshipVacancy>())).Returns(vvm);
            var provider = GetVacancyPostingProvider();

            // Act.
            var viewModel = provider.CreateVacancy(_validNewVacancyViewModelSansReferenceNumber);

            // Assert.
            MockVacancyPostingService.Verify(mock =>
                mock.GetVacancy(It.IsAny<long>()), Times.Never);
            MockVacancyPostingService.Verify(mock => mock.GetNextVacancyReferenceNumber(), Times.Once);
            MockVacancyPostingService.Verify(mock =>
                mock.CreateApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()), Times.Once);

            viewModel.VacancyReferenceNumber.Should().HaveValue();
        }

        [Test]
        public void ShouldStoreOfflineApplicationFields()
        {
            var provider = GetVacancyPostingProvider();

            const bool offlineVacancy = true;
            const string offlineApplicationUrl = "a_url.com";
            const string offlineApplicationInstructions = "Some instructions";

            provider.CreateVacancy(new NewVacancyViewModel
            {
                ProviderSiteEmployerLink = new ProviderSiteEmployerLinkViewModel
                {
                    ProviderSiteErn = ProviderSiteErn,
                    Employer  = new EmployerViewModel
                    {
                        Ern = Ern
                    }
                },
                OfflineVacancy = offlineVacancy,
                OfflineApplicationUrl = offlineApplicationUrl,
                OfflineApplicationInstructions = offlineApplicationInstructions,
            });

            MockVacancyPostingService.Verify(s => s.CreateApprenticeshipVacancy(It.Is<ApprenticeshipVacancy>(v => v.OfflineVacancy == offlineVacancy 
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
                OfflineVacancy = false,
                ProviderSiteEmployerLink = new ProviderSiteEmployerLinkViewModel
                {
                    ProviderSiteErn = ProviderSiteErn,
                    Employer = new EmployerViewModel
                    {
                        Ern = Ern
                    }
                }
            });

            MockVacancyPostingService.Verify(s => s.CreateApprenticeshipVacancy(It.Is<ApprenticeshipVacancy>(v => v.EntityId == vacancyGuid)));
        }

        [Test]
        public void ShouldReturnAnExistingVacancyIfVacancyGuidExists()
        {
            // Arrange
            var vacancyGuid = Guid.NewGuid();
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
                        Address = new PostalAddress()
                    }
                }
            };

            var vvm = new NewVacancyViewModel()
            {
                Title = av.Title,
                ShortDescription = av.ShortDescription,
                OfflineVacancy = av.OfflineVacancy,
                OfflineApplicationInstructions = av.OfflineApplicationInstructions,
                OfflineApplicationUrl = av.OfflineApplicationUrl
            };

            MockMapper.Setup(m => m.Map<ApprenticeshipVacancy, NewVacancyViewModel>(It.IsAny<ApprenticeshipVacancy>())).Returns(vvm);

            MockVacancyPostingService.Setup(s => s.GetVacancy(vacancyGuid)).Returns(av);
            var provider = GetVacancyPostingProvider();

            // Act
            var result = provider.GetNewVacancyViewModel(Ukprn, ProviderSiteErn, Ern, vacancyGuid, null);

            // Assert
            MockVacancyPostingService.Verify(s => s.GetVacancy(vacancyGuid), Times.Once);
            MockProviderService.Verify(s => s.GetProviderSiteEmployerLink(ProviderSiteErn, Ern), Times.Never);
            result.Should()
                .Match<NewVacancyViewModel>(
                    r =>
                        r.Title == av.Title && r.ShortDescription == av.ShortDescription &&
                        r.OfflineApplicationInstructions == av.OfflineApplicationInstructions);
        }

        [Test]
        public void ShouldReturnANewVacancyIfVacancyGuidDoesNotExists()
        {
            // Arrange
            var vacancyGuid = Guid.NewGuid();
            ApprenticeshipVacancy apprenticeshipVacancy = null;
            
            MockVacancyPostingService.Setup(s => s.GetVacancy(vacancyGuid)).Returns(apprenticeshipVacancy);
            var provider = GetVacancyPostingProvider();

            // Act
            var result = provider.GetNewVacancyViewModel(Ukprn, ProviderSiteErn, Ern, vacancyGuid, null);

            // Assert
            MockVacancyPostingService.Verify(s => s.GetVacancy(vacancyGuid), Times.Once);
            MockProviderService.Verify(s => s.GetProviderSiteEmployerLink(ProviderSiteErn, Ern), Times.Once);
            result.Should()
                .Match<NewVacancyViewModel>(
                    r =>
                        r.ProviderSiteEmployerLink.Description == _providerSiteEmployerLink.Description &&
                        r.ProviderSiteEmployerLink.ProviderSiteErn == ProviderSiteErn);
        }

        [Test]
        public void CreateVacancy_LocationSearchViewModel_StatusIsDraft()
        {
            //Arrange
            var locationSearchViewModel = new LocationSearchViewModel();
            var provider = GetVacancyPostingProvider();

            //Act
            provider.CreateVacancy(locationSearchViewModel);

            //Assert
            MockVacancyPostingService.Verify(s => s.CreateApprenticeshipVacancy(It.Is<ApprenticeshipVacancy>(av => av.Status == ProviderVacancyStatuses.Draft)));
        }

        [Test]
        public void CreateVacancy_LocationSearchViewModel_SavesOnce()
        {
            //Arrange
            var locationSearchViewModel = new LocationSearchViewModel();
            var provider = GetVacancyPostingProvider();

            //Act
            provider.CreateVacancy(locationSearchViewModel);

            //Assert
            MockVacancyPostingService.Verify(s => s.CreateApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()), Times.Once);
        }
    }
}