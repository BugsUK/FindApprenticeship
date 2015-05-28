namespace SFA.Apprenticeships.Application.Candidates.Strategies
{
    using System.Collections.Generic;
    using System.Dynamic;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using Interfaces.Logging;

    public class HardDeleteStrategy : HousekeepingStrategy
    {
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly ICandidateWriteRepository _candidateWriteRepository;
        private readonly ISavedSearchReadRepository _savedSearchReadRepository;
        private readonly ISavedSearchWriteRepository _savedSearchWriteRepository;
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        private readonly ITraineeshipApplicationWriteRepository _traineeshipApplicationWriteRepository;
        private readonly IAuditRepository _auditRepository;
        private readonly ILogService _logService;

        public HardDeleteStrategy(IConfigurationService configurationService, IUserWriteRepository userWriteRepository,
            IAuthenticationRepository authenticationRepository, ICandidateWriteRepository candidateWriteRepository,
            ISavedSearchReadRepository savedSearchReadRepository, ISavedSearchWriteRepository savedSearchWriteRepository,
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository,
            ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository,
            ITraineeshipApplicationWriteRepository traineeshipApplicationWriteRepository,
            IAuditRepository auditRepository, ILogService logService)
            : base(configurationService)
        {
            _userWriteRepository = userWriteRepository;
            _authenticationRepository = authenticationRepository;
            _candidateWriteRepository = candidateWriteRepository;
            _savedSearchReadRepository = savedSearchReadRepository;
            _savedSearchWriteRepository = savedSearchWriteRepository;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
            _traineeshipApplicationWriteRepository = traineeshipApplicationWriteRepository;
            _auditRepository = auditRepository;
            _logService = logService;
        }

        protected override bool DoHandle(User user, Candidate candidate)
        {
            if (user.Status != UserStatuses.PendingDeletion) return false;

            if (!user.DateUpdated.HasValue) return false;

            var housekeepingCyclesSinceDateUpdated = GetHousekeepingCyclesSince(user.DateUpdated.Value);

            if(housekeepingCyclesSinceDateUpdated < Configuration.HardDeleteAccountAfterCycles) return false;

            var savedSearches = _savedSearchReadRepository.GetForCandidate(user.EntityId);
            var apprenticeshipApplications = _apprenticeshipApplicationReadRepository.GetForCandidate(user.EntityId);
            var traineeshipApplications = _traineeshipApplicationReadRepository.GetForCandidate(user.EntityId);

            Audit(user, candidate, savedSearches, apprenticeshipApplications, traineeshipApplications);

            //These methods aren't transactional however the code can cope with missing entities and so will self heal over time in the unlikely event of partial failure

            DeleteApprenticeshipApplications(apprenticeshipApplications);

            DeleteTraineeshipApplications(traineeshipApplications);

            DeleteSavedSearches(savedSearches);

            DeleteCandidate(candidate);

            DeleteUser(user);

            return true;
        }

        private void Audit(User user, Candidate candidate, IList<SavedSearch> savedSearches, IList<ApprenticeshipApplicationSummary> apprenticeshipApplications, IList<TraineeshipApplicationSummary> traineeshipApplications)
        {
            var candidateUser = new
                {
                    User = user,
                    Candidate = candidate,
                    SavedSearches = savedSearches,
                    ApprenticeshipApplications = apprenticeshipApplications,
                    TraineeshipApplications = traineeshipApplications
                };

            _auditRepository.Audit(candidateUser, AuditEventTypes.HardDeleteCandidateUser, user.EntityId);
        }

        private void DeleteApprenticeshipApplications(IList<ApprenticeshipApplicationSummary> apprenticeshipApplications)
        {
            if (apprenticeshipApplications != null)
            {
                foreach (var apprenticeshipApplicationSummary in apprenticeshipApplications)
                {
                    _logService.Info("Hard deleting Apprenticeship Application: {0}", apprenticeshipApplicationSummary.ApplicationId);

                    _apprenticeshipApplicationWriteRepository.Delete(apprenticeshipApplicationSummary.ApplicationId);

                    _logService.Info("Hard deleted Apprenticeship Application: {0}", apprenticeshipApplicationSummary.ApplicationId);
                }
            }
        }

        private void DeleteTraineeshipApplications(IList<TraineeshipApplicationSummary> traineeshipApplications)
        {
            if (traineeshipApplications != null)
            {
                foreach (var traineeshipApplicationSummary in traineeshipApplications)
                {
                    _logService.Info("Hard deleting Traineeship Application: {0}", traineeshipApplicationSummary.ApplicationId);

                    _traineeshipApplicationWriteRepository.Delete(traineeshipApplicationSummary.ApplicationId);

                    _logService.Info("Hard deleted Traineeship Application: {0}", traineeshipApplicationSummary.ApplicationId);
                }
            }
        }

        private void DeleteSavedSearches(IList<SavedSearch> savedSearches)
        {
            if (savedSearches != null)
            {
                foreach (var savedSearch in savedSearches)
                {
                    _logService.Info("Hard deleting Saved Search: {0}", savedSearch.EntityId);

                    _savedSearchWriteRepository.Delete(savedSearch.EntityId);

                    _logService.Info("Hard deleted Saved Search: {0}", savedSearch.EntityId);
                }
            }
        }

        private void DeleteCandidate(Candidate candidate)
        {
            if (candidate != null)
            {
                _logService.Info("Hard deleting Candidate: {0}", candidate.EntityId);

                _candidateWriteRepository.Delete(candidate.EntityId);

                _logService.Info("Hard deleted Candidate: {0}", candidate.EntityId);
            }
        }

        private void DeleteUser(User user)
        {
            _logService.Info("Hard deleting User Credentials: {0}", user.EntityId);

            _authenticationRepository.Delete(user.EntityId);

            _logService.Info("Hard deleted User Credentials: {0}", user.EntityId);

            _logService.Info("Hard deleting User: {0}", user.EntityId);

            _userWriteRepository.Delete(user.EntityId);

            _logService.Info("Hard deleted User: {0}", user.EntityId);
        }
    }
}