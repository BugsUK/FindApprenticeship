namespace SFA.Apprenticeships.Application.Candidate.Strategies.Traineeships
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;

    public class SaveTraineeshipApplicationStrategy : ISaveTraineeshipApplicationStrategy
    {
        private readonly ITraineeshipApplicationWriteRepository _traineeshipApplicationWriteRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICandidateWriteRepository _candidateWriteRepository;

        public SaveTraineeshipApplicationStrategy(
            ITraineeshipApplicationWriteRepository traineeshipApplicationWriteRepository,
            ICandidateReadRepository candidateReadRepository,
            ICandidateWriteRepository candidateWriteRepository)
        {
            _traineeshipApplicationWriteRepository = traineeshipApplicationWriteRepository;
            _candidateReadRepository = candidateReadRepository;
            _candidateWriteRepository = candidateWriteRepository;
        }

        public TraineeshipApplicationDetail SaveApplication(
            Guid candidateId, int vacancyId, TraineeshipApplicationDetail traineeshipApplication)
        {
            // Set date applied when saving a traineeship application. Unlike apprenticeship applications, there are no intermediate states,
            // Draft, Submitting etc.
            traineeshipApplication.DateApplied = DateTime.UtcNow;

            var savedApplication = _traineeshipApplicationWriteRepository.Save(traineeshipApplication);

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