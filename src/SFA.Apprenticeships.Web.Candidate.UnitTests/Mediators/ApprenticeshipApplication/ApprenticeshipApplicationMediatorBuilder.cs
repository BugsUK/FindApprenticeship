﻿namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
{
    using Candidate.Mediators.Application;
    using Candidate.Providers;
    using Candidate.Validators;
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using Infrastructure.Web.Configuration;
    using Moq;

    public class ApprenticeshipApplicationMediatorBuilder
    {
        private Mock<IApprenticeshipApplicationProvider> _apprenticeshipApplicationProvider;
        private Mock<IConfigurationService> _configurationService;
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


        public ApprenticeshipApplicationMediator Build()
        {
            _configurationService.Setup(x => x.Get<WebConfiguration>(WebConfiguration.WebConfigurationName))
                .Returns(new WebConfiguration() {VacancyResultsPerPage = 5});
            var mediator = new ApprenticeshipApplicationMediator(_apprenticeshipApplicationProvider.Object, new ApprenticeshipApplicationViewModelServerValidator(), new ApprenticeshipApplicationViewModelSaveValidator(), _configurationService.Object, _userDataProvider.Object);
            return mediator;
        }
    }
}