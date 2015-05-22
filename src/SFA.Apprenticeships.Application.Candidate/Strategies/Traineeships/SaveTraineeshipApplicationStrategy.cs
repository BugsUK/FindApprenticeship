namespace SFA.Apprenticeships.Application.Candidate.Strategies.Traineeships
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;

    public class SaveTraineeshipApplicationStrategy : ISaveTraineeshipApplicationStrategy
    {
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        private readonly ITraineeshipApplicationWriteRepository _traineeshipApplicationWriteRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICandidateWriteRepository _candidateWriteRepository;

        public SaveTraineeshipApplicationStrategy(
            ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository,
            ITraineeshipApplicationWriteRepository traineeshipApplicationWriteRepository,
            ICandidateReadRepository candidateReadRepository,
            ICandidateWriteRepository candidateWriteRepository)
        {
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
            _traineeshipApplicationWriteRepository = traineeshipApplicationWriteRepository;
            _candidateReadRepository = candidateReadRepository;
            _candidateWriteRepository = candidateWriteRepository;
        }

        public TraineeshipApplicationDetail SaveApplication(
            Guid candidateId, int vacancyId, TraineeshipApplicationDetail traineeshipApplication)
        {
            var currentApplication = _traineeshipApplicationReadRepository.GetForCandidate(candidateId, vacancyId, true);

            currentApplication.CandidateInformation = traineeshipApplication.CandidateInformation;
            currentApplication.AdditionalQuestion1Answer = traineeshipApplication.AdditionalQuestion1Answer;
            currentApplication.AdditionalQuestion2Answer = traineeshipApplication.AdditionalQuestion2Answer;

            // Set date applied when saving a traineeship application. Unlike apprenticeship applications, there are no intermediate states,
            // Draft, Submitting etc.
            currentApplication.DateApplied = DateTime.UtcNow;

            var savedApplication = _traineeshipApplicationWriteRepository.Save(currentApplication);

            SyncToCandidatesApplicationTemplate(candidateId, savedApplication);

            return savedApplication;
        }

        private void SyncToCandidatesApplicationTemplate(Guid candidateId, TraineeshipApplicationDetail traineeshipApplication)
        {
            var candidate = _candidateReadRepository.Get(candidateId);

            candidate.ApplicationTemplate.Qualifications = traineeshipApplication.CandidateInformation.Qualifications;
            candidate.ApplicationTemplate.WorkExperience = traineeshipApplication.CandidateInformation.WorkExperience;
            candidate.ApplicationTemplate.TrainingHistory = traineeshipApplication.CandidateInformation.TrainingHistory;

            if (!candidate.MonitoringInformation.DisabilityStatus.HasValue &&
                traineeshipApplication.CandidateInformation.DisabilityStatus.HasValue)
            {
                candidate.MonitoringInformation.DisabilityStatus = traineeshipApplication.CandidateInformation.DisabilityStatus;
            }

            _candidateWriteRepository.Save(candidate);
        }
    }
}