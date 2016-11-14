namespace SFA.Apprenticeships.Application.Candidate.Strategies.Apprenticeships
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;

    public class SaveApprenticeshipApplicationStrategy : ISaveApprenticeshipApplicationStrategy
    {
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICandidateWriteRepository _candidateWriteRepository;

        public SaveApprenticeshipApplicationStrategy(
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository,
            ICandidateReadRepository candidateReadRepository,
            ICandidateWriteRepository candidateWriteRepository)
        {
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
            _candidateReadRepository = candidateReadRepository;
            _candidateWriteRepository = candidateWriteRepository;
        }

        public ApprenticeshipApplicationDetail SaveApplication(Guid candidateId, int vacancyId, ApprenticeshipApplicationDetail apprenticeshipApplication)
        {
            var currentApplication = _apprenticeshipApplicationReadRepository.GetForCandidate(candidateId, vacancyId, true);

            currentApplication.AssertState("Save apprenticeship application", ApplicationStatuses.Draft);

            currentApplication.CandidateInformation = apprenticeshipApplication.CandidateInformation;
            currentApplication.AdditionalQuestion1Answer = apprenticeshipApplication.AdditionalQuestion1Answer;
            currentApplication.AdditionalQuestion2Answer = apprenticeshipApplication.AdditionalQuestion2Answer;

            var savedApplication = _apprenticeshipApplicationWriteRepository.Save(currentApplication);

            SyncToCandidateApplicationTemplate(candidateId, savedApplication);

            return savedApplication;
        }

        private void SyncToCandidateApplicationTemplate(Guid candidateId, ApprenticeshipApplicationDetail apprenticeshipApplication)
        {
            var candidate = _candidateReadRepository.Get(candidateId);

            candidate.ApplicationTemplate.AboutYou = apprenticeshipApplication.CandidateInformation.AboutYou;
            candidate.ApplicationTemplate.EducationHistory = apprenticeshipApplication.CandidateInformation.EducationHistory;
            candidate.ApplicationTemplate.Qualifications = apprenticeshipApplication.CandidateInformation.Qualifications;
            candidate.ApplicationTemplate.WorkExperience = apprenticeshipApplication.CandidateInformation.WorkExperience;
            candidate.ApplicationTemplate.TrainingCourses = apprenticeshipApplication.CandidateInformation.TrainingCourses;

            if (!candidate.MonitoringInformation.DisabilityStatus.HasValue &&
                apprenticeshipApplication.CandidateInformation.DisabilityStatus.HasValue)
            {
                candidate.MonitoringInformation.DisabilityStatus = apprenticeshipApplication.CandidateInformation.DisabilityStatus;
            }

            _candidateWriteRepository.Save(candidate);
        }
    }
}