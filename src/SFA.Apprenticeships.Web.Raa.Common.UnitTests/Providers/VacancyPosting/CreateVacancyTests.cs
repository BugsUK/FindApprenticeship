using SFA.Apprenticeships.Domain.Entities.Locations;

namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyPosting
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Vacancies;
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
        private const string EdsUrn = "112";
        private const int EmployerId = 1;
        private const int ProviderSiteId = 3;
        private const int VacancyPartyId = 4;

        private NewVacancyViewModel _validNewVacancyViewModelWithReferenceNumber;
        private NewVacancyViewModel _validNewVacancyViewModelSansReferenceNumber;

        private readonly Vacancy _existingVacancy = new Vacancy()
        {
            OwnerPartyId = 2
        };

        private readonly VacancyParty _vacancyParty = new VacancyParty
        {
            VacancyPartyId = VacancyPartyId,
            ProviderSiteId = ProviderSiteId,
            EmployerId = EmployerId,
            EmployerDescription = "description"
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
                OwnerParty = new VacancyPartyViewModel()
                {
                    VacancyPartyId = VacancyPartyId,
                    ProviderSiteId = ProviderSiteId,
                    Employer = new EmployerViewModel
                    {
                        EmployerId = EmployerId,
                        EdsUrn = EdsUrn,
                        Address = new AddressViewModel()
                    }
                },
                OfflineVacancy = false,
            };

            MockVacancyPostingService.Setup(mock => mock.GetVacancyByReferenceNumber(_validNewVacancyViewModelWithReferenceNumber.VacancyReferenceNumber.Value))
                .Returns(_existingVacancy);
            MockVacancyPostingService.Setup(mock => mock.CreateApprenticeshipVacancy(It.IsAny<Vacancy>()))
                .Returns<Vacancy>(v => v);
            MockVacancyPostingService.Setup(mock => mock.SaveVacancy(It.IsAny<Vacancy>()))
                .Returns<Vacancy>(v => v);
            MockVacancyPostingService.Setup(mock => mock.SaveVacancy(It.IsAny<Vacancy>()))
                .Returns<Vacancy>(v => v);
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
            MockProviderService.Setup(s => s.GetVacancyParty(ProviderSiteId, EdsUrn))
                .Returns(_vacancyParty);
            MockProviderService.Setup(s => s.GetVacancyParty(VacancyPartyId))
                .Returns(_vacancyParty);
            MockEmployerService.Setup(s => s.GetEmployer(EmployerId)).Returns(new Fixture().Build<Employer>().Create());
        }

        [Test]
        public void ShouldUpdateIfVacancyReferenceIsPresent()
        {
            // Arrange.
            var vvm = new Fixture().Build<NewVacancyViewModel>().Create();
            MockMapper.Setup(m => m.Map<Vacancy, NewVacancyViewModel>(It.IsAny<Vacancy>())).Returns(vvm);
            var provider = GetVacancyPostingProvider();

            // Act.
            var viewModel = provider.CreateVacancy(_validNewVacancyViewModelWithReferenceNumber);

            // Assert.
            MockVacancyPostingService.Verify(mock =>
                mock.GetVacancyByReferenceNumber(_validNewVacancyViewModelWithReferenceNumber.VacancyReferenceNumber.Value), Times.Once);
            MockVacancyPostingService.Verify(mock => mock.GetNextVacancyReferenceNumber(), Times.Never);
            MockVacancyPostingService.Verify(mock =>
                mock.UpdateVacancy(It.IsAny<Vacancy>()), Times.Once);

            viewModel.VacancyReferenceNumber.Should().HaveValue();
        }

        [Test]
        public void ShouldCreateNewIfVacancyReferenceIsNotPresent()
        {
            // Arrange.
            var vvm = new Fixture().Build<NewVacancyViewModel>().Create();
            MockMapper.Setup(m => m.Map<Vacancy, NewVacancyViewModel>(It.IsAny<Vacancy>())).Returns(vvm);
            var provider = GetVacancyPostingProvider();

            // Act.
            var viewModel = provider.CreateVacancy(_validNewVacancyViewModelSansReferenceNumber);

            // Assert.
            MockVacancyPostingService.Verify(mock =>
                mock.GetVacancyByReferenceNumber(It.IsAny<int>()), Times.Never);
            MockVacancyPostingService.Verify(mock => mock.GetNextVacancyReferenceNumber(), Times.Once);
            MockVacancyPostingService.Verify(mock =>
                mock.CreateApprenticeshipVacancy(It.IsAny<Vacancy>()), Times.Once);

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
                OwnerParty = new VacancyPartyViewModel
                {
                    VacancyPartyId = VacancyPartyId,
                    ProviderSiteId = ProviderSiteId,
                    Employer  = new EmployerViewModel
                    {
                        EmployerId = EmployerId
                    }
                },
                OfflineVacancy = offlineVacancy,
                OfflineApplicationUrl = offlineApplicationUrl,
                OfflineApplicationInstructions = offlineApplicationInstructions,
            });

            MockVacancyPostingService.Verify(s => s.CreateApprenticeshipVacancy(It.Is<Vacancy>(v => v.OfflineVacancy == offlineVacancy 
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
                OwnerParty = new VacancyPartyViewModel
                {
                    VacancyPartyId = VacancyPartyId,
                    ProviderSiteId = ProviderSiteId,
                    Employer = new EmployerViewModel
                    {
                        EmployerId = EmployerId
                    }
                }
            });

            MockVacancyPostingService.Verify(s => s.CreateApprenticeshipVacancy(It.Is<Vacancy>(v => v.VacancyGuid == vacancyGuid)));
        }

        [Test]
        public void ShouldReturnAnExistingVacancyIfVacancyGuidExists()
        {
            // Arrange
            var vacancyGuid = Guid.NewGuid();
            var av = new Vacancy
            {
                Title = "Title",
                ShortDescription = "shorts",
                FrameworkCodeName = "fwcn",
                StandardId = 1234,
                OfflineVacancy = true,
                OfflineApplicationUrl = "http://www.google.com",
                OfflineApplicationInstructions = "optional",
                ApprenticeshipLevel = ApprenticeshipLevel.Advanced,
                OwnerPartyId = 3
            };

            var vvm = new NewVacancyViewModel()
            {
                Title = av.Title,
                ShortDescription = av.ShortDescription,
                OfflineVacancy = av.OfflineVacancy,
                OfflineApplicationInstructions = av.OfflineApplicationInstructions,
                OfflineApplicationUrl = av.OfflineApplicationUrl
            };

            MockMapper.Setup(m => m.Map<Vacancy, NewVacancyViewModel>(It.IsAny<Vacancy>())).Returns(vvm);

            MockVacancyPostingService.Setup(s => s.GetVacancy(vacancyGuid)).Returns(av);
            var provider = GetVacancyPostingProvider();

            // Act
            var result = provider.GetNewVacancyViewModel(VacancyPartyId, vacancyGuid, null);

            // Assert
            MockVacancyPostingService.Verify(s => s.GetVacancy(vacancyGuid), Times.Once);
            MockProviderService.Verify(s => s.GetVacancyParty(ProviderSiteId, EdsUrn), Times.Never);
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
            Vacancy vacancy = null;
            
            MockVacancyPostingService.Setup(s => s.GetVacancy(vacancyGuid)).Returns(vacancy);
            var provider = GetVacancyPostingProvider();

            // Act
            var result = provider.GetNewVacancyViewModel(VacancyPartyId, vacancyGuid, null);

            // Assert
            MockVacancyPostingService.Verify(s => s.GetVacancy(vacancyGuid), Times.Once);
            MockProviderService.Verify(s => s.GetVacancyParty(VacancyPartyId), Times.Once);
            MockEmployerService.Verify(s => s.GetEmployer(EmployerId), Times.Once);
            result.Should()
                .Match<NewVacancyViewModel>(
                    r =>
                        r.OwnerParty.EmployerDescription == _vacancyParty.EmployerDescription &&
                        r.OwnerParty.ProviderSiteId == ProviderSiteId);
        }

        [Test]
        public void CreateVacancy_LocationSearchViewModel_StatusIsDraft()
        {
            //Arrange
            var locationSearchViewModel = new LocationSearchViewModel
            {
                ProviderSiteId = ProviderSiteId,
                EmployerId = EmployerId,
                EmployerEdsUrn = EdsUrn
            };
            var provider = GetVacancyPostingProvider();

            MockMapper.Setup(
                m =>
                    m.Map<List<VacancyLocationAddressViewModel>, List<VacancyLocation>>(
                        It.IsAny<List<VacancyLocationAddressViewModel>>())).Returns(new List<VacancyLocation>());

            //Act
            provider.CreateVacancy(locationSearchViewModel);

            //Assert
            MockVacancyPostingService.Verify(s => s.CreateApprenticeshipVacancy(It.Is<Vacancy>(av => av.Status == VacancyStatus.Draft)));
        }

        [Test]
        public void CreateVacancy_LocationSearchViewModel_SavesOnce()
        {
            //Arrange
            var locationSearchViewModel = new LocationSearchViewModel
            {
                ProviderSiteId = ProviderSiteId,
                EmployerId = EmployerId,
                EmployerEdsUrn = EdsUrn
            };
            var provider = GetVacancyPostingProvider();

            MockMapper.Setup(
                m =>
                    m.Map<List<VacancyLocationAddressViewModel>, List<VacancyLocation>>(
                        It.IsAny<List<VacancyLocationAddressViewModel>>())).Returns(new List<VacancyLocation>());

            //Act
            provider.CreateVacancy(locationSearchViewModel);

            //Assert
            MockVacancyPostingService.Verify(s => s.CreateApprenticeshipVacancy(It.IsAny<Vacancy>()), Times.Once);
        }


        [Test]
        public void ShouldNotAssignGeocodeIfVacancyReferenceIsNotPresentAndEmployerAddressHasGeocode()
        {
            // Arrange.
            var vvm = new Fixture().Build<NewVacancyViewModel>().Create();
            var employerWithGeocode = new Fixture().Create<Employer>();
            MockMapper.Setup(m => m.Map<Vacancy, NewVacancyViewModel>(It.IsAny<Vacancy>())).Returns(vvm);
            MockEmployerService.Setup(m => m.GetEmployer(It.IsAny<int>())).Returns(employerWithGeocode);
            var provider = GetVacancyPostingProvider();

            // Act.
            var viewModel = provider.CreateVacancy(_validNewVacancyViewModelSansReferenceNumber);

            // Assert.
            MockGeocodeService.Verify(m => m.GetGeoPointFor(It.IsAny<PostalAddress>()), Times.Never);
        }

        [Test]
        public void ShouldAssignGeocodeIfVacancyReferenceIsNotPresentAndEmployerAddressDoesNotHaveGeocode()
        {
            // Arrange.
            var vvm = new Fixture().Build<NewVacancyViewModel>().Create();
            var postalAddress = new Fixture().Build<PostalAddress>().With(pa => pa.GeoPoint, null).Create();
            var employerWithoutGeocode = new Fixture()
                .Build<Employer>()
                .With(e => e.Address, postalAddress)
                .Create();
            MockMapper.Setup(m => m.Map<Vacancy, NewVacancyViewModel>(It.IsAny<Vacancy>())).Returns(vvm);
            MockEmployerService.Setup(m => m.GetEmployer(It.IsAny<int>())).Returns(employerWithoutGeocode);
            var provider = GetVacancyPostingProvider();

            // Act.
            var viewModel = provider.CreateVacancy(_validNewVacancyViewModelSansReferenceNumber);

            // Assert.
            MockGeocodeService.Verify(m => m.GetGeoPointFor(postalAddress), Times.Once);
        }

        [Test]
        public void ShouldCreateApprenticeshipVacancyWithGeocodeIfEmployerHasGeocode()
        {
            // Arrange.
            var vvm = new Fixture().Build<NewVacancyViewModel>().Create();
            var geopoint = new Fixture().Create<GeoPoint>();
            var postalAddress = new Fixture().Build<PostalAddress>().With(pa => pa.GeoPoint, geopoint).Create();
            var employerWithGeocode = new Fixture()
                .Build<Employer>()
                .With(e => e.Address, postalAddress)
                .Create();
            MockMapper.Setup(m => m.Map<Vacancy, NewVacancyViewModel>(It.IsAny<Vacancy>())).Returns(vvm);
            MockEmployerService.Setup(m => m.GetEmployer(It.IsAny<int>())).Returns(employerWithGeocode);
            var provider = GetVacancyPostingProvider();

            // Act.
            var viewModel = provider.CreateVacancy(_validNewVacancyViewModelSansReferenceNumber);

            // Assert.
            MockVacancyPostingService.Verify(m => m.CreateApprenticeshipVacancy(It.Is<Vacancy>(av => av.Address.GeoPoint == geopoint)));
        }

        [Test]
        public void ShouldCreateApprenticeshipVacancyWithGeocodeIfEmployerDoesNotHaveGeocode()
        {
            // Arrange.
            var vvm = new Fixture().Build<NewVacancyViewModel>().Create();
            var geopoint = new Fixture().Create<GeoPoint>();
            var postalAddress = new Fixture().Build<PostalAddress>().With(pa => pa.GeoPoint, null).Create();
            var employerWithGeocode = new Fixture()
                .Build<Employer>()
                .With(e => e.Address, postalAddress)
                .Create();
            MockMapper.Setup(m => m.Map<Vacancy, NewVacancyViewModel>(It.IsAny<Vacancy>())).Returns(vvm);
            MockEmployerService.Setup(m => m.GetEmployer(It.IsAny<int>())).Returns(employerWithGeocode);
            MockGeocodeService.Setup(m => m.GetGeoPointFor(postalAddress)).Returns(geopoint);
            var provider = GetVacancyPostingProvider();

            // Act.
            var viewModel = provider.CreateVacancy(_validNewVacancyViewModelSansReferenceNumber);

            // Assert.
            MockVacancyPostingService.Verify(m => m.CreateApprenticeshipVacancy(It.Is<Vacancy>(av => av.Address.GeoPoint == geopoint)));
        }
    }
}