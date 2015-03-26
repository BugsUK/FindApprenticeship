namespace SFA.Apprenticeships.Infrastructure.Monitor.IntegrationTests.Repositories
{
    using System;
    using Common.IoC;
    using FluentAssertions;
    using Logging.IoC;
    using Monitor.Repositories;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class CandidateMetricsRepositoryTests
    {
        protected Container Container;

        [SetUp]
        public void SetUpContainer()
        {
            Container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.For<ICandidateMetricsRepository>().Use<CandidateMetricsRepository>();
            });
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void GetCandidatesWithApplicationsInStatusCount()
        {
            var repository = Container.GetInstance<ICandidateMetricsRepository>();

            var count = 0;
            Action countAction = () => { count = repository.GetDismissedTraineeshipPromptCount(); };

            countAction.ShouldNotThrow();
            count.Should().BeGreaterOrEqualTo(0);
        }
    }
}