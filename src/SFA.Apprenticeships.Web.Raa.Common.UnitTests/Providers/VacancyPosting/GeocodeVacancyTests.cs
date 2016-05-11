namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyPosting
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Vacancies;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using ViewModels.Provider;
    using ViewModels.Vacancy;
    using ViewModels.VacancyPosting;
    using Web.Common.ViewModels.Locations;

    [TestFixture]
    public class GeoCodeVacancyTests : TestBase
    {
        private const string Ukprn = "12345";
        private const string EdsUrn = "112";
        private const int EmployerId = 1;
        private const int ProviderSiteId = 3;
        private const int VacancyPartyId = 4;

        private NewVacancyViewModel _validNewVacancyViewModelSansReferenceNumber;


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
            MockVacancyPostingService.Setup(mock => mock.CreateApprenticeshipVacancy(It.IsAny<Vacancy>()))
                .Returns<Vacancy>(v => v);
            MockProviderService.Setup(s => s.GetVacancyParty(ProviderSiteId, EdsUrn))
                .Returns(_vacancyParty);

            MockProviderService.Setup(s => s.GetVacancyParty(VacancyPartyId))
                .Returns(_vacancyParty);
            MockEmployerService.Setup(s => s.GetEmployer(EmployerId)).Returns(new Fixture().Build<Employer>().Create());
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
            provider.CreateVacancy(_validNewVacancyViewModelSansReferenceNumber, Ukprn);

            // Assert.
            MockGeocodeService.Verify(m => m.GetGeoPointFor(postalAddress), Times.Once);
        }

        [Test]
        public void ShouldAssignGeoCodeToVacancyWithMultipleAddresses()
        {
            // Arrange.
            var geopoint = new Fixture().Create<GeoPoint>();
            var geopoint2 = new Fixture().Create<GeoPoint>();
            var addressViewModel = new Fixture().Build<AddressViewModel>().Create();
            var postalAddress = new Fixture().Build<PostalAddress>().With(a => a.GeoPoint, geopoint).Create();
            var postalAddress2 = new Fixture().Build<PostalAddress>().With(a => a.GeoPoint, geopoint2).Create();
            MockGeocodeService.Setup(gs => gs.GetGeoPointFor(It.IsAny<PostalAddress>())).Returns(geopoint);
            var locationSearchViewModel = new LocationSearchViewModel
            {
                EmployerId = EmployerId,
                ProviderSiteId = ProviderSiteId,
                EmployerEdsUrn = EdsUrn,
                Addresses = new List<VacancyLocationAddressViewModel>
                {
                    new VacancyLocationAddressViewModel {Address = addressViewModel},
                    new VacancyLocationAddressViewModel {Address = addressViewModel}
                }
            };

            var vacancyLocations = new List<VacancyLocation>
            {
                new VacancyLocation {Address = postalAddress},
                new VacancyLocation {Address = postalAddress2}
            };

            MockMapper.Setup(
                m =>
                    m.Map<List<VacancyLocationAddressViewModel>, List<VacancyLocation>>(
                        It.IsAny<List<VacancyLocationAddressViewModel>>())).Returns(vacancyLocations);
            MockVacancyPostingService.Setup(v => v.CreateApprenticeshipVacancy(It.IsAny<Vacancy>()))
                .Returns(new Vacancy());

            var provider = GetVacancyPostingProvider();

            // Act.
            provider.CreateVacancy(locationSearchViewModel, Ukprn);

            // Assert.
            MockGeocodeService.Verify(gs => gs.GetGeoPointFor(It.IsAny<PostalAddress>()), Times.Exactly(2));
            MockVacancyPostingService.Verify(m => m.CreateApprenticeshipVacancy(It.IsAny<Vacancy>()), Times.Once);
            MockVacancyPostingService.Verify(m => m.SaveVacancyLocations(vacancyLocations), Times.Once);
        }

        [Test]
        public void ShouldAssignGeoCodeToVacancyWithSingleAddressDifferentToEmployer()
        {
            // Arrange.
            var geopoint = new Fixture().Create<GeoPoint>();
            var addressViewModel = new Fixture().Build<AddressViewModel>().Create();
            var postalAddress = new Fixture().Build<PostalAddress>().With(a => a.GeoPoint, geopoint).Create();
            MockGeocodeService.Setup(gs => gs.GetGeoPointFor(It.IsAny<PostalAddress>())).Returns(geopoint);
            var locationSearchViewModel = new LocationSearchViewModel
            {
                EmployerId = EmployerId,
                ProviderSiteId = ProviderSiteId,
                EmployerEdsUrn = EdsUrn,
                Addresses = new List<VacancyLocationAddressViewModel>
                {
                    new VacancyLocationAddressViewModel {Address = addressViewModel}
                }
            };

            MockMapper.Setup(m => m.Map<AddressViewModel, PostalAddress>(addressViewModel)).Returns(postalAddress);
            var geoPointViewModel = new Fixture().Create<GeoPointViewModel>();
            MockMapper.Setup(m => m.Map<GeoPoint, GeoPointViewModel>(geopoint))
                .Returns(geoPointViewModel);
            MockMapper.Setup(m => m.Map<GeoPointViewModel, GeoPoint>(geoPointViewModel)).Returns(geopoint);

            var provider = GetVacancyPostingProvider();

            // Act.
            provider.CreateVacancy(locationSearchViewModel, Ukprn);

            // Assert.
            MockGeocodeService.Verify(gs => gs.GetGeoPointFor(It.IsAny<PostalAddress>()), Times.Once);
            MockVacancyPostingService.Verify(
                m => m.CreateApprenticeshipVacancy(It.Is<Vacancy>(av => av.Address.GeoPoint.Equals(geopoint))));
            MockVacancyPostingService.Verify(m => m.CreateApprenticeshipVacancy(It.IsAny<Vacancy>()), Times.Once);
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
            provider.CreateVacancy(_validNewVacancyViewModelSansReferenceNumber, Ukprn);

            // Assert.
            MockVacancyPostingService.Verify(
                m => m.CreateApprenticeshipVacancy(It.Is<Vacancy>(av => av.Address.GeoPoint.Equals(geopoint))));
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
            provider.CreateVacancy(_validNewVacancyViewModelSansReferenceNumber, Ukprn);

            // Assert.
            MockVacancyPostingService.Verify(
                m => m.CreateApprenticeshipVacancy(It.Is<Vacancy>(av => av.Address.GeoPoint.Equals(geopoint))));
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
            provider.CreateVacancy(_validNewVacancyViewModelSansReferenceNumber, Ukprn);

            // Assert.
            MockGeocodeService.Verify(m => m.GetGeoPointFor(It.IsAny<PostalAddress>()), Times.Never);
        }

        // Update
        [Test]
        public void ShouldAssignGeoCodeOnUpdatingVacancyWithSingleAddressDifferentToEmployer()
        {
            // Arrange.
            var geopoint = new Fixture().Create<GeoPoint>();
            var addressViewModel = new Fixture().Build<AddressViewModel>().Create();
            var postalAddress = new Fixture().Build<PostalAddress>().With(a => a.GeoPoint, geopoint).Create();
            MockGeocodeService.Setup(gs => gs.GetGeoPointFor(It.IsAny<PostalAddress>())).Returns(geopoint);
            var locationSearchViewModel = new LocationSearchViewModel
            {
                EmployerId = EmployerId,
                ProviderSiteId = ProviderSiteId,
                EmployerEdsUrn = EdsUrn,
                Addresses = new List<VacancyLocationAddressViewModel>
                {
                    new VacancyLocationAddressViewModel {Address = addressViewModel}
                }
            };

            MockMapper.Setup(m => m.Map<AddressViewModel, PostalAddress>(addressViewModel)).Returns(postalAddress);
            var geoPointViewModel = new Fixture().Create<GeoPointViewModel>();
            MockMapper.Setup(m => m.Map<GeoPoint, GeoPointViewModel>(geopoint))
                .Returns(geoPointViewModel);
            MockMapper.Setup(m => m.Map<GeoPointViewModel, GeoPoint>(geoPointViewModel)).Returns(geopoint);

            var vacancy = new Fixture().Create<Vacancy>();
            MockVacancyPostingService.Setup(s => s.GetVacancyByReferenceNumber(It.IsAny<int>()))
                .Returns(vacancy);

            var provider = GetVacancyPostingProvider();

            // Act.
            provider.AddLocations(locationSearchViewModel);

            // Assert.
            MockGeocodeService.Verify(gs => gs.GetGeoPointFor(It.IsAny<PostalAddress>()), Times.Once);
            MockVacancyPostingService.Verify(
                m => m.UpdateVacancy(It.Is<Vacancy>(av => av.Address.GeoPoint.Equals(geopoint))));
            MockVacancyPostingService.Verify(m => m.UpdateVacancy(It.IsAny<Vacancy>()), Times.Once);
        }
        
        [Test]
        public void ShouldAssignGeoCodeOnUpdatingAVacancyWithMultipleAddresses()
        {
            // Arrange.
            var geopoint = new Fixture().Create<GeoPoint>();
            var geopoint2 = new Fixture().Create<GeoPoint>();
            var addressViewModel = new Fixture().Build<AddressViewModel>().Create();
            var postalAddress = new Fixture().Build<PostalAddress>().With(a => a.GeoPoint, geopoint).Create();
            var postalAddress2 = new Fixture().Build<PostalAddress>().With(a => a.GeoPoint, geopoint2).Create();
            MockGeocodeService.Setup(gs => gs.GetGeoPointFor(It.IsAny<PostalAddress>())).Returns(geopoint);
            var locationSearchViewModel = new LocationSearchViewModel
            {
                EmployerId = EmployerId,
                ProviderSiteId = ProviderSiteId,
                EmployerEdsUrn = EdsUrn,
                Addresses = new List<VacancyLocationAddressViewModel>
                {
                    new VacancyLocationAddressViewModel {Address = addressViewModel},
                    new VacancyLocationAddressViewModel {Address = addressViewModel}
                }
            };

            var vacancyLocations = new List<VacancyLocation>
            {
                new VacancyLocation {Address = postalAddress},
                new VacancyLocation {Address = postalAddress2}
            };

            MockMapper.Setup(
                m =>
                    m.Map<List<VacancyLocationAddressViewModel>, List<VacancyLocation>>(
                        It.IsAny<List<VacancyLocationAddressViewModel>>())).Returns(vacancyLocations);
            MockVacancyPostingService.Setup(v => v.CreateApprenticeshipVacancy(It.IsAny<Vacancy>()))
                .Returns(new Vacancy());
            var vacancy = new Fixture().Create<Vacancy>();
            MockVacancyPostingService.Setup(s => s.GetVacancyByReferenceNumber(It.IsAny<int>()))
                .Returns(vacancy);

            var provider = GetVacancyPostingProvider();

            // Act.
            provider.AddLocations(locationSearchViewModel);

            // Assert.
            MockGeocodeService.Verify(gs => gs.GetGeoPointFor(It.IsAny<PostalAddress>()), Times.Exactly(2));
            MockVacancyPostingService.Verify(m => m.UpdateVacancy(It.IsAny<Vacancy>()), Times.Once);
            MockVacancyPostingService.Verify(m => m.DeleteVacancyLocationsFor(vacancy.VacancyId));
            MockVacancyPostingService.Verify(m => m.SaveVacancyLocations(vacancyLocations), Times.Once);
        }
    }
}