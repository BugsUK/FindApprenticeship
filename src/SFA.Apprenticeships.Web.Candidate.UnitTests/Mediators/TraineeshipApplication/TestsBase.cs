﻿namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipApplication
{
    using Candidate.Mediators.Application;
    using Candidate.Providers;
    using Common.Providers;
    using Configuration;
    using Domain.Interfaces.Configuration;
    using Moq;
    using NUnit.Framework;

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
            ConfigurationService.Setup(x => x.Get<WebConfiguration>(WebConfiguration.ConfigurationName))
                .Returns(new WebConfiguration() {VacancyResultsPerPage = 5});
            UserDataProvider = new Mock<IUserDataProvider>();
            Mediator = new TraineeshipApplicationMediator(TraineeshipApplicationProvider.Object, ConfigurationService.Object, UserDataProvider.Object);
        }
    }
}