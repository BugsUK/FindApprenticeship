namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.VacancySearch
{
    using System;
    using System.Linq;
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
    using StructureMap;

    [TestFixture]
    public class AllVacanciesProviderTests
    {
        private IElasticsearchClientFactory _elasticsearchClientFactory;
        private readonly Mock<ILogService> _logger = new Mock<ILogService>();

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
        }

        [Test, Category("Integration")]
        public void ShouldGetAllApprenticeships()
        {
            // Arrange.
            var provider = new AllApprenticeshipVacanciesProvider(_logger.Object, _elasticsearchClientFactory);

            // Act.
            var vacancies = provider.GetAllVacancyIds("apprenticeships").ToList();

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
            var vacancies = provider.GetAllVacancyIds("traineeships").ToList();

            // Assert.
            vacancies.Should().NotBeNull();
            vacancies.Should().NotBeEmpty();

            Console.WriteLine("Traineeships: {0}", vacancies.Count);
        }
    }
}
