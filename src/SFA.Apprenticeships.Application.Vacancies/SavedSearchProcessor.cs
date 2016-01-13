namespace SFA.Apprenticeships.Application.Vacancies
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Entities.Candidates;
    using Domain.Entities.Communication;
    using Domain.Entities.Locations;
    using Domain.Entities.Users;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Extensions;
    using Factories;
    using Interfaces.Locations;
    using SFA.Infrastructure.Interfaces;
    using Interfaces.Vacancies;
    using Vacancy;

    public class SavedSearchProcessor : ISavedSearchProcessor
    {
        private readonly ISavedSearchReadRepository _savedSearchReadRepository;
        private readonly IServiceBus _serviceBus;
        private readonly IUserReadRepository _userReadRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ILocationSearchService _locationSearchService;
        private readonly IVacancySearchProvider<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters> _vacancySearchProvider;
        private readonly ISavedSearchAlertRepository _savedSearchAlertRepository;
        private readonly ISavedSearchWriteRepository _savedSearchWriteRepository;
        private readonly ILogService _logService;

        public SavedSearchProcessor(
            ISavedSearchReadRepository savedSearchReadRepository,
            IServiceBus serviceBus,
            IUserReadRepository userReadRepository,
            ICandidateReadRepository candidateReadRepository,
            ILocationSearchService locationSearchService,
            IVacancySearchProvider<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters> vacancySearchProvider,
            ISavedSearchAlertRepository savedSearchAlertRepository,
            ISavedSearchWriteRepository savedSearchWriteRepository,
            ILogService logService)
        {
            _savedSearchReadRepository = savedSearchReadRepository;
            _serviceBus = serviceBus;
            _userReadRepository = userReadRepository;
            _candidateReadRepository = candidateReadRepository;
            _locationSearchService = locationSearchService;
            _vacancySearchProvider = vacancySearchProvider;
            _savedSearchAlertRepository = savedSearchAlertRepository;
            _savedSearchWriteRepository = savedSearchWriteRepository;
            _logService = logService;
        }

        public void QueueCandidateSavedSearches()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            var candidateIds = _savedSearchReadRepository.GetCandidateIds();

            var message = string.Format("Querying candidate saved searches took {0}", stopwatch.Elapsed);

            var counter = 0;
            Parallel.ForEach(candidateIds, candidateId =>
            {
                var candidateSavedSearches = new CandidateSavedSearches
                {
                    CandidateId = candidateId
                };
                _serviceBus.PublishMessage(candidateSavedSearches);
                Interlocked.Increment(ref counter);
            });

            stopwatch.Stop();
            message += string.Format(". Queuing {0} candidate saved searches took {1}", counter, stopwatch.Elapsed);
            if (stopwatch.ElapsedMilliseconds > 60000)
            {
                _logService.Warn(message);
            }
            else
            {
                _logService.Info(message);
            }
        }

        public void ProcessCandidateSavedSearches(CandidateSavedSearches candidateSavedSearches)
        {
            var candidateId = candidateSavedSearches.CandidateId;

            var user = _userReadRepository.Get(candidateId);
            if (!user.IsActive()) return;

            var candidate = _candidateReadRepository.Get(candidateId);
            if (!candidate.ShouldSendSavedSearchAlerts()) return;

            var savedSearches = _savedSearchReadRepository.GetForCandidate(candidateId);
            if (savedSearches == null || !savedSearches.Any(s => s.AlertsEnabled)) return;

            foreach (var savedSearch in savedSearches)
            {
                if (!HasGeoLocation(savedSearch)) { continue; }

                var searchParameters = SearchParametersFactory.Create(savedSearch);
                var searchResults = _vacancySearchProvider.FindVacancies(searchParameters);
                var results = searchResults.Results.ToList();

                if (results.Count == 0)
                {
                    _logService.Info("Saved search with id {0} returned no results", savedSearch.EntityId);
                    continue;
                }

                var resultsHash = results.GetResultsHash();

                if (savedSearch.LastResultsHash != resultsHash)
                {
                    _logService.Info("Saved search with id {0} returned new results", savedSearch.EntityId);

                    //Results are new
                    savedSearch.LastResultsHash = resultsHash;
                    //todo: once we have the vacancy posted date (March 2015) we may store this instead of the processed date
                    savedSearch.DateProcessed = DateTime.UtcNow;

                    if (savedSearch.AlertsEnabled)
                    {
                        var savedSearchAlert = _savedSearchAlertRepository.GetUnsentSavedSearchAlert(savedSearch) ?? new SavedSearchAlert {Parameters = savedSearch};
                        savedSearchAlert.Results = results;

                        _savedSearchAlertRepository.Save(savedSearchAlert);
                    }

                    _savedSearchWriteRepository.Save(savedSearch);
                }
            }
        }

        private bool HasGeoLocation(SavedSearch savedSearch)
        {
            if (!savedSearch.HasGeoPoint())
            {
                var locations = _locationSearchService.FindLocation(savedSearch.Location);
                var locationsList = locations == null ? new List<Location>() : locations.ToList();

                if (locationsList.Any())
                {
                    var location = locationsList.First();

                    _logService.Info("Location {0} specified in saved search with id {1} was identified as {2}", savedSearch.Location, savedSearch.EntityId, location.Name);

                    savedSearch.Location = location.Name;
                    savedSearch.Latitude = location.GeoPoint.Latitude;
                    savedSearch.Longitude = location.GeoPoint.Longitude;
                    savedSearch.Hash = savedSearch.GetLatLonLocHash();

                    //Update saved search now we know the lat/long/hash
                    _savedSearchWriteRepository.Save(savedSearch);
                }
                else
                {
                    _logService.Info("Location {0} specified in saved search with id {1} could not be found", savedSearch.Location, savedSearch.EntityId);
                    return false;
                }
            }

            return true;
        }
    }
}
