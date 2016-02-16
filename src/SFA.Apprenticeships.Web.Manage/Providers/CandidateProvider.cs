namespace SFA.Apprenticeships.Web.Manage.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Application.Interfaces.Candidates;
    using Common.ViewModels;
    using Domain.Entities.Candidates;
    using SFA.Infrastructure.Interfaces;
    using ViewModels;

    public class CandidateProvider : ICandidateProvider
    {
        private readonly CultureInfo _dateCultureInfo = new CultureInfo("en-GB");

        private readonly ICandidateSearchService _candidateSearchService;
        private readonly IMapper _mapper;

        public CandidateProvider(ICandidateSearchService candidateSearchService, IMapper mapper)
        {
            _candidateSearchService = candidateSearchService;
            _mapper = mapper;
        }

        public CandidateSearchResultsViewModel SearchCandidates(CandidateSearchViewModel searchViewModel)
        {
            var dateOfBirth = string.IsNullOrEmpty(searchViewModel.DateOfBirth) ? (DateTime?)null : DateTime.Parse(searchViewModel.DateOfBirth, _dateCultureInfo);
            var request = new CandidateSearchRequest(searchViewModel.FirstName, searchViewModel.LastName, dateOfBirth, searchViewModel.Postcode);
            var candidates = _candidateSearchService.SearchCandidates(request) ?? new List<CandidateSummary>();

            var results = new CandidateSearchResultsViewModel
            {
                SearchViewModel = searchViewModel,
                Candidates = new PageableViewModel<CandidateSummaryViewModel>
                {
                    Page = candidates.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ThenBy(c => c.Address.Postcode).ThenBy(c => c.Address.AddressLine1).Skip((searchViewModel.CurrentPage - 1) * searchViewModel.PageSize).Take(searchViewModel.PageSize).Select(c => _mapper.Map<CandidateSummary, CandidateSummaryViewModel>(c)).ToList(),
                    ResultsCount = candidates.Count,
                    CurrentPage = searchViewModel.CurrentPage,
                    TotalNumberOfPages = candidates.Count == 0 ? 1 : (int)Math.Ceiling((double)candidates.Count / searchViewModel.PageSize)
                }
            };

            return results;
        }
    }
}