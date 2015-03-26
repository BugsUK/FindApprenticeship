namespace SFA.Apprenticeships.Infrastructure.Monitor.IntegrationTests.Repositories
{
    using System;
    using Common.IoC;
    using Domain.Entities.Applications;
    using FluentAssertions;
    using Logging.IoC;
    using Monitor.Repositories;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class ApprenticeshipMetricsRepositoryTests
    {
        protected Container Container;

        [SetUp]
        public void SetUpContainer()
        {
            Container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.For<IApprenticeshipMetricsRepository>().Use<ApprenticeshipMetricsRepository>();
            });
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void GetCandidatesWithApplicationsInStatusCount()
        {
            var repository = Container.GetInstance<IApprenticeshipMetricsRepository>();

            var count = 0;
            Action countAction = () => { count = repository.GetCandidatesWithApplicationsInStatusCount(ApplicationStatuses.Submitted, 1); };

            countAction.ShouldNotThrow();
            count.Should().BeGreaterOrEqualTo(0);
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void GetNumberOfCandidatesWithSixOrMoreUnsuccessfulApplications()
        {
            var repository = Container.GetInstance<IApprenticeshipMetricsRepository>();

            var count = 0;
            Action countAction = () => { count = repository.GetCandidatesWithApplicationsInStatusCount(ApplicationStatuses.Unsuccessful, 6); };

            countAction.ShouldNotThrow();
            count.Should().BeGreaterOrEqualTo(0);
        }
    }
}