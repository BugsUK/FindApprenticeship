namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipApplication
{
    using Candidate.Mediators.Application;
    using Candidate.Providers;
    using Candidate.Validators;
    using Common.Configuration;
    using Common.Providers;
    using SFA.Infrastructure.Interfaces;
    using Moq;

    public class TraineeshipApplicationMediatorBuilder
    {
        private Mock<ITraineeshipApplicationProvider> _traineeshipApplicationProvider;
        private readonly Mock<IConfigurationService> _configurationService;
        private Mock<IUserDataProvider> _userDataProvider;

        public TraineeshipApplicationMediatorBuilder()
        {
            _traineeshipApplicationProvider = new Mock<ITraineeshipApplicationProvider>();
            _configurationService = new Mock<IConfigurationService>();
            _userDataProvider = new Mock<IUserDataProvider>();
        }

        public TraineeshipApplicationMediatorBuilder With(Mock<ITraineeshipApplicationProvider> traineeshipApplicationProvider)
        {
            _traineeshipApplicationProvider = traineeshipApplicationProvider;
            return this;
        }

        public TraineeshipApplicationMediatorBuilder With(Mock<IUserDataProvider> userDataProvider)
        {
            _userDataProvider = userDataProvider;
            return this;
        }


        public ITraineeshipApplicationMediator Build()
        {
            _configurationService
                .Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration
                {
                    VacancyResultsPerPage = 5
                });

            return new TraineeshipApplicationMediator(
                _traineeshipApplicationProvider.Object,
                _configurationService.Object,
                _userDataProvider.Object,
                new TraineeshipApplicationViewModelServerValidator());
        }
    }
}