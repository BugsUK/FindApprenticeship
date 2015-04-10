﻿namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using System.Linq;
    using Apprenticeships;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;
    using Interfaces.Logging;
    using Interfaces.Users;

    public class SaveCandidateStrategy : ISaveCandidateStrategy
    {
        private readonly ILogService _logger;

        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly ICodeGenerator _codeGenerator;
        private readonly ISendMobileVerificationCodeStrategy _sendMobileVerificationCodeStrategy;
        private readonly IApprenticeshipApplicationWriteRepository _apprenticeshipApplicationWriteRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICandidateWriteRepository _candidateWriteRepository;
        private readonly IGetCandidateApprenticeshipApplicationsStrategy _getCandidateApplicationsStrategy;

        public SaveCandidateStrategy(ICandidateWriteRepository candidateWriteRepository,
            IGetCandidateApprenticeshipApplicationsStrategy getCandidateApplicationsStrategy,
            ICandidateReadRepository candidateReadRepository,
            IApprenticeshipApplicationWriteRepository apprenticeshipApplicationWriteRepository,
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            ICodeGenerator codeGenerator,
            ISendMobileVerificationCodeStrategy sendMobileVerificationCodeStrategy,
            ILogService logger)
        {
            _candidateWriteRepository = candidateWriteRepository;
            _getCandidateApplicationsStrategy = getCandidateApplicationsStrategy;
            _candidateReadRepository = candidateReadRepository;
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _codeGenerator = codeGenerator;
            _sendMobileVerificationCodeStrategy = sendMobileVerificationCodeStrategy;
            _logger = logger;
        }

        public Candidate SaveCandidate(Candidate candidate)
        {
            if (candidate.MobileVerificationRequired())
            {
                candidate.CommunicationPreferences.MobileVerificationCode = _codeGenerator.GenerateNumeric();
                _sendMobileVerificationCodeStrategy.SendMobileVerificationCode(candidate);
            }

            var result = _candidateWriteRepository.Save(candidate);
            var reloadedCandidate = _candidateReadRepository.Get(candidate.EntityId);

            var candidateApplications = _getCandidateApplicationsStrategy
                .GetApplications(candidate.EntityId)
                .Where(a => a.Status == ApplicationStatuses.Draft || a.Status == ApplicationStatuses.Saved)
                .ToList();

            candidateApplications.ForEach(candidateApplication =>
            {
                try
                {
                    UpdateApprenticeshipApplicationDetail(reloadedCandidate, candidateApplication.LegacyVacancyId);
                }
                catch (Exception e)
                {
                    // try updating the next one
                    var message = string.Format(
                        "Error while updating a draft application with the updated user personal details for user {0}",
                        candidate.EntityId);
                    _logger.Warn(message, e);
                }
            });

            return result;
        }

        private void UpdateApprenticeshipApplicationDetail(Candidate candidate, int vacancyId)
        {
            var apprenticeshipApplicationDetail = _apprenticeshipApplicationReadRepository.GetForCandidate(candidate.EntityId, vacancyId);

            if (apprenticeshipApplicationDetail != null)
            {
                apprenticeshipApplicationDetail.CandidateDetails = candidate.RegistrationDetails;
                _apprenticeshipApplicationWriteRepository.Save(apprenticeshipApplicationDetail);
            }
        }
    }
}
