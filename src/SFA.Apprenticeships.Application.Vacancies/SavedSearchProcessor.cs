namespace SFA.Apprenticeships.Application.Vacancies
{
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Entities.Candidates;
    using Domain.Entities.Communication;
    using Domain.Entities.Users;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Entities;
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
        private readonly ILogService _logService;

        public SavedSearchProcessor(ISavedSearchReadRepository savedSearchReadRepository, IMessageBus messageBus, IUserReadRepository userReadRepository, ICandidateReadRepository candidateReadRepository, IVacancySearchProvider<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters> vacancySearchProvider, ISavedSearchAlertRepository savedSearchAlertRepository, ILogService logService)
        {
            _savedSearchReadRepository = savedSearchReadRepository;
            _messageBus = messageBus;
            _userReadRepository = userReadRepository;
            _candidateReadRepository = candidateReadRepository;
            _vacancySearchProvider = vacancySearchProvider;
            _savedSearchAlertRepository = savedSearchAlertRepository;
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
            var message = string.Format("Queuing {0} candidate saved searches took {1}", counter, stopwatch);
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

            //todo: Should we run searches for all saved searches or just active ones?
            foreach (var savedSearch in savedSearches.Where(s => s.AlertsEnabled))
            {
                var results = _vacancySearchProvider.FindVacancies(null);

                var savedSearchAlert = new SavedSearchAlert
                {
                    Parameters = savedSearch
                };

                _savedSearchAlertRepository.Save(savedSearchAlert);
            }


            //todo: 1.8: retrieve top 5 most recent search results for each saved search (use IVacancySearchProvider to execute)
            // write results to SavedSearchAlerts repo, upsert based on candidate+savedsearch+date (assuming for now only runs once per day)
            // note: should not write if no new results (check LastResultsHash if any) since last *sent* search
            // note: update SavedSearch.LastResultsHash to ensure alerts are only triggered if there are new results
            // note: update SavedSearch.DateProcessed with datetime processed (if changes)

            //todo: once we have the vacancy posted date (March 2015) we may store this instead of the processed date
        }
    }
}
