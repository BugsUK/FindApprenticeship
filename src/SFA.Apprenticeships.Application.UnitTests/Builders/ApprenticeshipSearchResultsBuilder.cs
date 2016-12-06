namespace SFA.Apprenticeships.Application.UnitTests.Builders
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Vacancies;
    using Interfaces.Search;
    using Interfaces.Vacancies;
    using Ploeh.AutoFixture;

    public class ApprenticeshipSearchResultsBuilder
    {
        private ApprenticeshipSearchParameters _searchParameters;
        private IList<ApprenticeshipSearchResponse> _results = new List<ApprenticeshipSearchResponse>();

        public SearchResults<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters> Build()
        {
            var results = new SearchResults<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters>(_results.Count, _results, null, _searchParameters);
            return results;
        }

        public ApprenticeshipSearchResultsBuilder WithSearchParameters(ApprenticeshipSearchParameters searchParameters)
        {
            _searchParameters = searchParameters;
            return this;
        }

        public ApprenticeshipSearchResultsBuilder WithResults(IList<ApprenticeshipSearchResponse> results)
        {
            _results = results;
            return this;
        }

        public ApprenticeshipSearchResultsBuilder WithResultCount(int numOfResults)
        {
            _results = new Fixture().Build<ApprenticeshipSearchResponse>().CreateMany(numOfResults).ToList();
            return this;
        }
    }
}