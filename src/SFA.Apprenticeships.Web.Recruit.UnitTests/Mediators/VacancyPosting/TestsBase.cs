namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyPosting
{
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Raa.Common.Validators.Provider;
    using Raa.Common.Validators.Vacancy;
    using Raa.Common.Providers;
    using Raa.Common.Validators.VacancyPosting;
    using Apprenticeships.Application.Interfaces.Applications;
    using Apprenticeships.Application.Interfaces.Employers;
    using Apprenticeships.Application.Interfaces.Locations;
    using Apprenticeships.Application.Interfaces.Providers;
    using Apprenticeships.Application.Interfaces.ReferenceData;
    using Apprenticeships.Application.Interfaces.Users;
    using Apprenticeships.Application.Interfaces.Vacancies;
    using Apprenticeships.Application.Interfaces.VacancyPosting;
    using Domain.Entities.Raa.Parties;
    using Common.Configuration;
    using Raa.Common.Configuration;
    using Recruit.Mediators.VacancyPosting;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;

    public class TestsBase
    {
        protected Mock<IVacancyPostingProvider> VacancyPostingProvider;
        protected Mock<IVacancyPostingService> VacancyPostingService;
        protected Mock<IProviderProvider> ProviderProvider;
        protected Mock<IEmployerProvider> EmployerProvider;
        protected Mock<IGeoCodingProvider> GeoCodingProvider;

        private Mock<IConfigurationService> _mockConfigurationService;
        private Mock<ILogService> _mockLogService;
        private Mock<IReferenceDataService> _mockReferenceDataService;
        private Mock<IDateTimeService> _mockTimeService;
        private Mock<IApprenticeshipApplicationService> _mockApprenticeshipApplicationService;
        private Mock<ITraineeshipApplicationService> _mockTraineeshipApplicationService;
        private Mock<IVacancyLockingService> _mockVacancyLockingService;
        protected Mock<IMapper> MockMapper;
        protected Mock<IProviderService> MockProviderService;
        protected Mock<IEmployerService> MockEmployerService;
        protected Mock<IVacancyPostingService> MockVacancyPostingService;
        private Mock<ICurrentUserService> _mockCurrentUserService;
        private Mock<IUserProfileService> _mockUserProfileService;
        protected Mock<IGeoCodeLookupService> MockGeoCodingService;
        protected Mock<ILocalAuthorityLookupService> MockLocalAuthorityService;

        [SetUp]
        public void SetUp()
        {
            VacancyPostingProvider = new Mock<IVacancyPostingProvider>();
            VacancyPostingService=new Mock<IVacancyPostingService>();
            ProviderProvider = new Mock<IProviderProvider>();
            EmployerProvider = new Mock<IEmployerProvider>();
            GeoCodingProvider = new Mock<IGeoCodingProvider>();

            _mockLogService = new Mock<ILogService>();
            MockMapper = new Mock<IMapper>();
            MockVacancyPostingService = new Mock<IVacancyPostingService>();
            MockProviderService = new Mock<IProviderService>();
            MockEmployerService = new Mock<IEmployerService>();
            _mockReferenceDataService = new Mock<IReferenceDataService>();
            _mockConfigurationService = new Mock<IConfigurationService>();

            MockProviderService.Setup(s => s.GetProviderSite(It.IsAny<int>()))
                .Returns(new Fixture().Build<ProviderSite>().Create());
            MockEmployerService.Setup(s => s.GetEmployer(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(new Fixture().Build<Employer>().Create());
            _mockConfigurationService.Setup(mcs => mcs.Get<CommonWebConfiguration>()).Returns(new CommonWebConfiguration());
            _mockConfigurationService.Setup(mcs => mcs.Get<RecruitWebConfiguration>())
                .Returns(new RecruitWebConfiguration { AutoSaveTimeoutInSeconds = 60 });

            _mockTimeService = new Mock<IDateTimeService>();
            _mockApprenticeshipApplicationService = new Mock<IApprenticeshipApplicationService>();
            _mockTraineeshipApplicationService = new Mock<ITraineeshipApplicationService>();
            _mockVacancyLockingService = new Mock<IVacancyLockingService>();
            _mockCurrentUserService = new Mock<ICurrentUserService>();
            _mockUserProfileService = new Mock<IUserProfileService>();
            MockGeoCodingService = new Mock<IGeoCodeLookupService>();
            MockLocalAuthorityService = new Mock<ILocalAuthorityLookupService>();
        }

        protected IVacancyPostingProvider GetVacancyPostingProvider()
        {
            return new VacancyProvider(_mockLogService.Object,
                _mockConfigurationService.Object,
                MockVacancyPostingService.Object,
                _mockReferenceDataService.Object,
                MockProviderService.Object,
                MockEmployerService.Object,
                _mockTimeService.Object,
                MockMapper.Object,
                _mockApprenticeshipApplicationService.Object,
                _mockTraineeshipApplicationService.Object,
                _mockVacancyLockingService.Object,
                _mockCurrentUserService.Object,
                _mockUserProfileService.Object,
                MockGeoCodingService.Object,
                MockLocalAuthorityService.Object);
        }

        protected IVacancyPostingMediator GetMediator()
        {
            return new VacancyPostingMediator(
                VacancyPostingProvider.Object,
                ProviderProvider.Object,
                EmployerProvider.Object,
                GeoCodingProvider.Object,
                new NewVacancyViewModelServerValidator(),
                new NewVacancyViewModelClientValidator(),
                new VacancySummaryViewModelServerValidator(),
                new VacancySummaryViewModelClientValidator(),
                new VacancyRequirementsProspectsViewModelServerValidator(),
                new VacancyRequirementsProspectsViewModelClientValidator(),
                new VacancyQuestionsViewModelServerValidator(),
                new VacancyQuestionsViewModelClientValidator(),
                new VacancyDatesViewModelServerValidator(),
                new VacancyViewModelValidator(), 
                new VacancyPartyViewModelValidator(),
                new EmployerSearchViewModelServerValidator(),
                new LocationSearchViewModelServerValidator(),
                new Mock<ILocationsProvider>().Object,
                new TrainingDetailsViewModelServerValidator(),
                new TrainingDetailsViewModelClientValidator());
        }
    }
}
