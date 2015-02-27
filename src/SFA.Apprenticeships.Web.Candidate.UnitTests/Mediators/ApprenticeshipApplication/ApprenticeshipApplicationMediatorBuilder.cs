namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
{
    using Candidate.Mediators.Application;
    using Candidate.Providers;
    using Candidate.Validators;
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using Moq;

    public class ApprenticeshipApplicationMediatorBuilder
    {
        private Mock<IApprenticeshipApplicationProvider> _apprenticeshipApplicationProvider;
        private Mock<IConfigurationManager> _configurationManager;
        private Mock<IUserDataProvider> _userDataProvider;

        public ApprenticeshipApplicationMediatorBuilder()
        {
            _apprenticeshipApplicationProvider = new Mock<IApprenticeshipApplicationProvider>();
            _configurationManager = new Mock<IConfigurationManager>();
            _userDataProvider = new Mock<IUserDataProvider>();
        }

        public ApprenticeshipApplicationMediatorBuilder With(Mock<IApprenticeshipApplicationProvider> apprenticeshipApplicationProvider)
        {
            _apprenticeshipApplicationProvider = apprenticeshipApplicationProvider;
            return this;
        }

        public ApprenticeshipApplicationMediatorBuilder With(Mock<IConfigurationManager> configurationManager)
        {
            _configurationManager = configurationManager;
            return this;
        }

        public ApprenticeshipApplicationMediatorBuilder With(Mock<IUserDataProvider> userDataProvider)
        {
            _userDataProvider = userDataProvider;
            return this;
        }


        public ApprenticeshipApplicationMediator Build()
        {
            var mediator = new ApprenticeshipApplicationMediator(_apprenticeshipApplicationProvider.Object, new ApprenticeshipApplicationViewModelServerValidator(), new ApprenticeshipApplicationViewModelSaveValidator(), _configurationManager.Object, _userDataProvider.Object);
            return mediator;
        }
    }
}