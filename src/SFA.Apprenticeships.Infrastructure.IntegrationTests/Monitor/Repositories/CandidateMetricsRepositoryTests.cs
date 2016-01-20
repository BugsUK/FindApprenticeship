namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.Monitor.Repositories
{
    using System;
    using Common.Configuration;
    using Common.IoC;
    using FluentAssertions;
    using Infrastructure.Monitor.Repositories;
    using Logging.IoC;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class CandidateMetricsRepositoryTests
    {
        protected Container Container;

        [SetUp]
        public void SetUpContainer()
        {
            var configurationStorageConnectionString = SettingsTestHelper.GetStorageConnectionString();

            Container = new Container(x =>
            {
                x.AddRegistry(new CommonRegistry(new CacheConfiguration(), configurationStorageConnectionString));
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