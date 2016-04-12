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
    using Web.Common.ViewModels.Locations;

    [TestFixture]
    public class LocalAuthorityTests : TestBase
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
        public void ShouldAssignLocalAuthorityCodeToVacancyWithSameAddressAsEmployer()
        {
            // Arrange.
            const string localAuthorityCode = "ABCD";
            var vvm = new Fixture().Build<NewVacancyViewModel>().Create();
            var employerWithGeocode = new Fixture().Create<Employer>();
            MockMapper.Setup(m => m.Map<Vacancy, NewVacancyViewModel>(It.IsAny<Vacancy>())).Returns(vvm);
            MockEmployerService.Setup(m => m.GetEmployer(It.IsAny<int>())).Returns(employerWithGeocode);
            MockLocalAuthorityLookupService.Setup(m => m.GetLocalAuthorityCode(employerWithGeocode.Address.Postcode)).Returns(localAuthorityCode);
            var provider = GetVacancyPostingProvider();

            // Act.
            provider.CreateVacancy(_validNewVacancyViewModelSansReferenceNumber);

            // Assert.
            MockLocalAuthorityLookupService.Verify(m => m.GetLocalAuthorityCode(employerWithGeocode.Address.Postcode), Times.Once);
            MockVacancyPostingService.Verify(s => s.CreateApprenticeshipVacancy(It.Is<Vacancy>(v => v.LocalAuthorityCode == localAuthorityCode)), Times.Once);
        }
    }
}