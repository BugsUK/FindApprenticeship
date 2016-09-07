namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using Constants.Pages;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Application.Interfaces;
    using ViewModels.VacancySearch;

    public class ApprenticeshipVacancyProvider : IApprenticeshipVacancyProvider
    {
        private readonly IVacancySearchService<ApprenticeshipSearchResponse, ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters> _apprenticeshipSearchService;
        private readonly ICandidateService _candidateService;
        private readonly IMapper _apprenticeshipSearchMapper;
        private readonly ILogService _logger;
        private readonly ICandidateVacancyService _candidateVacancyService;

        private static class ResultsKeys
        {
            public const string NonNational = "NonNational";
            public const string National = "National";
            public const string Unfiltered = "Unfiltered";
        }

        public ApprenticeshipVacancyProvider(
            IVacancySearchService<ApprenticeshipSearchResponse, ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters> apprenticeshipSearchService,
            ICandidateService candidateService,
            IMapper apprenticeshipSearchMapper,
            ILogService logger, ICandidateVacancyService candidateVacancyService)
        {
            _apprenticeshipSearchService = apprenticeshipSearchService;
            _candidateService = candidateService;
            _apprenticeshipSearchMapper = apprenticeshipSearchMapper;
            _logger = logger;
            _candidateVacancyService = candidateVacancyService;
        }

        public ApprenticeshipSearchResponseViewModel FindVacancies(ApprenticeshipSearchViewModel search)
        {
            _logger.Debug("Calling SearchProvider to find apprenticeship vacancies.");

            var searchLocation = _apprenticeshipSearchMapper.Map<ApprenticeshipSearchViewModel, Location>(search);

            try
            {
                string vacancyReference;
                var isVacancyReference = VacancyHelper.TryGetVacancyReference(search.Keywords, out vacancyReference);

                if ((search.SearchField == ApprenticeshipSearchField.All.ToString() && isVacancyReference) || search.SearchField == ApprenticeshipSearchField.ReferenceNumber.ToString())
                {
                    if (isVacancyReference)
                    {
                        var searchParameters = new ApprenticeshipSearchParameters
                        {
                            VacancyReference = vacancyReference,
                            PageNumber = 1,
                            PageSize = 1
                        };

                        var searchResults = _apprenticeshipSearchService.Search(searchParameters);

                        //Expect only a single result. Any other number should be interpreted as no results
                        if (searchResults.Total == 1)
                        {
                            var exactMatchResponse = _apprenticeshipSearchMapper.Map<SearchResults<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters>, ApprenticeshipSearchResponseViewModel>(searchResults);
                            exactMatchResponse.ExactMatchFound = true;
                            return exactMatchResponse;
                        }

                        if (searchResults.Total > 1)
                        {
                            _logger.Info("{0} results found for Vacancy Reference Number {1} parsed from {2}. Expected 0 or 1", searchResults.Total, vacancyReference, search.Keywords);
                        }
                    }

                    var response = new ApprenticeshipSearchResponseViewModel
                    {
                        Vacancies = new List<ApprenticeshipVacancySummaryViewModel>(),
                        VacancySearch = search
                    };

                    return response;
                }

                var results = ProcessNationalAndNonNationalSearches(search, searchLocation);

                var nationalResults = results[ResultsKeys.National];
                var nonNationalResults = results[ResultsKeys.NonNational];
                var unfilteredResults = results[ResultsKeys.Unfiltered];

                var nationalResponse = _apprenticeshipSearchMapper.Map<SearchResults<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters>, ApprenticeshipSearchResponseViewModel>(nationalResults);

                nationalResponse.VacancySearch = search;

                var nonNationalResponse = _apprenticeshipSearchMapper.Map<SearchResults<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters>, ApprenticeshipSearchResponseViewModel>(nonNationalResults);

                nonNationalResponse.VacancySearch = search;

                if (search.LocationType == ApprenticeshipLocationType.NonNational)
                {
                    nonNationalResponse.TotalLocalHits = nonNationalResults.Total;
                    nonNationalResponse.TotalNationalHits = nationalResults.Total;
                    nonNationalResponse.PageSize = search.ResultsPerPage;

                    if (nonNationalResults.Total == 0 && nationalResults.Total > 0)
                    {
                        nonNationalResponse.Vacancies = nationalResponse.Vacancies;

                        var vacancySearch = nonNationalResponse.VacancySearch;

                        if (vacancySearch.SearchAction == SearchAction.Search || vacancySearch.SortType == VacancySearchSortType.Distance)
                        {
                            vacancySearch.SortType = string.IsNullOrWhiteSpace(vacancySearch.Keywords) ? VacancySearchSortType.ClosingDate : VacancySearchSortType.Relevancy;
                        }

                        vacancySearch.LocationType = ApprenticeshipLocationType.National;
                        SetAggregationResults(nonNationalResponse, nationalResults.AggregationResults, unfilteredResults.AggregationResults);
                    }
                    else
                    {
                        SetAggregationResults(nonNationalResponse, nonNationalResults.AggregationResults, unfilteredResults.AggregationResults);
                    }

                    return nonNationalResponse;
                }

                nationalResponse.TotalLocalHits = nonNationalResults.Total;
                nationalResponse.TotalNationalHits = nationalResults.Total;
                nationalResponse.PageSize = search.ResultsPerPage;

                if (nationalResults.Total == 0 && nonNationalResults.Total > 0)
                {
                    nationalResponse.Vacancies = nonNationalResponse.Vacancies;
                    SetAggregationResults(nonNationalResponse, nonNationalResults.AggregationResults, unfilteredResults.AggregationResults);
                }
                else
                {
                    SetAggregationResults(nonNationalResponse, nationalResults.AggregationResults, unfilteredResults.AggregationResults);
                }

                return nationalResponse;
            }
            catch (CustomException ex)
            {
                _logger.Error("Find apprenticeship vacancies failed. Check inner details for more info", ex);
                return new ApprenticeshipSearchResponseViewModel(VacancySearchResultsPageMessages.VacancySearchFailed);
            }
            catch (Exception e)
            {
                _logger.Error("Find apprenticeship vacancies failed. Check inner details for more info", e);
                throw;
            }
        }

        public ApprenticeshipVacancyDetailViewModel GetVacancyDetailViewModel(Guid? candidateId, int vacancyId)
        {
            _logger.Debug(
                "Calling ApprenticeshipVacancyDetailProvider to get the Vacancy detail View Model for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                var vacancyDetail = candidateId.HasValue ?
                    _candidateService.GetApprenticeshipVacancyDetail(candidateId.Value, vacancyId) :
                    _apprenticeshipSearchService.GetVacancyDetails(vacancyId);

                if (vacancyDetail == null) return null;

                var vacancyDetailViewModel = _apprenticeshipSearchMapper.Map<ApprenticeshipVacancyDetail, ApprenticeshipVacancyDetailViewModel>(vacancyDetail);

                if (candidateId == null) return vacancyDetailViewModel;

                var applicationDetails = _candidateService.GetApplication(candidateId.Value, vacancyId);
                if (applicationDetails == null) return vacancyDetailViewModel;

                // If candidate has applied for vacancy, include the details in the view model.
                vacancyDetailViewModel.CandidateApplicationStatus = applicationDetails.Status;
                vacancyDetailViewModel.DateApplied = applicationDetails.DateApplied;

                return vacancyDetailViewModel;
            }
            catch (CustomException e)
            {
                var message = string.Format("Get Apprenticeship Vacancy View Model failed for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);

                _logger.Error(message, e);

                return new ApprenticeshipVacancyDetailViewModel(ApprenticeshipVacancyDetailPageMessages.GetVacancyDetailFailed);
            }
            catch (Exception e)
            {
                var message = string.Format("Get Apprenticeship Vacancy View Model failed for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);

                _logger.Error(message, e);
                throw;
            }
        }

        public ApprenticeshipVacancyDetailViewModel GetVacancyDetailViewModelByReferenceNumber(Guid? candidateId, int vacancyReferenceNumber)
        {
            var vacancyId = _apprenticeshipSearchService.GetVacancyId(vacancyReferenceNumber);
            return GetVacancyDetailViewModel(candidateId, vacancyId);
        }

        public ApprenticeshipVacancyDetailViewModel IncrementClickThroughFor(int vacancyId)
        {
            _logger.Debug(
                "Calling ApprenticeshipVacancyDetailProvider to increment click-throughs for vacancy ID: {0}.", vacancyId);

            var vacancyDetail = _apprenticeshipSearchService.GetVacancyDetails(vacancyId);

            if (vacancyDetail == null) return null;

            var vacancyDetailViewModel = _apprenticeshipSearchMapper.Map<ApprenticeshipVacancyDetail, ApprenticeshipVacancyDetailViewModel>(vacancyDetail);

            try
            {
                //Incrementing this value should be able to fail and the user still redirected to the offline URL
                _candidateVacancyService.IncrementOfflineApplicationClickThrough(vacancyId);
            }
            catch (Exception e)
            {
                var message = $"Increment click-throughs failed for vacancy ID: {vacancyId}.";

                _logger.Error(message, e);
            }

            return vacancyDetailViewModel;
        }

        private Dictionary<string, SearchResults<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters>> ProcessNationalAndNonNationalSearches(
            ApprenticeshipSearchViewModel search, Location searchLocation)
        {
            VacancySearchSortType nationalSortType, nonNationalSortType;
            int nationalPageNumber, nonNationalPageNumber;

            if (search.LocationType == ApprenticeshipLocationType.National)
            {
                nationalSortType = search.SortType;
                nonNationalSortType = VacancySearchSortType.Distance;
                nationalPageNumber = search.PageNumber;
                nonNationalPageNumber = 1;
            }
            else
            {
                nonNationalSortType = search.SortType;
                nationalSortType = VacancySearchSortType.ClosingDate;
                nationalPageNumber = 1;
                nonNationalPageNumber = search.PageNumber;
            }

            var searchField = (ApprenticeshipSearchField) Enum.Parse(typeof (ApprenticeshipSearchField), search.SearchField);

            var searchParameters = new Dictionary<string, ApprenticeshipSearchParameters>
            {
                {
                    ResultsKeys.National,
                    new ApprenticeshipSearchParameters
                    {
                        SearchField = searchField,
                        Keywords = search.Keywords,
                        Location = null,
                        PageNumber = nationalPageNumber,
                        PageSize = search.ResultsPerPage,
                        SearchRadius = search.WithinDistance,
                        SortType = nationalSortType,
                        VacancyLocationType = ApprenticeshipLocationType.National,
                        ApprenticeshipLevel = search.ApprenticeshipLevel,
                        CategoryCode = search.Category,
                        SubCategoryCodes = search.SubCategories
                    }
                },
                {
                    ResultsKeys.NonNational,
                    new ApprenticeshipSearchParameters
                    {
                        SearchField = searchField,
                        Keywords = search.Keywords,
                        Location = searchLocation,
                        PageNumber = nonNationalPageNumber,
                        PageSize = search.ResultsPerPage,
                        SearchRadius = search.WithinDistance,
                        SortType = nonNationalSortType,
                        VacancyLocationType = ApprenticeshipLocationType.NonNational,
                        ApprenticeshipLevel = search.ApprenticeshipLevel,
                        CategoryCode = search.Category,
                        SubCategoryCodes = search.SubCategories
                    }
                },
                {
                    ResultsKeys.Unfiltered,
                    new ApprenticeshipSearchParameters
                    {
                        SearchField = searchField,
                        Keywords = null,
                        Location = searchLocation,
                        PageNumber = 1,
                        PageSize = 1,
                        SearchRadius = search.WithinDistance,
                        SortType = VacancySearchSortType.ClosingDate,
                        VacancyLocationType = search.LocationType,
                        ApprenticeshipLevel = "All",
                        CategoryCode = null,
                        SubCategoryCodes = null
                    }
                }
            };

            var resultCollection = new ConcurrentDictionary<string, SearchResults<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters>>();

            Parallel.ForEach(searchParameters, parameters =>
            {
                var searchResults = _apprenticeshipSearchService.Search(parameters.Value);

                resultCollection[parameters.Key] = searchResults;
            });

            return resultCollection.ToDictionary(each => each.Key, each => each.Value);
        }

        private static void SetAggregationResults(
            ApprenticeshipSearchResponseViewModel response,
            IEnumerable<AggregationResult> aggregationResults,
            IEnumerable<AggregationResult> unfilteredAggregationResults)
        {
            if (aggregationResults == null || response.VacancySearch == null || response.VacancySearch.Categories == null) return;

            var aggregationResultsList = aggregationResults.ToList();
            var unfilteredAggregationResultsList = unfilteredAggregationResults.ToList();

            foreach (var category in response.VacancySearch.Categories)
            {
                foreach (var subCategory in category.SubCategories)
                {
                    var aggregationResult = aggregationResultsList.SingleOrDefault(each => each.Code == subCategory.CodeName);

                    if (aggregationResult == null && category.CodeName != response.VacancySearch.Category)
                    {
                        aggregationResult = unfilteredAggregationResultsList.SingleOrDefault(each => each.Code == subCategory.CodeName);
                    }

                    subCategory.Count = aggregationResult?.Count ?? 0;
                }

                category.Count = category.SubCategories.Sum(each => each.Count);
            }
        }
    }
}