namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Logging;
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using Constants.Pages;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Mapping;
    using ViewModels.VacancySearch;

    public class ApprenticeshipVacancyProvider : IApprenticeshipVacancyProvider
    {
        private readonly IVacancySearchService<ApprenticeshipSearchResponse, ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters> _apprenticeshipSearchService;
        private readonly ICandidateService _candidateService;
        private readonly IMapper _apprenticeshipSearchMapper;
        private readonly ILogService _logger;

        public ApprenticeshipVacancyProvider(
            IVacancySearchService<ApprenticeshipSearchResponse, ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters> apprenticeshipSearchService,
            ICandidateService candidateService,
            IMapper apprenticeshipSearchMapper,
            ILogService logger)
        {
            _apprenticeshipSearchService = apprenticeshipSearchService;
            _candidateService = candidateService;
            _apprenticeshipSearchMapper = apprenticeshipSearchMapper;
            _logger = logger;
        }

        public ApprenticeshipSearchResponseViewModel FindVacancies(ApprenticeshipSearchViewModel search)
        {
            _logger.Debug("Calling SearchProvider to find apprenticeship vacancies.");

            var searchLocation = _apprenticeshipSearchMapper.Map<ApprenticeshipSearchViewModel, Location>(search);

            try
            {
                string vacancyReference;
                if ((search.SearchField == ApprenticeshipSearchField.All.ToString() || search.SearchField == ApprenticeshipSearchField.ReferenceNumber.ToString())
                    && VacancyHelper.TryGetVacancyReference(search.Keywords, out vacancyReference))
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
                        _logger.Warn("{0} results found for Vacancy Reference Number {1} parsed from {2}. Expected 0 or 1", searchResults.Total, vacancyReference, search.Keywords);
                    }
                    var response = new ApprenticeshipSearchResponseViewModel
                    {
                        Vacancies = new List<ApprenticeshipVacancySummaryViewModel>(),
                        VacancySearch = search
                    };
                    return response;
                }

                var results = ProcessNationalAndNonNationalSearches(search, searchLocation);

                var nationalResults = results.Single(r => r.SearchParameters.VacancyLocationType == ApprenticeshipLocationType.National);

                var nonNationalResults = results.Single(r => r.SearchParameters.VacancyLocationType == ApprenticeshipLocationType.NonNational);

                var nationalResponse =
                    _apprenticeshipSearchMapper.Map<SearchResults<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters>, ApprenticeshipSearchResponseViewModel>(
                        nationalResults);
                nationalResponse.VacancySearch = search;

                var nonNationlResponse =
                    _apprenticeshipSearchMapper.Map<SearchResults<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters>, ApprenticeshipSearchResponseViewModel>(
                        nonNationalResults);
                nonNationlResponse.VacancySearch = search;

                if (search.LocationType == ApprenticeshipLocationType.NonNational)
                {
                    nonNationlResponse.TotalLocalHits = nonNationalResults.Total;
                    nonNationlResponse.TotalNationalHits = nationalResults.Total;
                    nonNationlResponse.PageSize = search.ResultsPerPage;

                    if (nonNationalResults.Total == 0 && nationalResults.Total > 0)
                    {
                        nonNationlResponse.Vacancies = nationalResponse.Vacancies;
                        var vacancySearch = nonNationlResponse.VacancySearch;
                        if (vacancySearch.SearchAction == SearchAction.Search || vacancySearch.SortType == VacancySearchSortType.Distance)
                        {
                            vacancySearch.SortType = string.IsNullOrWhiteSpace(vacancySearch.Keywords) ? VacancySearchSortType.ClosingDate : VacancySearchSortType.Relevancy;
                        }
                        vacancySearch.LocationType = ApprenticeshipLocationType.National;
                        SetAggregationResults(nonNationlResponse, nationalResults.AggregationResults);
                    }
                    else
                    {
                        SetAggregationResults(nonNationlResponse, nonNationalResults.AggregationResults);
                    }

                    return nonNationlResponse;
                }

                nationalResponse.TotalLocalHits = nonNationalResults.Total;
                nationalResponse.TotalNationalHits = nationalResults.Total;
                nationalResponse.PageSize = search.ResultsPerPage;

                if (nationalResults.Total == 0 && nonNationalResults.Total != 0)
                {
                    nationalResponse.Vacancies = nonNationlResponse.Vacancies;
                    SetAggregationResults(nonNationlResponse, nonNationalResults.AggregationResults);
                }
                else
                {
                    SetAggregationResults(nonNationlResponse, nationalResults.AggregationResults);
                }

                return nationalResponse;
            }
            catch (CustomException ex)
            {
                // ReSharper disable once FormatStringProblem
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

        private SearchResults<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters>[] ProcessNationalAndNonNationalSearches(
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

            var searchparameters = new List<ApprenticeshipSearchParameters>
            {
                new ApprenticeshipSearchParameters
                {
                    SearchField = (ApprenticeshipSearchField)Enum.Parse(typeof(ApprenticeshipSearchField), search.SearchField),
                    Keywords = search.Keywords,
                    Location = null,
                    PageNumber = nationalPageNumber,
                    PageSize = search.ResultsPerPage,
                    SearchRadius = search.WithinDistance,
                    SortType = nationalSortType,
                    VacancyLocationType = ApprenticeshipLocationType.National,
                    ApprenticeshipLevel = search.ApprenticeshipLevel,
                    Sector = search.Category,
                    Frameworks = search.SubCategories
                },
                new ApprenticeshipSearchParameters
                {
                    SearchField = (ApprenticeshipSearchField)Enum.Parse(typeof(ApprenticeshipSearchField), search.SearchField),
                    Keywords = search.Keywords,
                    Location = searchLocation,
                    PageNumber = nonNationalPageNumber,
                    PageSize = search.ResultsPerPage,
                    SearchRadius = search.WithinDistance,
                    SortType = nonNationalSortType,
                    VacancyLocationType = ApprenticeshipLocationType.NonNational,
                    ApprenticeshipLevel = search.ApprenticeshipLevel,
                    Sector = search.Category,
                    Frameworks = search.SubCategories
                }
            };

            var resultCollection = new ConcurrentBag<SearchResults<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters>>();
            Parallel.ForEach(searchparameters,
                parameters =>
                {
                    var searchResults = _apprenticeshipSearchService.Search(parameters);
                    resultCollection.Add(searchResults);
                });

            return resultCollection.ToArray();
        }

        private static void SetAggregationResults(ApprenticeshipSearchResponseViewModel response, IEnumerable<AggregationResult> aggregationResults)
        {
            if (aggregationResults == null || response.VacancySearch == null || response.VacancySearch.Categories == null) return;

            var aggregationResultsList = aggregationResults.ToList();

            foreach (var category in response.VacancySearch.Categories)
            {
                foreach (var subCategory in category.SubCategories)
                {
                    var aggregationResult = aggregationResultsList.SingleOrDefault(ar => ar.Code == subCategory.CodeName);
                    if (aggregationResult != null) subCategory.Count = aggregationResult.Count;
                }
            }

            foreach (var category in response.VacancySearch.Categories.Where(c => c.CodeName == response.VacancySearch.Category || c.SubCategories.Any(sc => sc.Count.HasValue)))
            {
                foreach (var subCategory in category.SubCategories)
                {
                    if (!subCategory.Count.HasValue) subCategory.Count = 0;
                }
            }
        }
    }
}