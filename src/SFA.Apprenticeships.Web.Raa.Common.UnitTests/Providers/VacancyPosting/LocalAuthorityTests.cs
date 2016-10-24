namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyPosting
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Vacancies;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using ViewModels.Employer;
    using ViewModels.Provider;
    using ViewModels.Vacancy;
    using ViewModels.VacancyPosting;
    using Web.Common.ViewModels.Locations;

    [TestFixture]
    [Parallelizable]
    public class LocalAuthorityTests : TestBase
    {
        private const string Ukprn = "12345";
        private const string EdsUrn = "112";
        private const int EmployerId = 1;
        private const int ProviderSiteId = 3;
        private const int VacancyOwnerRelationshipId = 4;

        private NewVacancyViewModel _validNewVacancyViewModelSansReferenceNumber;
        private VacancyMinimumData _validVacancyMinimumDataSansReferenceNumber;
        private NewVacancyViewModel _validNewVacancyViewModelWithReferenceNumber;


        private readonly VacancyOwnerRelationship _vacancyOwnerRelationship = new VacancyOwnerRelationship
        {
            VacancyOwnerRelationshipId = VacancyOwnerRelationshipId,
            ProviderSiteId = ProviderSiteId,
            EmployerId = EmployerId,
            EmployerDescription = "description"
        };

        [SetUp]
        public void SetUp()
        {
            _validVacancyMinimumDataSansReferenceNumber = new VacancyMinimumData
            {
                VacancyOwnerRelationshipId = VacancyOwnerRelationshipId
            };

            _validNewVacancyViewModelSansReferenceNumber = new NewVacancyViewModel
            {
                VacancyOwnerRelationship = new VacancyOwnerRelationshipViewModel()
                {
                    VacancyOwnerRelationshipId = VacancyOwnerRelationshipId,
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

            _validNewVacancyViewModelWithReferenceNumber = new NewVacancyViewModel
            {
                VacancyOwnerRelationship = new VacancyOwnerRelationshipViewModel()
                {
                    VacancyOwnerRelationshipId = VacancyOwnerRelationshipId,
                    ProviderSiteId = ProviderSiteId,
                    Employer = new EmployerViewModel
                    {
                        EmployerId = EmployerId,
                        EdsUrn = EdsUrn,
                        Address = new AddressViewModel()
                    }
                },
                OfflineVacancy = false,
                VacancyReferenceNumber = 1,
                VacancyGuid = Guid.NewGuid()
            };

            MockVacancyPostingService.Setup(mock => mock.CreateVacancy(It.IsAny<Vacancy>()))
                .Returns<Vacancy>(v => v);
            MockProviderService.Setup(s => s.GetVacancyOwnerRelationship(ProviderSiteId, EdsUrn))
                .Returns(_vacancyOwnerRelationship);
                
            MockProviderService.Setup(s => s.GetVacancyOwnerRelationship(VacancyOwnerRelationshipId, true))
                .Returns(_vacancyOwnerRelationship);
            MockEmployerService.Setup(s => s.GetEmployer(EmployerId, true)).Returns(new Fixture().Build<Employer>().Create());
        }

        [Test]
        public void ShouldAssignLocalAuthorityCodeToVacancyWithSameAddressAsEmployer()
        {
            // Arrange.
            const string localAuthorityCode = "ABCD";
            var vvm = new Fixture().Build<NewVacancyViewModel>().Create();
            var employerWithGeocode = new Fixture().Create<Employer>();
            MockMapper.Setup(m => m.Map<Vacancy, NewVacancyViewModel>(It.IsAny<Vacancy>())).Returns(vvm);
            MockEmployerService.Setup(m => m.GetEmployer(It.IsAny<int>(), It.IsAny<bool>())).Returns(employerWithGeocode);
            MockLocalAuthorityLookupService.Setup(m => m.GetLocalAuthorityCode(employerWithGeocode.Address.Postcode)).Returns(localAuthorityCode);
            MockProviderService.Setup(s => s.GetProvider(Ukprn, true)).Returns(new Provider());
            MockVacancyPostingService.Setup(s => s.GetVacancy(It.IsAny<Guid>())).Returns(new Fixture().Create<Vacancy>());

            var provider = GetVacancyPostingProvider();

            // Act.
            provider.UpdateVacancy(_validVacancyMinimumDataSansReferenceNumber);

            // Assert.
            MockLocalAuthorityLookupService.Verify(m => m.GetLocalAuthorityCode(employerWithGeocode.Address.Postcode), Times.Once);
            MockVacancyPostingService.Verify(s => s.UpdateVacancy(It.Is<Vacancy>(v => v.LocalAuthorityCode == localAuthorityCode)), Times.Once);
        }

        [Test]
        public void ShouldAssignLocalAuthorityCodeOnUpdatingVacancyLocationsWithSingleAddressDifferentToEmployer()
        {
            // Arrange.
            const string localAuthorityCode = "ABCD";
            var geopoint = new Fixture().Create<GeoPoint>();
            var addressViewModel = new Fixture().Build<AddressViewModel>().Create();
            var postalAddress = new Fixture().Build<PostalAddress>().With(a => a.GeoPoint, geopoint).Create();
            MockGeocodeService.Setup(gs => gs.GetGeoPointFor(It.IsAny<PostalAddress>())).Returns(geopoint);
            var locationSearchViewModel = new LocationSearchViewModel
            {
                EmployerId = EmployerId,
                ProviderSiteId = ProviderSiteId,
                EmployerEdsUrn = EdsUrn,
                Addresses = new List<VacancyLocationAddressViewModel>()
                {
                    new VacancyLocationAddressViewModel() {Address = addressViewModel}
                }
            };

            MockMapper.Setup(
                m =>
                    m.Map<VacancyLocationAddressViewModel, VacancyLocation>(It.IsAny<VacancyLocationAddressViewModel>()))
                .Returns(new VacancyLocation
                {
                    Address = postalAddress
                });

            MockMapper.Setup(m => m.Map<AddressViewModel, PostalAddress>(addressViewModel)).Returns(postalAddress);
            var geoPointViewModel = new Fixture().Create<GeoPointViewModel>();
            MockMapper.Setup(m => m.Map<GeoPoint, GeoPointViewModel>(geopoint))
                .Returns(geoPointViewModel);
            MockMapper.Setup(m => m.Map<GeoPointViewModel, GeoPoint>(geoPointViewModel)).Returns(geopoint);
            MockLocalAuthorityLookupService.Setup(m => m.GetLocalAuthorityCode(It.IsAny<string>()))
                .Returns(localAuthorityCode);
            var vacancy = new Fixture().Create<Vacancy>();
            MockVacancyPostingService.Setup(s => s.GetVacancyByReferenceNumber(It.IsAny<int>()))
                .Returns(vacancy);

            MockProviderService.Setup(s => s.GetProvider(Ukprn, true)).Returns(new Provider());

            var provider = GetVacancyPostingProvider();

            // Act.
            provider.AddLocations(locationSearchViewModel);

            // Assert.
            MockLocalAuthorityLookupService.Verify(m => m.GetLocalAuthorityCode(It.IsAny<string>()), Times.Once);
            MockVacancyPostingService.Verify(m => m.UpdateVacancy(It.Is<Vacancy>(av => av.LocalAuthorityCode == localAuthorityCode)));
            MockVacancyPostingService.Verify(m => m.UpdateVacancy(It.IsAny<Vacancy>()), Times.Once);
        }
        
        [Test]
        public void ShouldAssignLocalAuthorityCodeOnUpdatingVacancyLocationsWithMultipleAddresses()
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

            MockMapper.Setup(m => m.Map<VacancyLocationAddressViewModel, VacancyLocation>(locationSearchViewModel.Addresses[0])).Returns(vacancyLocations[0]);
            MockMapper.Setup(m => m.Map<VacancyLocationAddressViewModel, VacancyLocation>(locationSearchViewModel.Addresses[1])).Returns(vacancyLocations[1]);

            MockVacancyPostingService.Setup(v => v.CreateVacancy(It.IsAny<Vacancy>()))
                .Returns(new Vacancy());

            var vacancy = new Fixture().Create<Vacancy>();
            MockVacancyPostingService.Setup(s => s.GetVacancyByReferenceNumber(It.IsAny<int>()))
                .Returns(vacancy);

            MockProviderService.Setup(s => s.GetProvider(Ukprn, true)).Returns(new Provider());

            var provider = GetVacancyPostingProvider();

            // Act.
            provider.AddLocations(locationSearchViewModel);

            // Assert.
            MockVacancyPostingService.Verify(m => m.UpdateVacancy(It.IsAny<Vacancy>()), Times.Once);
            MockLocalAuthorityLookupService.Verify(m => m.GetLocalAuthorityCode(It.IsAny<string>()), Times.Exactly(2));
            MockVacancyPostingService.Verify(m => m.DeleteVacancyLocationsFor(vacancy.VacancyId));
            MockVacancyPostingService.Verify(m => m.CreateVacancyLocations(It.IsAny<List<VacancyLocation>>()), Times.Once);
        }
    }
}