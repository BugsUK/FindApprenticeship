namespace SFA.Apprenticeships.Web.Manage.Providers
{
    using System;
    using System.Globalization;
    using Application.Interfaces.Candidates;
    using ViewModels;

    public class CandidateProvider : ICandidateProvider
    {
        private readonly CultureInfo _dateCultureInfo = new CultureInfo("en-GB");

        private readonly ICandidateSearchService _candidateSearchService;

        public CandidateProvider(ICandidateSearchService candidateSearchService)
        {
            _candidateSearchService = candidateSearchService;
        }

        public CandidateSearchResultsViewModel SearchCandidates(CandidateSearchViewModel searchViewModel)
        {
            var dateOfBirth = DateTime.Parse(searchViewModel.DateOfBirth, _dateCultureInfo);
            var request = new CandidateSearchRequest(searchViewModel.FirstName, searchViewModel.LastName, dateOfBirth, searchViewModel.Postcode);
            var candidates = _candidateSearchService.SearchCandidates(request);

            var results = new CandidateSearchResultsViewModel
            {
                SearchViewModel = searchViewModel
            };

            return results;
        }
    }
}