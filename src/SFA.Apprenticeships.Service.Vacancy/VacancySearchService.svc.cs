namespace SFA.Apprenticeships.Service.Vacancy
{
    using System;
    using System.Linq;
    using Application.Interfaces.Logging;
    using Application.Interfaces.Vacancies;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Infrastructure.Common.IoC;
    using Infrastructure.Elastic.Common.Configuration;
    using Infrastructure.Elastic.Common.IoC;
    using Infrastructure.Logging.IoC;
    using Infrastructure.VacancySearch;
    using Infrastructure.VacancySearch.Configuration;
    using Infrastructure.VacancySearch.IoC;
    using StructureMap;
    using Types;

    public class VacancySearchService : IVacancySearchService
    {
        private readonly ILogService _logService;
        private readonly IMapper _mapper;
        private readonly IConfigurationService _configurationService;

        public VacancySearchService()
        {
            var container = new Container(c =>
            {
                c.AddRegistry<LoggingRegistry>();
                c.AddRegistry<CommonRegistry>();
                c.AddRegistry<ElasticsearchCommonRegistry>();
                c.AddRegistry<VacancySearchRegistry>();
            });

            _logService = container.GetInstance<ILogService>();
            _configurationService = container.GetInstance<IConfigurationService>();
            _mapper = container.GetInstance<IMapper>();
        }

        public SearchResponse Search(SearchRequest request)
        {
            var searchFactorConfiguration = new SearchFactorConfiguration
            {
                DescriptionFactors = GetFactorsFromRequest(SearchFactorKeys.Description, request),
                EmployerFactors = GetFactorsFromRequest(SearchFactorKeys.Employer, request),
                JobTitleFactors = GetFactorsFromRequest(SearchFactorKeys.JobTitle, request),
                ProviderFactors = GetFactorsFromRequest(SearchFactorKeys.Provider, request)
            };

            var configurationService = new VacancySearchConfigurationService(
                _configurationService, searchFactorConfiguration);

            var elasticsearchClientFactory = new ElasticsearchClientFactory(
                configurationService, _logService);

            var searchProvider = new ApprenticeshipsSearchProvider(
                elasticsearchClientFactory, _mapper, configurationService, _logService);

            var parameters = new ApprenticeshipSearchParameters
            {
                PageNumber = 1,
                PageSize = 50,
                SearchField = GetSearchFieldFromRequest(request),
                Keywords = request.SearchTerms
            };

            var vacancies = searchProvider.FindVacancies(parameters);

            return new SearchResponse
            {
                Request = request,
                SearchResults = vacancies.Results.ToArray(),
                Total = vacancies.Total,
                SearchFactorConfiguration = configurationService.Get<SearchFactorConfiguration>(),
                Parameters = parameters
            };
        }

        #region Helpers

        private static ApprenticeshipSearchField GetSearchFieldFromRequest(SearchRequest request)
        {
            ApprenticeshipSearchField searchField;

            if (Enum.TryParse(request.SearchField, out searchField))
            {
                return searchField;
            }

            return ApprenticeshipSearchField.All;
        }

        private static SearchTermFactors GetFactorsFromRequest(string searchFactorKey, SearchRequest request)
        {
            var defaultSearchTermFactors = new SearchTermFactors();

            if (request.SearchFactorsParameters == null)
            {
                return defaultSearchTermFactors;
            }

            var searchFactors = request.SearchFactorsParameters.FirstOrDefault(each => each.Name == searchFactorKey);

            return searchFactors == null ? defaultSearchTermFactors : searchFactors.Value;
        }

        #endregion
    }
}