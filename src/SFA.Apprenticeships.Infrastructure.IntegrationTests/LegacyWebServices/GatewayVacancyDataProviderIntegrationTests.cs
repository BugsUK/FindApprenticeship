namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.LegacyWebServices
{
    using System.Linq;
    using Application.Vacancies;
    using Application.Vacancy;
    using Common.Configuration;
    using Common.IoC;
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentAssertions;
    using Infrastructure.LegacyWebServices.IoC;
    using Infrastructure.Monitor.IoC;
    using Logging.IoC;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class GatewayVacancyDataProviderIntegrationTests
    {
        [SetUp]
        public void SetUp()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry(new LegacyWebServicesRegistry());
                x.AddRegistry(new VacancySourceRegistry(new CacheConfiguration(),  new ServicesConfiguration { ServiceImplementation = ServicesConfiguration.Legacy }));
            });

            _vacancyDataProvider = container.GetInstance<IVacancyDataProvider<ApprenticeshipVacancyDetail>>();
            _vacancyIndexDataProvider = container.GetInstance<IVacancyIndexDataProvider>();
        }

        private IVacancyDataProvider<ApprenticeshipVacancyDetail> _vacancyDataProvider;
        private IVacancyIndexDataProvider _vacancyIndexDataProvider;

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldNotReturnVacancyDetailsForInvalidVacancyId()
        {
            var result = _vacancyDataProvider.GetVacancyDetails(123456789);

            result.Should().BeNull();
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldReturnVacancyDetailsForValidVacancyId()
        {
            var response = _vacancyIndexDataProvider.GetVacancySummaries(1);

            var firstOrDefault = response.ApprenticeshipSummaries.FirstOrDefault();

            if (firstOrDefault == null) return;

            var firstVacancyId = firstOrDefault.Id;
            var result = _vacancyDataProvider.GetVacancyDetails(firstVacancyId);
            result.Should().NotBeNull();
        }
    }
}