namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipApplication
{
    using Candidate.Mediators.Application;
    using Candidate.Providers;
    using Candidate.Validators;
    using Common.Configuration;
    using Common.Providers;
    using SFA.Infrastructure.Interfaces;
    using Moq;
    using NUnit.Framework;

    using SFA.Apprenticeships.Application.Interfaces;

    public abstract class TestsBase
    {
        protected Mock<ITraineeshipApplicationProvider> TraineeshipApplicationProvider;
        protected Mock<IConfigurationService> ConfigurationService;
        protected Mock<IUserDataProvider> UserDataProvider;
        protected ITraineeshipApplicationMediator Mediator;

        [SetUp]
        public void Setup()
        {
            TraineeshipApplicationProvider = new Mock<ITraineeshipApplicationProvider>();
            ConfigurationService = new Mock<IConfigurationService>();
            ConfigurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration() {VacancyResultsPerPage = 5});
            UserDataProvider = new Mock<IUserDataProvider>();
            Mediator = new TraineeshipApplicationMediator(TraineeshipApplicationProvider.Object, ConfigurationService.Object, UserDataProvider.Object, new TraineeshipApplicationViewModelServerValidator());
        }
    }
}