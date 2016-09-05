namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using Constants.Pages;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies.Traineeships;
    using SFA.Apprenticeships.Application.Interfaces;
    using System;
    using ViewModels.VacancySearch;

    public class TraineeshipVacancyProvider : ITraineeshipVacancyProvider
    {
        private readonly IVacancySearchService<TraineeshipSearchResponse, TraineeshipVacancyDetail, TraineeshipSearchParameters> _traineeshipSearchService;
        private readonly ICandidateService _candidateService;
        private readonly IMapper _traineeshipSearchMapper;
        private readonly ILogService _logger;

        public TraineeshipVacancyProvider(
            IVacancySearchService<TraineeshipSearchResponse, TraineeshipVacancyDetail, TraineeshipSearchParameters> traineeshipSearchService,
            ICandidateService candidateService,
            IMapper traineeshipSearchMapper,
            ILogService logger)
        {
            _traineeshipSearchService = traineeshipSearchService;
            _candidateService = candidateService;
            _traineeshipSearchMapper = traineeshipSearchMapper;
            _logger = logger;
        }

        public TraineeshipSearchResponseViewModel FindVacancies(TraineeshipSearchViewModel search)
        {
            _logger.Debug("Calling SearchProvider to find traineeship vacancies.");

            var searchLocation = _traineeshipSearchMapper.Map<TraineeshipSearchViewModel, Location>(search);

            try
            {
                var searchRequest = new TraineeshipSearchParameters
                {
                    Location = searchLocation,
                    PageNumber = search.PageNumber,
                    PageSize = search.ResultsPerPage,
                    SearchRadius = search.WithinDistance,
                    SortType = search.SortType,
                    VacancyReference = search.ReferenceNumber
                };

                var searchResults = _traineeshipSearchService.Search(searchRequest);

                var searchResponse =
                    _traineeshipSearchMapper.Map<SearchResults<TraineeshipSearchResponse, TraineeshipSearchParameters>, TraineeshipSearchResponseViewModel>(
                        searchResults);

                searchResponse.TotalHits = searchResults.Total;
                searchResponse.PageSize = search.ResultsPerPage;
                searchResponse.VacancySearch = search;

                return searchResponse;
            }
            catch (CustomException ex)
            {
                // ReSharper disable once FormatStringProblem
                _logger.Error("Find traineeship vacancies failed. Check inner details for more info", ex);
                return new TraineeshipSearchResponseViewModel(VacancySearchResultsPageMessages.VacancySearchFailed);
            }
            catch (Exception e)
            {
                _logger.Error("Find traineeship vacancies failed. Check inner details for more info", e);
                throw;
            }
        }

        public TraineeshipVacancyDetailViewModel GetVacancyDetailViewModel(Guid? candidateId, int vacancyId)
        {
            _logger.Debug(
                "Calling TraineeshipVacancyDetailProvider to get the Vacancy detail View Model for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                var vacancyDetail = candidateId.HasValue ?
                    _candidateService.GetTraineeshipVacancyDetail(candidateId.Value, vacancyId) :
                    _traineeshipSearchService.GetVacancyDetails(vacancyId);

                if (vacancyDetail == null) return null;

                var vacancyDetailViewModel = _traineeshipSearchMapper.Map<TraineeshipVacancyDetail, TraineeshipVacancyDetailViewModel>(vacancyDetail);

                if (candidateId == null) return vacancyDetailViewModel;

                var traineeshipApplication = _candidateService.GetTraineeshipApplication(candidateId.Value, vacancyId);

                if (traineeshipApplication == null) return vacancyDetailViewModel;

                // If candidate has applied for vacancy, include the details in the view model.
                vacancyDetailViewModel.DateApplied = traineeshipApplication.DateApplied;

                return vacancyDetailViewModel;
            }
            catch (CustomException e)
            {
                var message =
                    string.Format("Get Traineeship Vacancy View Model failed for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);

                _logger.Error(message, e);

                return new TraineeshipVacancyDetailViewModel(TraineeshipVacancyDetailPageMessages.GetVacancyDetailFailed);
            }
            catch (Exception e)
            {
                var message =
                    string.Format("Get Traineeship Vacancy View Model failed for candidate ID: {0}, vacancy ID: {1}.",
                        candidateId, vacancyId);

                _logger.Error(message, e);
                throw;
            }
        }
    }
}