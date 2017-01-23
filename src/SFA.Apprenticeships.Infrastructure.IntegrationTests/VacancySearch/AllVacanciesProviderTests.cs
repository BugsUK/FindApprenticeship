namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.VacancySearch
{
    using System;
    using System.Linq;
    using Common.Configuration;
    using SFA.Infrastructure.Interfaces;
    using Common.IoC;
    using FluentAssertions;
    using Infrastructure.Elastic.Common.Configuration;
    using Infrastructure.Elastic.Common.IoC;
    using Infrastructure.VacancySearch;
    using Infrastructure.VacancySearch.IoC;
    using Logging.IoC;
    using Moq;
    using NUnit.Framework;

    using SFA.Apprenticeships.Application.Interfaces;

    using StructureMap;
    using Web.Common.Configuration;

    [TestFixture]
    public class AllVacanciesProviderTests
    {
        private IElasticsearchClientFactory _elasticsearchClientFactory;
        private readonly Mock<ILogService> _logger = new Mock<ILogService>();
        private string _environment;

        [SetUp]
        public void SetUp()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<ElasticsearchCommonRegistry>();
                x.AddRegistry<VacancySearchRegistry>();
            });

            _elasticsearchClientFactory = container.GetInstance<IElasticsearchClientFactory>();
            container.GetInstance<IMapper>();

            var configurationService = container.GetInstance<IConfigurationService>();
            _environment = configurationService.Get<CommonWebConfiguration>().Environment.ToLower();
        }

        [Test, Category("Integration")]
        //, Ignore("Failing because an Newtonsoft.Json.JsonReaderException -> update libraries?")
        public void ShouldGetAllApprenticeships()
        {
            // Arrange.
            var provider = new AllApprenticeshipVacanciesProvider(_logger.Object, _elasticsearchClientFactory);

            // Act.
            var vacancies = provider.GetAllVacancyIds($"{_environment}_apprenticeships").ToList();

            // Assert.
            vacancies.Should().NotBeNull();
            vacancies.Should().NotBeEmpty();

            Console.WriteLine("Apprenticeships: {0}", vacancies.Count);
        }

        [Test, Category("Integration")]
        public void ShouldGetAllTraineeships()
        {
            // Arrange.
            var provider = new AllTraineeshipVacanciesProvider(_logger.Object, _elasticsearchClientFactory);

            // Act.
            var vacancies = provider.GetAllVacancyIds($"{_environment}_traineeships").ToList();

            // Assert.
            vacancies.Should().NotBeNull();
            vacancies.Should().NotBeEmpty();

            Console.WriteLine("Traineeships: {0}", vacancies.Count);
        }
    }
}
