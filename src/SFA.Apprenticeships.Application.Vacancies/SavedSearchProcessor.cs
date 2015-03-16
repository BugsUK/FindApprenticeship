namespace SFA.Apprenticeships.Application.Vacancies
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Entities.Candidates;
    using Domain.Entities.Communication;
    using Domain.Entities.Users;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Extensions;
    using Factories;
    using Interfaces.Logging;
    using Interfaces.Vacancies;
    using Vacancy;

    //todo: 1.8: move to Candidates? TBC
    public class SavedSearchProcessor : ISavedSearchProcessor
    {
        private readonly ISavedSearchReadRepository _savedSearchReadRepository;
        private readonly IMessageBus _messageBus;
        private readonly IUserReadRepository _userReadRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly IVacancySearchProvider<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters> _vacancySearchProvider;
        private readonly ISavedSearchAlertRepository _savedSearchAlertRepository;
        private readonly ISavedSearchWriteRepository _savedSearchWriteRepository;
        private readonly ILogService _logService;

        public SavedSearchProcessor(ISavedSearchReadRepository savedSearchReadRepository, IMessageBus messageBus, IUserReadRepository userReadRepository, ICandidateReadRepository candidateReadRepository, IVacancySearchProvider<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters> vacancySearchProvider, ISavedSearchAlertRepository savedSearchAlertRepository, ISavedSearchWriteRepository savedSearchWriteRepository, ILogService logService)
        {
            _savedSearchReadRepository = savedSearchReadRepository;
            _messageBus = messageBus;
            _userReadRepository = userReadRepository;
            _candidateReadRepository = candidateReadRepository;
            _vacancySearchProvider = vacancySearchProvider;
            _savedSearchAlertRepository = savedSearchAlertRepository;
            _savedSearchWriteRepository = savedSearchWriteRepository;
            _logService = logService;
        }

        public void QueueCandidateSavedSearches()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var counter = 0;
            var candidateIds = _savedSearchReadRepository.GetCandidateIds();

            Parallel.ForEach(candidateIds, candidateId =>
            {
                var candidateSavedSearches = new CandidateSavedSearches
                {
                    CandidateId = candidateId
                };
                _messageBus.PublishMessage(candidateSavedSearches);
                counter++;
            });

            stopwatch.Stop();
            var message = string.Format("Queuing {0} candidate saved searches took {1}", counter, stopwatch.Elapsed);
            if (stopwatch.ElapsedMilliseconds > 10000)
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
            if (!candidate.ShouldCommunicateWithCandidate() || !candidate.CommunicationPreferences.SendSavedSearchAlerts) return;

            var savedSearches = _savedSearchReadRepository.GetForCandidate(candidateId);
            if (savedSearches == null || !savedSearches.Any(s => s.AlertsEnabled)) return;

            foreach (var savedSearch in savedSearches)
            {
                var searchParameters = SearchParametersFactory.Create(savedSearch);
                var searchResults = _vacancySearchProvider.FindVacancies(searchParameters);
                var results = searchResults.Results.Select(r => new ApprenticeshipSummary(r)).ToList();
                var resultsHash = results.GetResultsHash();

                if (savedSearch.LastResultsHash != resultsHash)
                {
                    //Results are new
                    savedSearch.LastResultsHash = resultsHash;
                    //todo: once we have the vacancy posted date (March 2015) we may store this instead of the processed date
                    savedSearch.DateProcessed = DateTime.UtcNow;
                    _savedSearchWriteRepository.Save(savedSearch);

                    if (savedSearch.AlertsEnabled)
                    {
                        var savedSearchAlert = _savedSearchAlertRepository.GetUnsentSavedSearchAlert(savedSearch) ?? new SavedSearchAlert {Parameters = savedSearch};
                        savedSearchAlert.Results = results;

                        _savedSearchAlertRepository.Save(savedSearchAlert);
                    }
                }
            }
        }
    }
}
