namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.LegacyWebServices
{
    using Application.Vacancies;
    using Common.Configuration;
    using Common.IoC;
    using FluentAssertions;
    using Infrastructure.Caching.Memory.IoC;
    using Infrastructure.LegacyWebServices.IoC;
    using Infrastructure.Monitor.IoC;
    using Logging.IoC;
    using NUnit.Framework;

    using Application.Candidate.Configuration;

    using StructureMap;

    [TestFixture]
    public class VacancyServiceTests
    {
        [TestCase, Category("Integration"), Category("SmokeTests")]
        public void ShouldReturnMappedCollectionFromGetVacancySummary()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<MemoryCacheRegistry>();
                x.AddRegistry(new LegacyWebServicesRegistry(new ServicesConfiguration { ServiceImplementation = ServicesConfiguration.Legacy, VacanciesSource = ServicesConfiguration.Legacy }, new CacheConfiguration()));
                x.AddRegistry(new VacancySourceRegistry(new CacheConfiguration(), new ServicesConfiguration { ServiceImplementation = ServicesConfiguration.Legacy, VacanciesSource = ServicesConfiguration.Legacy}));
            });

            var service = container.GetInstance<IVacancyIndexDataProvider>();

            var result = service.GetVacancySummaries(1);

            result.ApprenticeshipSummaries.Should().NotBeNullOrEmpty();
            result.TraineeshipSummaries.Should().NotBeNullOrEmpty();
        }
    }
}