namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
{
    using Candidate.Mediators.Application;
    using Candidate.Providers;
    using Candidate.Validators;
    using Common.Configuration;
    using Common.Providers;
    using Common.Validators;
    using SFA.Infrastructure.Interfaces;
    using Moq;

    public class ApprenticeshipApplicationMediatorBuilder
    {
        private Mock<IApprenticeshipApplicationProvider> _apprenticeshipApplicationProvider;
        private readonly Mock<IConfigurationService> _configurationService;
        private Mock<IUserDataProvider> _userDataProvider;

        public ApprenticeshipApplicationMediatorBuilder()
        {
            _apprenticeshipApplicationProvider = new Mock<IApprenticeshipApplicationProvider>();
            _configurationService = new Mock<IConfigurationService>();
            _userDataProvider = new Mock<IUserDataProvider>();
        }

        public ApprenticeshipApplicationMediatorBuilder With(Mock<IApprenticeshipApplicationProvider> apprenticeshipApplicationProvider)
        {
            _apprenticeshipApplicationProvider = apprenticeshipApplicationProvider;
            return this;
        }

        public ApprenticeshipApplicationMediatorBuilder With(Mock<IUserDataProvider> userDataProvider)
        {
            _userDataProvider = userDataProvider;
            return this;
        }


        public IApprenticeshipApplicationMediator Build()
        {
            _configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration {VacancyResultsPerPage = 5});

            return new ApprenticeshipApplicationMediator(
                _configurationService.Object,
                _userDataProvider.Object,
                _apprenticeshipApplicationProvider.Object,
                new ApprenticeshipApplicationViewModelServerValidator(),
                new ApprenticeshipApplicationViewModelSaveValidator(),
                new ApprenticeshipApplicationPreviewViewModelValidator());
        }
    }
}