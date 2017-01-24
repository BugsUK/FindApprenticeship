namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.VacancySearch
{
    using System.Linq;
    using SFA.Infrastructure.Interfaces;
    using Application.Interfaces.Vacancies;
    using Common.IoC;
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies;
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

    //TODO: Create and delete test data directly in index
    [TestFixture]
    public class ApprenticeshipVacancySearchTests
    {
        private const string RetailAndCommercialEnterprise = "SSAT1.HBY"; //"Retail and Commercial Enterprise";
        private IElasticsearchClientFactory _elasticsearchClientFactory;
        private IConfigurationService _configurationService;
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
            _configurationService = container.GetInstance<IConfigurationService>();
            _mapper = container.GetInstance<IMapper>();
        }

        [Test, Category("Integration")]
        public void ShouldReturnFrameworksCount()
        {
            var vacancySearchProvider = new ApprenticeshipsSearchProvider(_elasticsearchClientFactory, _mapper, _configurationService, _logger.Object);

            var vacancies = vacancySearchProvider.FindVacancies(GetCommonSearchParameters());

            vacancies.AggregationResults.Should().HaveCount(c => c > 0);
        }

        [Test, Category("Integration")]
        public void ShouldSearchBySector()
        {

            var vacancySearchProvider = new ApprenticeshipsSearchProvider(_elasticsearchClientFactory, _mapper, _configurationService, _logger.Object);

            var searchParameters = GetCommonSearchParameters();
            searchParameters.CategoryCode = RetailAndCommercialEnterprise;

            var vacancies = vacancySearchProvider.FindVacancies(searchParameters);

            vacancies.AggregationResults.Should().HaveCount(c => c > 0);
        }

        [Test, Category("Integration")]
        public void ShouldSearchBySectorAndFramework()
        {
            var vacancySearchProvider = new ApprenticeshipsSearchProvider(_elasticsearchClientFactory, _mapper, _configurationService, _logger.Object);

            var searchParameters = GetCommonSearchParameters();
            searchParameters.CategoryCode = RetailAndCommercialEnterprise;
            searchParameters.SubCategoryCodes = new[] {"582"};

            var vacancies = vacancySearchProvider.FindVacancies(searchParameters);

            vacancies.AggregationResults.Should().HaveCount(c => c > 0);
        }

        [Test, Category("Integration")]
        //, Ignore("Failing because an exception")
        public void ShouldSearchAllEngland()
        {
            var vacancySearchProvider = new ApprenticeshipsSearchProvider(_elasticsearchClientFactory, _mapper, _configurationService, _logger.Object);

            var searchParameters = GetEnglandSearchParameters("it");

            var vacancies = vacancySearchProvider.FindVacancies(searchParameters);

            vacancies.Results.Should().HaveCount(c => c > 0);
        }

        [Test, Category("Integration")]
        public void ShouldSortByPostedDate()
        {
            var vacancySearchProvider = new ApprenticeshipsSearchProvider(_elasticsearchClientFactory, _mapper, _configurationService, _logger.Object);

            var searchParameters = GetPostedDateSearchParameters();

            var vacancies = vacancySearchProvider.FindVacancies(searchParameters);

            vacancies.Results.Should().BeInDescendingOrder(r => r.PostedDate);
        }

        private static ApprenticeshipSearchParameters GetCommonSearchParameters()
        {
            return new ApprenticeshipSearchParameters
            {
                ApprenticeshipLevel = "Intermediate",
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
                PageSize = 5,
                SearchRadius = 50,
                SortType = VacancySearchSortType.ClosingDate,
                VacancyLocationType = VacancyLocationType.NonNational
            };
        }

        private static ApprenticeshipSearchParameters GetEnglandSearchParameters(string keyword)
        {
            var searchParameters = GetCommonSearchParameters();
            searchParameters.SearchRadius = 0;
            searchParameters.Keywords = keyword;
            searchParameters.ApprenticeshipLevel = string.Empty;
            searchParameters.PageSize = 50;
            searchParameters.SortType = VacancySearchSortType.Relevancy;

            return searchParameters;
        }

        private static ApprenticeshipSearchParameters GetPostedDateSearchParameters()
        {
            var searchParameters = GetCommonSearchParameters();
            searchParameters.SortType = VacancySearchSortType.RecentlyAdded;

            return searchParameters;
        }
    }
}