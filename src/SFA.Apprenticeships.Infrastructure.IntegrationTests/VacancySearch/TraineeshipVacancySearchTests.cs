namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.VacancySearch
{
    using System.Linq;
    using SFA.Infrastructure.Interfaces;
    using Application.Interfaces.Vacancies;
    using Common.IoC;
    using Domain.Entities.Locations;
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

    [TestFixture]
    public class TraineeshipVacancySearchTests
    {
        private IElasticsearchClientFactory _elasticsearchClientFactory;
        private IMapper _mapper;
        private readonly Mock<ILogService> _logger = new Mock<ILogService>();

        [SetUp]
        public void FixtureSetUp()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<ElasticsearchCommonRegistry>();
                x.AddRegistry<VacancySearchRegistry>();
            });

            _elasticsearchClientFactory = container.GetInstance<IElasticsearchClientFactory>();
            _mapper = container.GetInstance<IMapper>();
        }

        [Test, Category("Integration")]
        public void ShouldSortByPostedDate()
        {
            var vacancySearchProvider = new TraineeshipsSearchProvider(_elasticsearchClientFactory, _mapper, _logger.Object);

            var searchParameters = GetPostedDateSearchParameters();
            
            var vacancies = vacancySearchProvider.FindVacancies(searchParameters);

            vacancies.Results.Count().Should().BeGreaterThan(0);
            vacancies.Results.Should().BeInDescendingOrder(r => r.PostedDate);
        }

        private static TraineeshipSearchParameters GetCommonSearchParameters()
        {
            return new TraineeshipSearchParameters
            {
                Location = new Location
                {
                    Name = "London",
                    GeoPoint = new GeoPoint
                    {
                        Longitude = -1.50812239495425,
                        Latitude = 52.4009991288043
                    }
                },
                PageNumber = 1,
                PageSize = 50,
                SearchRadius = 40,
                SortType = VacancySearchSortType.ClosingDate
            };
        }

        private static TraineeshipSearchParameters GetPostedDateSearchParameters()
        {
            var searchParameters = GetCommonSearchParameters();
 
            searchParameters.SortType = VacancySearchSortType.RecentlyAdded;

            return searchParameters;
        }
    }
}