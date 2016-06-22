namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyPosting
{
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Raa.Common.Validators.Provider;
    using Raa.Common.Validators.Vacancy;
    using Raa.Common.Providers;
    using Raa.Common.Validators.VacancyPosting;
    using Application.Interfaces.Applications;
    using Application.Interfaces.Employers;
    using Application.Interfaces.Locations;
    using Application.Interfaces.Providers;
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.Users;
    using Application.Interfaces.Vacancies;
    using Application.Interfaces.VacancyPosting;
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
        private Mock<IGeoCodeLookupService> _mockGeoCodingService;
        private Mock<ILocalAuthorityLookupService> _mockLocalAuthorityService;

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
            MockEmployerService.Setup(s => s.GetEmployer(It.IsAny<int>()))
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
            _mockGeoCodingService = new Mock<IGeoCodeLookupService>();
            _mockLocalAuthorityService = new Mock<ILocalAuthorityLookupService>();
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
                _mockGeoCodingService.Object,
                _mockLocalAuthorityService.Object);
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
