﻿namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.LegacyWebServices
{
    using Application.Vacancies;
    using Common.IoC;
    using FluentAssertions;
    using Infrastructure.LegacyWebServices.IoC;
    using Infrastructure.LegacyWebServices.Vacancy;
    using Logging.IoC;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class GatewayVacancyIndexDataProviderIntegrationTests
    {
        [SetUp]
        public void SetUp()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<LegacyWebServicesRegistry>();

                // Inject provider under test.
                x.For<IVacancyIndexDataProvider>().Use<LegacyVacancyIndexDataProvider>();
            });

            // Providers.
            _vacancyIndexDataProvider = container.GetInstance<IVacancyIndexDataProvider>();
        }

        private IVacancyIndexDataProvider _vacancyIndexDataProvider;

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldReturnTheFirstPageResultForVacancies()
        {
            var response = _vacancyIndexDataProvider.GetVacancySummaries(1);

            response.Should().NotBeNull();
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldReturnThePageCountForVacancies()
        {
            var result = _vacancyIndexDataProvider.GetVacancyPageCount();

            // Assert.
            result.Should().BePositive();
        }
    }
}